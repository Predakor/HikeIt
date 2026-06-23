import { arrayUtils } from "@/Utils/arrayUtils";
import type { TimeSpanString } from "@/types/Api/types";

type CastableTimes = TimeSpanString | number | TimeSpan;

export class TimeSpan {
  private seconds: number;

  private constructor(seconds: number) {
    this.seconds = seconds;
  }

  static From(time: CastableTimes) {
    return new TimeSpan(castToSeconds(time));
  }

  static FromSeconds(seconds: number) {
    return new TimeSpan(seconds);
  }

  static FromTimeString(timeString: TimeSpanString) {
    return new TimeSpan(toTotalSeconds(timeString));
  }

  static FromCombined(times: CastableTimes[]) {
    const totalSeconds = arrayUtils.sum(times, castToSeconds);
    return new TimeSpan(totalSeconds);
  }

  static FromDifference(startTime: CastableTimes, endTime: CastableTimes) {
    const start = castToSeconds(startTime);
    const end = castToSeconds(endTime);
    return new TimeSpan(Math.abs(start - end));
  }

  subtract(time: TimeSpanString | number | TimeSpan) {
    this.seconds -= castToSeconds(time);
    return this;
  }

  toSeconds() {
    return this.seconds;
  }

  toMinutes() {
    return Math.floor(this.seconds / 60);
  }

  toHours() {
    return Math.floor(this.seconds / 3600);
  }

  toString() {
    const d = Math.floor(this.seconds / 86400);
    const h = Math.floor((this.seconds % 86400) / 3600);
    const m = Math.floor((this.seconds % 3600) / 60);

    const parts = [];
    if (d > 0) parts.push(`${d}d`);
    if (h > 0) parts.push(`${h}h`);
    if (m > 0 || (d === 0 && h === 0)) parts.push(`${m}min`);

    return parts.join(" ");
  }
}

type TimeUnits = {
  days: number;
  hours: number;
  minutes: number;
  seconds: number;
};

const extractTimeUnits = (time: TimeSpanString): TimeUnits => {
  const [, rawDays, rawHours, rawMinutes, rawSeconds] =
    time.match(/^(?:(\d+)\.)?(\d+):(\d+):(\d+)/) ?? [];

  return {
    days: parseInt(rawDays ?? "0", 10),
    hours: parseInt(rawHours ?? "0", 10),
    minutes: parseInt(rawMinutes ?? "0", 10),
    seconds: parseInt(rawSeconds ?? "0", 10),
  };
};

const toTotalSeconds = (time: TimeSpanString) => {
  const { days, hours, minutes, seconds } = extractTimeUnits(time);

  const daysToSeconds = days * 24 * 60 * 60;
  const hoursToSeconds = hours * 60 * 60;
  const minutesToSeconds = minutes * 60;

  return daysToSeconds + hoursToSeconds + minutesToSeconds + seconds;
};

const castToSeconds = (time: CastableTimes) => {
  switch (typeof time) {
    case "string":
      return toTotalSeconds(time);
    case "object":
      return time.toSeconds();
    case "number":
      return time;
    default:
      return time satisfies never;
  }
};
