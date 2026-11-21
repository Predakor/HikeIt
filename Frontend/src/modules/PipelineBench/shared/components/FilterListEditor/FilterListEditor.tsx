import { Button, Flex, For, Menu, Portal, Show, Stack, Text } from "@chakra-ui/react";
import { useContext, type ReactNode } from "react";
import { FillHeightContext } from "../../context/fillHeightContext";
import { IconPlus } from "@/Icons/Icons";
import type { PipeStageStepTiming } from "@/modules/PipelineBench/shared/types/PipeStageDto";
import {
  createFilterItem,
  fieldsOf,
  filterTypesOf,
  hasGradeBeforeIdle,
  hasSimplifyBeforePinLocalMaxima,
  withFieldValue,
  type FilterFieldValue,
  type FilterItem,
  type FilterKind,
} from "./filterCatalog";
import { FilterRow } from "./FilterRow";

interface OrderWarning {
  isTriggered: (filters: FilterItem[]) => boolean;
  message: string;
}

const orderWarningByKind: Partial<Record<FilterKind, OrderWarning>> = {
  point: {
    isTriggered: hasSimplifyBeforePinLocalMaxima,
    message:
      "SimplifyFilter runs before PinLocalMaximaFilter, so summits can be simplified away before they are pinned.",
  },
  pace: {
    isTriggered: hasGradeBeforeIdle,
    message:
      "GradeFilter runs before IdleFilter, so standing still on a slope can be counted as climbing.",
  },
};

const emptyTextByKind: Record<FilterKind, string> = {
  point: "No filters: points pass through unchanged.",
  peak: "No filters: points pass through unchanged.",
  pace: "No filters: every segment counts as flat and the ascent, descent and idle times are not split out.",
  terrain: "No filters: ascent and descent are plain sums, and the steepest and longest segments are not computed.",
};

// Only the list of filters scrolls; the header, the add / reset / apply row and the result info stay in view.
const listMaxHeight = "15rem";

interface FilterListEditorProps<TFilter extends FilterItem> {
  kind: FilterKind;
  filters: TFilter[] | null | undefined;
  defaultFilters: TFilter[];
  onChange: (filters: TFilter[] | null) => void;
  stepTimings?: PipeStageStepTiming[] | null;
  actions?: ReactNode;
}

export function FilterListEditor<TFilter extends FilterItem>({
  kind,
  filters,
  defaultFilters,
  onChange,
  stepTimings,
  actions,
}: FilterListEditorProps<TFilter>) {
  const effectiveFilters = filters ?? defaultFilters;
  const isFillingHeight = useContext(FillHeightContext);
  const orderWarning = orderWarningByKind[kind];
  const hasOrderWarning = orderWarning?.isTriggered(effectiveFilters) === true;

  // Steps come back in run order; a name mismatch means the list was edited since the run.
  const durationOf = (index: number, filter: TFilter) => {
    const step = stepTimings?.[index];

    return step?.name === filter.filterType ? step.durationMs : undefined;
  };

  const addFilter = (filterType: string) => {
    const created = createFilterItem(filterType, fieldsOf(kind, filterType)) as TFilter;
    onChange([...effectiveFilters, created]);
  };

  const moveFilter = (index: number, delta: number) => {
    const targetIndex = index + delta;
    const isOutOfRange = targetIndex < 0 || targetIndex >= effectiveFilters.length;

    if (isOutOfRange) {
      return;
    }

    const next = [...effectiveFilters];
    [next[index], next[targetIndex]] = [next[targetIndex], next[index]];
    onChange(next);
  };

  const removeFilter = (index: number) => {
    onChange(effectiveFilters.filter((_, position) => position !== index));
  };

  const changeValue = (index: number, name: string, value: FilterFieldValue) => {
    const filter = effectiveFilters[index];
    const next = [...effectiveFilters];
    next[index] = withFieldValue(filter, name, value) as TFilter;
    onChange(next);
  };

  return (
    <Stack gap={2} flex={isFillingHeight ? 1 : undefined} minH={0}>
      <Show when={hasOrderWarning}>
        <Text fontSize={"xs"} color={"fg.warning"}>
          {orderWarning?.message}
        </Text>
      </Show>

      <Show when={effectiveFilters.length === 0}>
        <Text fontSize={"sm"} color={"fg.muted"}>
          {emptyTextByKind[kind]}
        </Text>
      </Show>

      <Stack
        gap={2}
        flex={isFillingHeight ? 1 : undefined}
        minH={0}
        maxH={isFillingHeight ? undefined : listMaxHeight}
        overflowY={"auto"}
        p={1}
      >
        <For each={effectiveFilters}>
          {(filter, index) => (
            <FilterRow
              key={`${index}-${filter.filterType}`}
              kind={kind}
              filter={filter}
              index={index}
              count={effectiveFilters.length}
              durationMs={durationOf(index, filter)}
              onMove={(delta) => moveFilter(index, delta)}
              onRemove={() => removeFilter(index)}
              onValueChange={(name, value) => changeValue(index, name, value)}
            />
          )}
        </For>

        <Flex>
          <Menu.Root onSelect={(details) => addFilter(details.value)}>
            <Menu.Trigger asChild>
              <Button
                size={"sm"}
                variant={"outline"}
                colorPalette={"blue"}
                w={"full"}
                borderStyle={"dashed"}
                _hover={{ bg: "colorPalette.subtle" }}
              >
                <IconPlus />
                Add filter
              </Button>
            </Menu.Trigger>
            <Portal>
              <Menu.Positioner>
                <Menu.Content>
                  <For each={filterTypesOf(kind)}>
                    {(filterType) => (
                      <Menu.Item key={filterType} value={filterType}>
                        {filterType}
                      </Menu.Item>
                    )}
                  </For>
                </Menu.Content>
              </Menu.Positioner>
            </Portal>
          </Menu.Root>
        </Flex>
      </Stack>

      <Flex gap={2} wrap={"wrap"} align={"center"}>
        <Show when={filters != null}>
          <Button size={"sm"} variant={"ghost"} colorPalette={"red"} onClick={() => onChange(null)}>
            Reset to production default
          </Button>
        </Show>

        <Show when={actions}>
          <Flex ms={"auto"}>{actions}</Flex>
        </Show>
      </Flex>
    </Stack>
  );
}
