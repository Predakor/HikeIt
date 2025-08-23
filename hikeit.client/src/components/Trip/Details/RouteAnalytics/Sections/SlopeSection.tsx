import { IconTrendUp } from "@/Icons/Icons";
import { GenericFormatter } from "@/Utils/Formatters/valueFormatter";
import { arrayUtils } from "@/Utils/arrayUtils";
import RowStat from "@/components/Stats/RowStat";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import type { LengthUnit, StatAddons } from "@/types/Utils/stat.types";
import {
  Button,
  ButtonGroup,
  Flex,
  For,
  Separator,
  Span,
  Stack,
  Strong,
  Text,
} from "@chakra-ui/react";
import { useState } from "react";
import type { RouteAnalyticsProps } from "../RouteAnalytics";

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
  } satisfies StatAddons;

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

function ElevationGainPerUnit({ base }: { base: number }) {
  const [unit, setUnit] = useState<LengthUnit>("m");
  const [distance, setDistance] = useState(100);

  const handleUnitChange = (unit: LengthUnit, value: number) => {
    setUnit(unit);
    setDistance(value);
  };

  const getScaledRatio = (slope: number) => {
    // slope is meters per 100m
    const baseUnitMagnitude = 100;
    const meters = (slope / baseUnitMagnitude) * distance;
    const apliedUnit = meters > 1000 ? "km" : "m";
    const finalValue = (meters / unitRatios[apliedUnit]).toFixed(1);
    return `${finalValue}${apliedUnit}`;
  };

  const unitRatios = {
    m: 1,
    km: 1000,
  } as const;

  const buttonsData = [
    { value: 100, unit: "m" },
    { value: 1000, unit: "km" },
    { value: 10000, unit: "km" },
  ] as const;

  return (
    <Stack align="center" gap={2}>
      <Text color="gray.400">Average Elevation change</Text>

      <Flex align="baseline" gap={2}>
        <IconTrendUp />
        <Strong fontSize="2xl">{getScaledRatio(base)}</Strong>
        <Span fontSize="md" color="gray.500">
          {`/ ${distance / unitRatios[unit]} ${unit}`}
        </Span>
      </Flex>

      <Separator />

      <ButtonGroup>
        <For each={buttonsData}>
          {(data) => (
            <Button
              size={"xs"}
              variant={distance === data.value ? "solid" : "outline"}
              colorPalette={"green"}
              onClick={() => handleUnitChange(data.unit, data.value)}
            >
              {`${data.value / unitRatios[data.unit]}${data.unit}`}
            </Button>
          )}
        </For>
      </ButtonGroup>
    </Stack>
  );
}
