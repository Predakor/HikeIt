import { ObjectToArray, ToTitleCase } from "@/Utils/ObjectToArray";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import type { InputConfigEntry } from "@/components/Utils/RenderInputs/inputTypes";
import { PrimaryButton, SecondaryButton } from "@/components/ui/Buttons";
import { For, GridItem, SimpleGrid, Stack } from "@chakra-ui/react";
import type { Feature, LineString } from "geojson";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { useParams } from "react-router";
import { useVisualisationPreview } from "./useRouteVisualizationMutations";
import type { FilterTypes } from "./routeVisualization.types";

export const filterTypeToFieldConfigMap: Record<FilterTypes, InputConfigEntry> = {
  medianSmoothingFilter: {
    key: "medianSmoothingFilter",
    label: "",
    type: "range",
    min: 3,
    max: 10,
  },
  emaSmoothingFilter: {
    key: "emaSmoothingFilter",
    label: "",
    type: "range",
    min: 0.1,
    max: 0.4,
    step: 0.01,
  },
  maxSpikeFilter: { key: "maxSpikeFilter", label: "", type: "range", min: 0, max: 10 },
  roundingPrecisionFilter: {
    key: "roundingPrecisionFilter",
    label: "",
    type: "range",
    min: 0,
    max: 10,
  },
};

export const valueToConfigMap: Record<FilterTypes, (value: any) => any> = {
  medianSmoothingFilter: (value) => ({ WindowSize: value }),
  emaSmoothingFilter: (value) => ({ Alpha: value }),
  maxSpikeFilter: (value) => ({ MaxSpike: value }),
  roundingPrecisionFilter: (value) => ({ Decimals: value }),
};

export const avaiableFilters = [
  "medianSmoothingFilter",
  "emaSmoothingFilter",
  "maxSpikeFilter",
  "roundingPrecisionFilter",
] as Array<FilterTypes>;

interface Props {
  onFilterSubmit: (r: Feature<LineString>) => any;
}
export function VisualisationFilterMenu({ onFilterSubmit }: Props) {
  const { tripId } = useParams();
  const [filters, setFilters] = useState<InputConfigEntry[]>([]);

  const previewMutation = useVisualisationPreview({
    tripId: tripId,
    onSuccess: onFilterSubmit,
  });

  const loadPreview = () => {
    const routeDataFilters = filtersHook.getValues();
    const proValueList = ObjectToArray(routeDataFilters);

    const parsedFilters = proValueList.map(([key, value]) => ({
      FilterType: ToTitleCase(key),
      name: key,
      value: valueToConfigMap[key as FilterTypes](value),
    }));

    previewMutation.mutateAsync(parsedFilters as any);
  };

  const filtersHook = useForm({});
  return (
    <Stack
      direction={{
        lg: "row",
      }}
    >
      <SimpleGrid columns={2} gap={"4"} maxWidth={"sm"} alignContent={"start"}>
        <GridItem colSpan={2} asChild>
          <Stack>
            <RenderInputs config={filters} formHook={filtersHook} />
          </Stack>
        </GridItem>
        <For each={avaiableFilters}>
          {(filterType) => (
            <SecondaryButton
              variant={"outline"}
              onClick={() => setFilters((p) => [...p, filterTypeToFieldConfigMap[filterType]])}
            >
              {filterType}
            </SecondaryButton>
          )}
        </For>
        <GridItem colSpan={2} asChild>
          <PrimaryButton onClick={() => loadPreview()}>Preview</PrimaryButton>
        </GridItem>
      </SimpleGrid>
    </Stack>
  );
}
