import type { ReachedPeakWithBadges } from "@/components/AddFile/AddFile/tripTypes";
import { PeakBadge } from "@/components/Trip/TripDetails/Common/PeakBadge";
import Peak from "../Peak";
import ReachDistance from "./ReachDistance";
import ReachTime from "./ReachTime";

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
