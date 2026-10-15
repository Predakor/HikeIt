import {
  Button,
  Flex,
  For,
  IconButton,
  Input,
  Menu,
  NativeSelect,
  Portal,
  Slider,
  Stack,
  Switch,
  Text,
} from "@chakra-ui/react";
import { useEffect, useState } from "react";
import { IconArrowDown, IconArrowUp } from "@/Icons/Icons";
import {
  createFilterItem,
  fieldsOf,
  filterTypesOf,
  hasSimplifyBeforePinLocalMaxima,
  readFieldValue,
  withFieldValue,
  type FilterField,
  type FilterFieldValue,
  type FilterItem,
  type FilterKind,
  type NumberField,
  type SelectField,
} from "./filterCatalog";

interface FilterListEditorProps<F extends FilterItem> {
  kind: FilterKind;
  filters: F[] | null | undefined;
  defaultFilters: F[];
  onChange: (filters: F[] | null) => void;
}

// filters == null means the production default list. Customising starts from defaultFilters.
export function FilterListEditor<F extends FilterItem>({
  kind,
  filters,
  defaultFilters,
  onChange,
}: FilterListEditorProps<F>) {
  if (filters == null) {
    return (
      <Flex align={"center"} justify={"space-between"} gap={3} wrap={"wrap"}>
        <Text fontSize={"sm"} color={"fg.muted"}>
          Production default
        </Text>
        <Button size={"sm"} variant={"outline"} onClick={() => onChange([...defaultFilters])}>
          Customize
        </Button>
      </Flex>
    );
  }

  const hasOrderWarning = hasSimplifyBeforePinLocalMaxima(filters);

  const addFilter = (filterType: string) => {
    const created = createFilterItem(filterType, fieldsOf(kind, filterType)) as F;
    onChange([...filters, created]);
  };

  const moveFilter = (index: number, delta: number) => {
    const targetIndex = index + delta;
    const isOutOfRange = targetIndex < 0 || targetIndex >= filters.length;

    if (isOutOfRange) {
      return;
    }

    const next = [...filters];
    [next[index], next[targetIndex]] = [next[targetIndex], next[index]];
    onChange(next);
  };

  const removeFilter = (index: number) => {
    onChange(filters.filter((_, position) => position !== index));
  };

  const changeValue = (index: number, name: string, value: FilterFieldValue) => {
    const filter = filters[index];
    const next = [...filters];
    next[index] = withFieldValue(filter, name, value) as F;
    onChange(next);
  };

  return (
    <Stack gap={2}>
      {hasOrderWarning ?
        <Text fontSize={"xs"} color={"fg.warning"}>
          SimplifyFilter runs before PinLocalMaximaFilter, so summits can be simplified away before
          they are pinned.
        </Text>
      : null}

      {filters.length === 0 ?
        <Text fontSize={"sm"} color={"fg.muted"}>
          No filters: points pass through unchanged.
        </Text>
      : null}

      <For each={filters}>
        {(filter, index) => (
          <FilterRow
            key={`${index}-${filter.FilterType}`}
            kind={kind}
            filter={filter}
            index={index}
            count={filters.length}
            onMove={(delta) => moveFilter(index, delta)}
            onRemove={() => removeFilter(index)}
            onValueChange={(name, value) => changeValue(index, name, value)}
          />
        )}
      </For>

      <Flex gap={2} wrap={"wrap"}>
        <Menu.Root onSelect={(details) => addFilter(details.value)}>
          <Menu.Trigger asChild>
            <Button size={"sm"} variant={"outline"}>
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

        <Button size={"sm"} variant={"ghost"} colorPalette={"red"} onClick={() => onChange(null)}>
          Reset to production default
        </Button>
      </Flex>
    </Stack>
  );
}

interface FilterRowProps {
  kind: FilterKind;
  filter: FilterItem;
  index: number;
  count: number;
  onMove: (delta: number) => void;
  onRemove: () => void;
  onValueChange: (name: string, value: FilterFieldValue) => void;
}

function FilterRow({
  kind,
  filter,
  index,
  count,
  onMove,
  onRemove,
  onValueChange,
}: FilterRowProps) {
  const fields = fieldsOf(kind, filter.FilterType);
  const isFirst = index === 0;
  const isLast = index === count - 1;

  return (
    <Flex
      align={"center"}
      gap={2}
      wrap={"wrap"}
      borderWidth={"1px"}
      borderRadius={"md"}
      px={3}
      py={2}
    >
      <Text fontSize={"sm"} fontWeight={"medium"} flex={1} minW={0}>
        {index + 1}. {filter.FilterType}
      </Text>

      <For each={fields}>
        {(field) => (
          <FieldInput
            key={field.name}
            field={field}
            value={readFieldValue(filter, field.name)}
            onValueChange={(value) => onValueChange(field.name, value)}
          />
        )}
      </For>

      <IconButton
        size={"xs"}
        variant={"ghost"}
        aria-label={"Move filter up"}
        disabled={isFirst}
        onClick={() => onMove(-1)}
      >
        <IconArrowUp />
      </IconButton>
      <IconButton
        size={"xs"}
        variant={"ghost"}
        aria-label={"Move filter down"}
        disabled={isLast}
        onClick={() => onMove(1)}
      >
        <IconArrowDown />
      </IconButton>
      <Button size={"xs"} variant={"ghost"} colorPalette={"red"} onClick={onRemove}>
        Remove
      </Button>
    </Flex>
  );
}

interface FieldInputProps {
  field: FilterField;
  value: FilterFieldValue | undefined;
  onValueChange: (value: FilterFieldValue) => void;
}

function FieldInput({ field, value, onValueChange }: FieldInputProps) {
  if (field.kind === "boolean") {
    return (
      <Switch.Root
        size={"sm"}
        checked={value === true}
        onCheckedChange={({ checked }) => onValueChange(checked)}
      >
        <Switch.HiddenInput />
        <Switch.Control />
        <Switch.Label>{field.name}</Switch.Label>
      </Switch.Root>
    );
  }

  if (field.kind === "select") {
    return (
      <SelectValueInput
        field={field}
        value={String(value ?? field.defaultValue)}
        onValueChange={onValueChange}
      />
    );
  }

  if (field.control === "range") {
    return <RangeValueInput field={field} value={Number(value)} onValueChange={onValueChange} />;
  }

  return <NumberValueInput field={field} value={Number(value)} onValueChange={onValueChange} />;
}

interface SelectValueInputProps {
  field: SelectField;
  value: string;
  onValueChange: (value: string) => void;
}

function SelectValueInput({ field, value, onValueChange }: SelectValueInputProps) {
  return (
    <Flex align={"center"} gap={1}>
      <Text fontSize={"xs"} color={"fg.muted"}>
        {field.name}
      </Text>
      <NativeSelect.Root size={"xs"} width={"28"}>
        <NativeSelect.Field value={value} onChange={(event) => onValueChange(event.target.value)}>
          {field.options.map((option) => (
            <option key={option} value={option}>
              {option}
            </option>
          ))}
        </NativeSelect.Field>
        <NativeSelect.Indicator />
      </NativeSelect.Root>
    </Flex>
  );
}

interface RangeValueInputProps {
  field: NumberField;
  value: number;
  onValueChange: (value: number) => void;
}

// Same Chakra slider parts as the shared Range input, driven by value/onChange since this editor is not a react-hook-form.
// The dragged value lives in local state and is committed when the drag ends, so a drag does not re-render the whole bench per tick.
function RangeValueInput({ field, value, onValueChange }: RangeValueInputProps) {
  const [draft, setDraft] = useState(value);

  useEffect(() => {
    setDraft(value);
  }, [value]);

  return (
    <Flex align={"center"} gap={2}>
      <Text fontSize={"xs"} color={"fg.muted"}>
        {field.name}
      </Text>
      <Slider.Root
        width={"28"}
        min={field.min}
        max={field.max}
        step={field.step}
        value={[draft]}
        onValueChange={({ value: next }) => setDraft(next[0])}
        onValueChangeEnd={({ value: next }) => onValueChange(next[0])}
      >
        <Slider.Control>
          <Slider.Track>
            <Slider.Range />
          </Slider.Track>
          <Slider.Thumbs />
        </Slider.Control>
      </Slider.Root>
      <Text fontSize={"xs"} minW={"10"} textAlign={"right"}>
        {draft}
      </Text>
    </Flex>
  );
}

interface NumberValueInputProps {
  field: NumberField;
  value: number;
  onValueChange: (value: number) => void;
}

// Keeps the raw text while typing ("1." is not a number yet), so the input is not reset mid-edit.
function NumberValueInput({ field, value, onValueChange }: NumberValueInputProps) {
  const [draft, setDraft] = useState(String(value));

  useEffect(() => {
    setDraft((previous) => (Number(previous) === value ? previous : String(value)));
  }, [value]);

  const handleChange = (raw: string) => {
    setDraft(raw);

    const parsed = Number(raw);
    const isValidNumber = raw !== "" && !Number.isNaN(parsed);

    if (isValidNumber) {
      onValueChange(parsed);
    }
  };

  return (
    <Flex align={"center"} gap={1}>
      <Text fontSize={"xs"} color={"fg.muted"}>
        {field.name}
      </Text>
      <Input
        type={"number"}
        size={"xs"}
        w={"20"}
        min={field.min}
        max={field.max}
        step={field.step}
        value={draft}
        onChange={(event) => handleChange(event.target.value)}
      />
    </Flex>
  );
}
