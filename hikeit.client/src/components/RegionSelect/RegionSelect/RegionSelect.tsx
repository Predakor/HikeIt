import type { Region } from "@/data/types";
import { createListCollection } from "@chakra-ui/react";
import SelectWrapper from "../SelectWrapper";
import { regionsList } from "@/data/regionsList";

interface Props<T> {
  onValueChange: (el: T) => void;
}

function RegionSelect({ onValueChange }: Props<Region>) {
  return (
    <SelectWrapper onValueChange={onValueChange} collection={regions}>
      {(region) => <>{region.name}</>}
    </SelectWrapper>
  );
}

const regions = createListCollection({
  items: regionsList,
  itemToString: (item) => item.name,
  itemToValue: (item) => item.id.toString(),
});

export default RegionSelect;
