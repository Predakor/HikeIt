import { Flex, For, IconButton, Text } from "@chakra-ui/react";
import { IconArrowDown, IconArrowUp, IconDelete } from "@/Icons/Icons";
import {
  fieldsOf,
  readFieldValue,
  type FilterFieldValue,
  type FilterItem,
  type FilterKind,
} from "./filterCatalog";
import { FieldInput } from "./FieldInput";

interface FilterRowProps {
  kind: FilterKind;
  filter: FilterItem;
  index: number;
  count: number;
  onMove: (delta: number) => void;
  onRemove: () => void;
  onValueChange: (name: string, value: FilterFieldValue) => void;
}

export function FilterRow({
  kind,
  filter,
  index,
  count,
  onMove,
  onRemove,
  onValueChange,
}: FilterRowProps) {
  const fields = fieldsOf(kind, filter.filterType);
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
        {index + 1}. {filter.filterType}
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
      <IconButton
        size={"xs"}
        variant={"ghost"}
        colorPalette={"red"}
        aria-label={"Remove filter"}
        onClick={onRemove}
      >
        <IconDelete />
      </IconButton>
    </Flex>
  );
}
