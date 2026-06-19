import { TimeSpan } from "@/Utils/Formatters/Duration/Duration";
import type { TimeUnit } from "@/types/Utils/stat.types";
import { RowStat } from "..";
import type { RowStatProps } from "../RowStat";
import type { TimeSpanString } from "@/types/Api/types";

export function TimeRowStat({
  value,
  label,
  styles,
  addons,
}: RowStatProps<TimeSpan | TimeSpanString>) {
  if (typeof value === "string") {
    value = TimeSpan.From(value);
  }

  const totalHours = value.toHours();
  const hasAtleast1Hour = totalHours > 0;
  const hasMoreThan3Days = totalHours > 72;

  let unit: TimeUnit = "s";
  let timeValue = value.toMinutes();

  if (hasAtleast1Hour) {
    unit = "h";
    timeValue = value.toHours();
  }

  if (hasMoreThan3Days) {
    unit = "d";
    timeValue = value.toHours() / 24;
  }

  return (
    <RowStat
      label={label}
      styles={styles}
      value={timeValue}
      addons={{ unit, IconSource: addons?.IconSource, badge: addons?.badge }}
    />
  );
}
