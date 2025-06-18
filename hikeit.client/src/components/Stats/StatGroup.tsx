import { KeyToLabelFormatter } from "@/Utils/Formatters/valueFormatter";
import { For, Heading, Stack } from "@chakra-ui/react";
import RowStat from "./RowStat";
import type { StatType } from "./_typesStats";
import GetStatsMeta from "@/Utils/GetStatsMeta";

interface Props {
  stats: StatType[];
  groupLabel?: string;
}

function StatGroup({ stats, groupLabel }: Props) {
  return (
    <Stack direction={{ base: "column", lg: "row" }}>
      <Heading fontSize={"5xl"}>
        {groupLabel && KeyToLabelFormatter(groupLabel)}
      </Heading>

      <For each={stats} children={Stat} />
    </Stack>
  );
}

function Stat(stat: StatType) {
  const addons = GetStatsMeta(stat.key);

  return <RowStat value={stat.value} label={stat.label} addons={addons} />;
}

export default StatGroup;
