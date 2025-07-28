export const loginValidator = (value: string) => {
  if (!value) return "Username is required";
  if (value.length < 3) return "Username must be at least 3 characters";
  if (!/^[a-zA-Z0-9\-._@+]+$/.test(value)) {
    return "Username can only contain letters, numbers, and -._@+";
  }
  return true;
};
