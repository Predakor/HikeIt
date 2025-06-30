import { dateOnlyToString } from "@/Utils/Formatters/dateFormats";
import type { PeaksAnalyticData } from "../../Types/TripAnalyticsTypes";
import type { ReachedPeakWithBadges } from "@/components/AddFile/AddFile/tripTypes";

function usePeakAnalytics(data: PeaksAnalyticData) {
  let highest = data.reached.reduce((max, item) =>
    item.height > max.height ? item : max
  ) as ReachedPeakWithBadges;

  const peaks: ReachedPeakWithBadges[] = data.reached
    .concat(data.reached)
    .concat(data.reached)
    .concat(data.reached)
    .concat(data.reached)
    .concat(data.reached)
    .map((p) => ({
      ...p,
      firstTime: Math.random() * 10 > 7,
      reachedAt: dateOnlyToString(new Date().toISOString().split("T")[0]),
      isHighest: Math.random() * 10 > 7,
    }));

  highest = {
    ...highest,
    isHighest: true,
    reachedAt: dateOnlyToString(new Date().toISOString().split("T")[0]),
  };

  return { highest, peaks };
}
export default usePeakAnalytics;
