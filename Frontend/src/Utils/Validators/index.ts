import { passwordValidator } from "./passwordValidator";
import { emailValidator } from "./emailValidator";
import { loginValidator } from "./loginValidator";

export const validators = {
  password: passwordValidator,
  email: emailValidator,
  login: loginValidator,
};
