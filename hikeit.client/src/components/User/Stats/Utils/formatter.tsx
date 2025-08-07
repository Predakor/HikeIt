export const formatter = {
  toKm: (v: number) => (v / 1000).toFixed(1),
  toHours: (v: number) => (v / 60).toFixed(2),
  toDuration: (stringDate: string) => {
    const [rawHours, rawMinutes] = stringDate.split(":");
    const hours = parseInt(rawHours, 10);
    const minutes = parseInt(rawMinutes, 10);

    const parts = [];
    if (hours > 0) parts.push(`${hours}h`);
    if (minutes > 0 || hours === 0) parts.push(`${minutes}min`);

    return parts.join(" ");
  },
};
