import type { CollectionType } from "@/components/ui/Inputs/Select/useCollection";
import type { FullMap } from "@/types/Utils/mapping.types";
import type { FieldValues, UseFormGetValues, UseFormSetValue } from "react-hook-form";
import type { AllowedInputTypes } from "@utils/Schemas/index";
interface InputConfigEntryBase {
  key: string;
  label: string;
  type: AllowedInputTypes;
  min: number;
  max: number;
  required?: boolean;
}

export type InputConfigEntry<TForm extends FieldValues = any> =
  | PasswordInputConfigEntry
  | RangeInputConfigEntry<TForm>
  | NumberInputConfigEntry
  | DateInputConfigEntry
  | CheckboxInputConfigEntry
  | TextInputConfigEntry
  | SelectInputConfigEntry
  | EmailInputConfigEntry;

interface EmailInputConfigEntry extends InputConfigEntryBase {
  type: "email";
  validate: (value: string) => bool | string;
}

interface RangeInputConfigEntry<TForm extends FieldValues = any> extends InputConfigEntryBase {
  type: "range";
  min: number;
  max: number;
  step?: number;
  /** Runs after this field changes; use `form.setValue` to move related fields. */
  effect?: (value: number, form: InputEffectForm<TForm>) => void;
}

interface PasswordInputConfigEntry extends InputConfigEntryBase {
  type: "password";
  pattern?: ?Regex;
  validate: (value: string) => bool | string;
}

interface TextInputConfigEntry extends InputConfigEntryBase {
  type: "text";
  pattern?: RegExp;
  validate?: (value: string) => bool | string;

  placeholder?: string;
}

interface NumberInputConfigEntry extends InputConfigEntryBase, Partial<HTMLInputElement> {
  type: "number";
}

interface DateInputConfigEntry extends InputConfigEntryBase {
  type: "date";
  defaultChecked?: boolean;
}

interface SelectInputConfigEntry extends Omit<InputConfigEntryBase, "min" | "max"> {
  type: "select";
  collection: CollectionType;
}

interface CheckboxInputConfigEntry extends InputConfigEntryBase {
  type: "checkbox";
  defaultChecked?: boolean;
}

export type InputsConfig = InputConfigEntry[];

type InputsConfigFor<T extends FieldValues> = FullMap<T, InputConfigEntry<T>>;
///other types
export interface DisplayOptions {
  label?: "inline" | "ontop";
  size?: "2xl" | "xl" | "lg" | "md" | "sm" | "xs";
}
/** The slice of the form an effect may touch, typed to the form's own fields. */
export interface InputEffectForm<TForm extends FieldValues = any> {
  getValues: UseFormGetValues<TForm>;
  setValue: UseFormSetValue<TForm>;
}
interface RenderInputBaseProps<T extends FieldValues> {
  control: Control<T, any, T>;
  form?: InputEffectForm<T>;
  register: UseFormRegister<T>;
  displayOptions?: DisplayOptions;
}
