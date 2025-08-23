import type { ReachedPeakWithBadges } from "@/components/ui/AddFile/tripTypes";
import Peak from "../Peak";
import ReachDistance from "./ReachDistance";
import ReachTime from "./ReachTime";
import { PeakBadge } from "@/components/Trip/Details/Common/PeakBadge";

export default function ReachedPeak({ peak }: { peak: ReachedPeakWithBadges }) {
  const { firstTime, reachedAt } = peak;

  const mockPeak = {
    ...peak,
    id: 1,
  };

  return (
    <>
      <Peak peak={mockPeak}></Peak>
      <ReachTime reachTime={new Date(reachedAt!)} />
      {/* show when there is a distance markup */}
      <ReachDistance reachDistance={1500} />
      {firstTime && <PeakBadge text={"First time reached"} />}
    </>
  );
}
