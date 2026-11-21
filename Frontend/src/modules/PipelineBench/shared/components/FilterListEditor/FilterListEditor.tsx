import { Button, Flex, For, Menu, Portal, Show, Stack, Text } from "@chakra-ui/react";
import { useContext, type ReactNode } from "react";
import { FillHeightContext } from "../../context/fillHeightContext";
import { IconPlus } from "@/Icons/Icons";
import {
  createFilterItem,
  fieldsOf,
  filterTypesOf,
  hasSimplifyBeforePinLocalMaxima,
  withFieldValue,
  type FilterFieldValue,
  type FilterItem,
  type FilterKind,
} from "./filterCatalog";
import { FilterRow } from "./FilterRow";

// Only the list of filters scrolls; the header, the add / reset / apply row and the result info stay in view.
const listMaxHeight = "15rem";

interface FilterListEditorProps<TFilter extends FilterItem> {
  kind: FilterKind;
  filters: TFilter[] | null | undefined;
  defaultFilters: TFilter[];
  onChange: (filters: TFilter[] | null) => void;
  actions?: ReactNode;
}

export function FilterListEditor<TFilter extends FilterItem>({
  kind,
  filters,
  defaultFilters,
  onChange,
  actions,
}: FilterListEditorProps<TFilter>) {
  const effectiveFilters = filters ?? defaultFilters;
  const isFillingHeight = useContext(FillHeightContext);
  const hasOrderWarning = hasSimplifyBeforePinLocalMaxima(effectiveFilters);

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
          SimplifyFilter runs before PinLocalMaximaFilter, so summits can be simplified away before
          they are pinned.
        </Text>
      </Show>

      <Show when={effectiveFilters.length === 0}>
        <Text fontSize={"sm"} color={"fg.muted"}>
          No filters: points pass through unchanged.
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
