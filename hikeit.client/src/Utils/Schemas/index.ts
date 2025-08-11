import { emailSchema } from "./emailSchema";
import { loginSchema } from "./loginSchema";
import { passwordSchema } from "./passwordSchema";
import { regionsSchema } from "./regionsSchema";

const schemas = {
  login: loginSchema,
  password: passwordSchema,
  email: emailSchema,
  region: regionsSchema,
};

export default schemas;

export type AllowedInputTypes =
  | "text"
  | "email"
  | "password"
  | "number"
  | "date"
  | "range"
  | "checkbox"
  | "select";

export interface FieldSchema {
  key: string;
  label: string;
  type: AllowedInputTypes;
  min: number;
  max: number;
  required?: boolean;
  pattern?: string | RegExp;
  placeholder?: string;
  validate?: ValidatorFn;
}

export type ValidatorFn = (value: string, field?: FieldSchema) => true | string;
