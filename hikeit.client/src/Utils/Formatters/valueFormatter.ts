import { dateOnlyToString } from "./dateFormats";

export function GenericFormatter<T extends string | number>(value: T) {
  switch (typeof value) {
    case "string":
      return isValidDateString(value) ? dateOnlyToString(value) : value;

    case "number":
      return value.toFixed(0);
  }
}

function isValidDateString(value: string): boolean {
  const date = new Date(value);
  return !isNaN(date.getTime());
}
