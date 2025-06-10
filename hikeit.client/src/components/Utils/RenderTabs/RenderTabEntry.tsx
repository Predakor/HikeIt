import type { EntryType } from "@/types/Utils/OrderTypes";
import type { BaseProps } from "./typesRender";
import { GroupRender } from "./GroupRender";
import { ItemRender } from "./ItemRender";

interface Props<T> extends BaseProps<T> {
  entry: EntryType<T>;
}

export default function RenderTabEntry<T extends object>(props: Props<T>) {
  const { entry } = props;
  const type = entry.type;

  //Group render
  if (type == "group") {
    return <GroupRender {...props} entry={entry} />;
  }

  //item has undefined type bcs programmer was to lazy
  if (!type) {
    return <ItemRender {...props} entry={entry} />;
  }

  throw new Error("Passing object with no matching type:" + type);
}
