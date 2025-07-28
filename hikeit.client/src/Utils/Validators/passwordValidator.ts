export const passwordValidator = (value: string) => {
  if (!/[A-Z]/.test(value)) return "Must contain an uppercase letter";
  if (!/[a-z]/.test(value)) return "Must contain a lowercase letter";
  if (!/\d/.test(value)) return "Must contain a number";
  if (!/[^\da-zA-Z]/.test(value)) return "Must contain a special character";
  return true;
};
