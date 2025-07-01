import { GenericFormatter } from "@/Utils/Formatters/valueFormatter";

export function routeFormatter(stat: any): string | number {
  return GenericFormatter(stat);
}

export function toMinutes(stat: any): string | number | null {
  const [hours, minutes, seconds] = stat.split(":").map(Number);
  const date = new Date();
  date.setHours(hours, minutes, seconds, 0);
  return hours * 60 + date.getMinutes();
}

export function toHours(stat: any): string | number | null {
  const [hours, minutes, seconds] = stat.split(":").map(Number);
  const date = new Date();
  date.setHours(hours, minutes, seconds, 0);
  return hours + date.getMinutes() / 60;
}
