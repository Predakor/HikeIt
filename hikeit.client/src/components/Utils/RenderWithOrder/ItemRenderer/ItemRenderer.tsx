import { KeyToLabelFormatter } from "@/Utils/Formatters/valueFormatter";
import GetStatsMeta from "@/Utils/GetStatsMeta";
import RowStat from "@/components/Stats/RowStat";
import type { OrderEntryItem } from "@/types/Utils/OrderTypes";

interface Props<T> {
  entry: OrderEntryItem<T, any>;
  data: any;
}

export function ItemRenderer<T>({ entry, data }: Props<T>) {
  const key = entry.key as string;
  const { label } = entry;

  const addons = GetStatsMeta(key);

  return (
    <RowStat
      value={data}
      addons={addons}
      label={label || KeyToLabelFormatter(key)}
    />
  );
}
