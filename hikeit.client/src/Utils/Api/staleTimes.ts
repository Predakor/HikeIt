//all values are in ms eg 1000 = 1 sec
const second = 1000;
const minute = second * 60;
const hour = minute * 60;
const day = hour * 24;

export const cacheTimes = {
  hour,
  day,
  fromMinutes: (minutes: number) => minutes * day,
  fromHoures: (hours: number) => hours * hour,
  fromDays: (days: number) => days * day,
};
