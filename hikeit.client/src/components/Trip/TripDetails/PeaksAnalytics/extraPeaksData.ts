import { toUkDate } from "@/Utils/Formatters/dateFormatter";
import type { ReachedPeakWithBadges } from "@/components/AddFile/AddFile/tripTypes";
import type { PeaksAnalytics } from "@/types/ApiTypes/Analytics";

export default function extraPeaksData(data: PeaksAnalytics) {
  let highest = data.reached.reduce((max, item) =>
    item.height > max.height ? item : max
  ) as ReachedPeakWithBadges;

  const peaks: ReachedPeakWithBadges[] = data.reached.map((p) => ({
    ...p,
    firstTime: Math.random() * 10 > 7,
    reachedAt: toUkDate(new Date().toISOString().split("T")[0]),
    isHighest: Math.random() * 10 > 7,
  }));

  highest = {
    ...highest,
    isHighest: true,
    reachedAt: toUkDate(new Date().toISOString().split("T")[0]),
  };

  return { highest, peaks };
}
