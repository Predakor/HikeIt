import { DurationAndCoverage } from "./Duration/DurationAndCoverage";
import { SpeedSection } from "./Speed/SpeedSection";
import { TimeDistributionSection } from "./TimeDistribution/TimeDistributionSection";
import { TimeframeSection } from "./TimeFrame/TimeframeSection";

const TimeAnalyticSection = {
  TimeDistribution: TimeDistributionSection,
  TimeFrame: TimeframeSection,
  Speed: SpeedSection,
  Duration: DurationAndCoverage,
};

export default TimeAnalyticSection;
