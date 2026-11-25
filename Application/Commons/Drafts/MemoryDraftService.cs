using Microsoft.Extensions.Caching.Memory;

namespace Application.Commons.Drafts;

internal class MemoryDraftService<TDraft> : IDraftService<TDraft>
    where TDraft : class, IDraft
{
    //5 mins
    private static readonly TimeSpan cacheDuration = TimeSpan.FromMinutes(5);

    private readonly IMemoryCache _drafts;

    public MemoryDraftService(IMemoryCache drafts)
    {
        _drafts = drafts;
    }

    public async Task<TDraft> Add(TDraft draft)
    {
        //for async compatibility only
        await Task.CompletedTask;

        _drafts.Set(draft.Id, draft, cacheDuration);
        return draft;
    }

    public async Task Remove(Guid id)
    {
        //for async compatibility only
        await Task.CompletedTask;
        _drafts.Remove(id);
    }

    public async Task<TDraft> Get(Guid id)
    {
        //for async compatibility only
        await Task.CompletedTask;

        if (_drafts.TryGetValue(id, out TDraft? value))
        {
            return value;
        }

        return null;
    }
}
