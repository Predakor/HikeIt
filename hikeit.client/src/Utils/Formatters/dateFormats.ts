export const dateOnlyToString = (dateString: string): string => {
  const date = new Date(dateString);
  return date.toLocaleDateString("en-UK", {
    year: "numeric",
    month: "long",
    day: "2-digit",
  });
};
