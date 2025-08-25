import type { TimeString } from "@/Utils/Formatters/Duration/Duration";

export const formatter = {
  toKm: (v: number) => (v / 1000).toFixed(1),
  toHours: (v: number) => (v / 60).toFixed(2),
  toRawDuration: (timeString: TimeString) => {
    const [hours, minutes, seconds] = extractTimeUnits(timeString);
    const hoursToSeconds = hours * 60 * 60;
    const minutesToSeconds = minutes * 60;
    return hoursToSeconds + minutesToSeconds + seconds;
  },
  toDuration: (stringDate: TimeString) => {
    const [hours, minutes] = extractTimeUnits(stringDate);

    const parts = [];
    if (hours > 0) parts.push(`${hours}h`);
    if (minutes > 0 || hours === 0) parts.push(`${minutes}min`);

    return parts.join(" ");
  },
  toPercentage: (v: number, precision = 1) => `${v.toFixed(precision)}%`,
};

export const timeConverter = {
  fromSeconds: (totalSeconds: number) => {
    const hours = Math.floor(totalSeconds / 3600);
    const minutes = Math.floor((totalSeconds % 3600) / 60);
    const seconds = totalSeconds % 60;

    const parts: string[] = [];
    if (hours > 0) parts.push(`${hours}h`);
    if (minutes > 0) parts.push(`${minutes}min`);
    if (seconds > 0 && hours === 0) parts.push(`${seconds}s`);
    // optional: hide seconds if there are hours

    return parts.join(" ");
  },

  toRawDuration: (timeString: TimeString) => {
    const [hours, minutes, seconds] = extractTimeUnits(timeString);
    const hoursToSeconds = hours * 60 * 60;
    const minutesToSeconds = minutes * 60;
    return hoursToSeconds + minutesToSeconds + seconds;
  },
};

const extractTimeUnits = (time: TimeString) => {
  const [rawHours, rawMinutes, rawSeconds] = time.split(":");

  return [parseInt(rawHours), parseInt(rawMinutes), parseInt(rawSeconds)];
};
