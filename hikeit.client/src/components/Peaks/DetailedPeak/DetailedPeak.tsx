import RowStat from "@/components/Stats/RowStat";
import { PeakIcon } from "@/components/Trip/Details/Common/PeakIcon";
import type { Peak } from "@/types/ApiTypes/peak.types";
import { Flex, Heading } from "@chakra-ui/react";

function DetailedPeak({ peak }: { peak: Peak }) {
  return (
    <>
      <Flex align={"center"} gapX={4}>
        <PeakIcon />
        <Heading fontSize={"2xl"}>{peak.name}</Heading>
      </Flex>
      <RowStat label="Height" value={peak.height} addons={{ unit: "m" }} />
    </>
  );
}
export default DetailedPeak;
