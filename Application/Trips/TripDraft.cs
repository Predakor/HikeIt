using Application.Commons.Drafts;
using Domain.TripAnalytics;
using Domain.Trips;

namespace Application.Trips;

public class TripDraft : IDraft {
    public Guid Id => Trip.Id;
    public Guid UserId => Trip.UserId;
    public Trip Trip { get; private set; }
    public DraftState State { get; private set; }

    readonly Queue<Func<Trip, Task>> _tasks = new();

    readonly AsyncTaskLock _waitForEmptyQueue = new();
    readonly AsyncTaskLock _waitForTrip = new();

    TripDraft(Guid userId) {
        Guid tripId = Guid.NewGuid();
        Trip = Trip.Create(tripId, "", DateOnly.MinValue, userId);

        _waitForEmptyQueue.Lock();
        _waitForTrip.Lock();
    }

    async Task RunQueueAsync() {
        await _waitForTrip.AwaitUnlock();

        if (State == DraftState.Processing) {
            return;
        }

        while (_tasks.TryDequeue(out var task)) {
            State = DraftState.Processing;
            await task(Trip);
        }

        State = DraftState.Iddle;
        _waitForEmptyQueue.Unlock();
    }

    public async Task<Trip> AwaitAll() {
        await _waitForEmptyQueue.AwaitUnlock();

        return Trip;
    }

    public TripDraft AddTask(Func<Trip, Task> mutation) {
        _tasks.Enqueue(mutation);

        if (_tasks.Count == 1) {
            _ = RunQueueAsync();
        }

        return this;
    }

    public TripDraft AddAnalytics(TripAnalytic analytics) {
        _waitForTrip.Unlock();

        AddTask(
            async (trip) => {
                await Task.CompletedTask;
                trip.AddAnalytics(analytics);
            }
        );
        return this;
    }

    public static TripDraft Create(Guid userId) {
        return new TripDraft(userId);
    }
}

public class AsyncTaskLock {
    TaskCompletionSource<bool>? _gate = null;

    public Task AwaitUnlock() {
        if (_gate != null) {
            return _gate.Task;
        }
        return Task.CompletedTask;
    }

    public void Unlock() {
        _gate?.TrySetResult(true);
        _gate = null;
    }

    public void Lock() {
        if (_gate is null || _gate.Task.IsCompleted) {
            _gate = new TaskCompletionSource<bool>(
                TaskCreationOptions.RunContinuationsAsynchronously
            );
        }
    }
}

public enum DraftState {
    None,
    Iddle,
    Processing,
    Complete,
    Canceled,
}
