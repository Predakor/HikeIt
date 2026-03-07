import { GenericFormatter } from "@/Utils/Formatters/valueFormatter";
import { arrayUtils } from "@/Utils/arrayUtils";
import RowStat from "@/components/Stats/RowStat";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import type { StatAddons } from "@/types/Utils/stat.types";
import { Flex, Stack } from "@chakra-ui/react";
import type { RouteAnalyticsProps } from "../../RouteAnalytics";
import { ElevationGainPerUnit } from "./ElevationGainPerUnit";

export function SlopeSection({ data }: RouteAnalyticsProps) {
  const averageSlope = arrayUtils.average(
    [data.averageAscentSlopePercent, data.averageDescentSlopePercent],
    Math.abs
  );

  const slopes = {
    ascent: data.averageAscentSlopePercent,
    descent: data.averageDescentSlopePercent,
    average: averageSlope,
  };

  const addons = {
    unit: "%",
    formatt: GenericFormatter,
  } satisfies StatAddons<any>;

  return (
    <SimpleCard title="Slopes">
      <Stack gapY={8}>
        <Flex>
          <RowStat
            value={slopes.descent}
            label={"Average Descent"}
            addons={addons}
          />

          <RowStat
            value={slopes.ascent}
            label={"Average Ascent"}
            addons={addons}
          />
        </Flex>
        <ElevationGainPerUnit base={slopes.average} />
      </Stack>
    </SimpleCard>
  );
}
