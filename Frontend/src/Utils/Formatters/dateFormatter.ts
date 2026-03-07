export const toUkDate = (dateString: string): string => {
  const date = new Date(dateString);
  return date.toLocaleDateString("en-UK", {
    year: "numeric",
    month: "long",
    day: "numeric",
  });
};
