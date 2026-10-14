import type { CollectionType } from "@/components/ui/Inputs/Select/useCollection";
import type { FullMap } from "@/types/Utils/mapping.types";
import type { AllowedInputTypes } from "@/Utils/Schemas";
import type { Control, FieldValues, UseFormRegister } from "react-hook-form";

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
  | IntervalInputConfigEntry
  | NumberInputConfigEntry
  | DateInputConfigEntry
  | CheckboxInputConfigEntry
  | TextInputConfigEntry
  | SelectInputConfigEntry
  | EmailInputConfigEntry;

export interface EmailInputConfigEntry extends InputConfigEntryBase {
  type: "email";
  validate: (value: string) => boolean | string;
}

export interface RangeInputConfigEntry extends InputConfigEntryBase {
  type: "range";
  min: number;
  max: number;
  step?: number;
}

/** Two-thumb slider writing to two form fields: `key` is the lower end, `maxKey` the upper. */
export interface IntervalInputConfigEntry extends InputConfigEntryBase {
  type: "interval";
  maxKey: string;
  min: number;
  max: number;
  step?: number;
}

export interface PasswordInputConfigEntry extends InputConfigEntryBase {
  type: "password";
  pattern?: RegExp;
  validate: (value: string) => boolean | string;
}

export interface TextInputConfigEntry extends InputConfigEntryBase {
  type: "text";
  pattern?: RegExp;
  validate?: (value: string) => boolean | string;

  placeholder?: string;
}

export interface NumberInputConfigEntry extends InputConfigEntryBase {
  type: "number";
  step?: string | number;
}

export interface DateInputConfigEntry extends InputConfigEntryBase {
  type: "date";
  defaultChecked?: boolean;
}

export interface SelectInputConfigEntry extends Omit<InputConfigEntryBase, "min" | "max"> {
  type: "select";
  collection: CollectionType<any>;
}

export interface CheckboxInputConfigEntry extends Omit<InputConfigEntryBase, "min" | "max"> {
  type: "checkbox";
  defaultChecked?: boolean;
}

export type InputsConfig = InputConfigEntry[];

/** Lays its items out together; items are inputs or further groups. */
export interface InputGroupEntry {
  type: "group";
  key: string;
  label?: string;
  direction?: "row" | "column";
  items: InputsLayout;
}

export type InputsLayout = (InputConfigEntry | InputGroupEntry)[];

export type InputsConfigFor<T extends object> = FullMap<T, InputConfigEntry>;

///other types
export interface DisplayOptions {
  label?: "inline" | "ontop";
  size?: "2xl" | "xl" | "lg" | "md" | "sm" | "xs";
}

export interface RenderInputBaseProps<T extends FieldValues> {
  control: Control<T, any, T>;
  register: UseFormRegister<T>;
  displayOptions?: DisplayOptions;
}
