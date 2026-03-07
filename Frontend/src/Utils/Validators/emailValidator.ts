export const emailValidator = (value: string) => {
  const emailRegex = /^[^@\s]+@[^@\s]+\.[^@\s]+$/;
  if (!emailRegex.test(value)) return "Invalid email address";
  return true;
};
