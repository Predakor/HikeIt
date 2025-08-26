import { arrayUtils } from "@/Utils/arrayUtils";

export type TimeString = string & { __brand: "timeString" };

type CastableTimes = TimeString | number | TimeSpan;

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

  static FromTimeString(timeString: TimeString) {
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

  subtract(time: TimeString | number | TimeSpan) {
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
    return this.seconds / 3600;
  }

  toString() {
    const h = Math.floor(this.seconds / 3600);
    const m = Math.floor((this.seconds % 3600) / 60);
    return h > 0 ? `${h}h ${m}min` : `${m}min`;
  }
}

const extractTimeUnits = (time: TimeString) => {
  const [rawHours, rawMinutes, rawSeconds] = time.split(":");

  return [parseInt(rawHours), parseInt(rawMinutes), parseInt(rawSeconds)];
};

const toTotalSeconds = (time: TimeString) => {
  const [hours, minutes, seconds] = extractTimeUnits(time);
  const hoursToSeconds = hours * 60 * 60;
  const minutesToSeconds = minutes * 60;
  return hoursToSeconds + minutesToSeconds + seconds;
};

const castToSeconds = (time: CastableTimes) => {
  switch (typeof time) {
    case "string":
      return toTotalSeconds(time);
    case "object":
      return time.toSeconds();
    default:
      return time;
  }
};
