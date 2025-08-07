import type { TripSummaries } from "@/types/ApiTypes/TripDtos";

type YearGroups = Record<number, TripSummaries>;
type YearGroup = {
  year: number;
  trips: TripSummaries;
};

export default function sortTrips(trips: TripSummaries): YearGroup[] {
  const groupedByYear = GroupByYear(trips);
  const groupsSortedByYear = SortByYear(groupedByYear);

  return groupsSortedByYear;
}

function GroupByYear(trips: TripSummaries): YearGroups {
  return trips.reduce((groups: YearGroups, trip) => {
    const year = new Date(trip.tripDay).getUTCFullYear();

    if (!groups[year]) {
      groups[year] = [];
    }

    groups[year].push(trip);
    return groups;
  }, {});
}

function SortByYear(yearGroups: YearGroups): YearGroup[] {
  return Object.entries(yearGroups)
    .sort(([year1], [year2]) => Number(year2) - Number(year1))
    .map(([year, trips]) => ({
      year: Number(year),
      trips: SortTripsByDate(trips),
    }));
}

function SortTripsByDate(trips: TripSummaries): TripSummaries {
  return [...trips].sort((t1, t2) => {
    const date1 = new Date(t1.tripDay);
    const date2 = new Date(t2.tripDay);
    return date2.getTime() - date1.getTime();
  });
}
