import type { FullMap } from "@/types/Utils/MappingTypes";
import type { CollectionType } from "@components/RegionSelect/useCollection";
import type { AllowedInputTypes } from "@utils/Schemas/index";
interface InputConfigEntryBase {
  key: string;
  label: string;
  type: AllowedInputTypes;
  min: number;
  max: number;
  required?: boolean;
}

export type InputConfigEntry =
  | PasswordInputConfigEntry
  | RangeInputConfigEntry
  | NumberInputConfigEntry
  | DateInputConfigEntry
  | CheckboxInputConfigEntry
  | TextInputConfigEntry
  | SelectInputConfigEntry
  | EmailInputConfigEntry;

interface EmailInputConfigEntry extends InputConfigEntryBase {
  type: "email";
  pattern: RegExp;
}
interface RangeInputConfigEntry extends InputConfigEntryBase {
  type: "range";
  min: number;
  max: number;
  step?: number;
  formatLabel?: (value: number) => number;
  formatValue?: (value: number) => number;
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

interface NumberInputConfigEntry extends InputConfigEntryBase {
  type: "number";
}

interface DateInputConfigEntry extends InputConfigEntryBase {
  type: "date";
  defaultChecked?: boolean;
}

interface SelectInputConfigEntry
  extends Omit<InputConfigEntryBase, "min" | "max"> {
  type: "select";
  collection: CollectionType;
}

interface CheckboxInputConfigEntry extends InputConfigEntryBase {
  type: "checkbox";
  defaultChecked?: boolean;
}

export type InputsConfig = InputConfigEntry[];

export type InputsConfigFor<T> = FullMap<InputConfigEntry, T>[];
///other types
export interface DisplayOptions {
  label?: "inline" | "ontop";
  size?: "2xl" | "xl" | "lg" | "md" | "sm" | "xs";
}
interface RenderInputBaseProps<T extends FieldValues> {
  control: Control<T, any, T>;
  register: UseFormRegister<T>;
  displayOptions?: DisplayOptions;
}
