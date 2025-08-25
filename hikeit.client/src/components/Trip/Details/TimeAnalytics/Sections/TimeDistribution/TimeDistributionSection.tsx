import { IconStop, IconTrendDown, IconTrendUp } from "@/Icons/Icons";
import { TimeSpan } from "@/Utils/Formatters/Duration/Duration";
import { RowStat } from "@/components/Stats";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import { For, Stack } from "@chakra-ui/react";
import { useState } from "react";
import type { Duration } from "../../TimeAnalytics";
import { SelectUnit } from "./SelectUnit";
import { TimeDistributionGraph } from "./TimeDistributionGraph";

export type AllowedUnit = "percentage" | "minutes" | "hours";

export function TimeDistributionSection({ duration }: { duration: Duration }) {
  const [unit, setUnit] = useState<AllowedUnit>("hours");

  const timeDistribution = {
    ascent: { value: duration.ascent, icon: IconTrendUp },
    descent: { value: duration.descent, icon: IconTrendDown },
    iddle: { value: duration.iddle, icon: IconStop },
  };

  //change it to use total time when api response is correct
  const total = TimeSpan.FromCombined([
    duration.ascent,
    duration.descent,
    duration.iddle,
  ]).toSeconds();

  const views = {
    percentage: (t: TimeSpan) => getPercentage(t.toSeconds(), total),
    minutes: (t: TimeSpan) => `${t.toMinutes()}`,
    hours: (t: TimeSpan) => t.toString(),
  };

  return (
    <SimpleCard
      title="Time distrubution"
      headerCta={<SelectUnit selectedItem={unit} onChange={setUnit} />}
    >
      <Stack
        direction={{ base: "column", lg: "row" }}
        align={{ base: "center" }}
      >
        <For each={Object.entries(timeDistribution)}>
          {([key, item]) => (
            <RowStat
              label={`${key} Time`}
              value={item.value}
              addons={{
                unit: getUnit[unit],
                formatt: (t: TimeSpan) => views[unit](t),
                IconSource: item.icon,
              }}
            />
          )}
        </For>
      </Stack>
      <TimeDistributionGraph duration={duration} total={total} />
    </SimpleCard>
  );
}

const getUnit = {
  hours: undefined,
  minutes: "min",
  percentage: "%",
} as const;

const getPercentage = (n: number, max: number) => {
  const percentage = (n / max) * 100;
  return percentage.toFixed(0);
};
