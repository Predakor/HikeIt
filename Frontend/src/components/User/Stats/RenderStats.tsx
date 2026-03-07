import {
  GenericFormatter,
  KeyToLabelFormatter,
} from "@/Utils/Formatters/valueFormatter";
import RowStat from "@/components/Stats/RowStat";
import { For } from "@chakra-ui/react";

interface Props {
  stats: Array<[string, string | number]>;
}

export default function RenderStats({ stats }: Props) {
  return (
    <For each={stats}>
      {([key, value]) => (
        <RowStat
          value={value}
          label={KeyToLabelFormatter(key)}
          addons={{ formatt: GenericFormatter }}
        />
      )}
    </For>
  );
}
