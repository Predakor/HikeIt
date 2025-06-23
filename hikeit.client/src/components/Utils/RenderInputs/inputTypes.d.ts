interface InputCoonfigEntryBase {
  key: string;
  label: string;
  type: React.HTMLInputTypeAttribute;
}

export type InputConfigEntry =
  | RangeInputConfigEntry
  | NumberInputConfigEntry
  | DateInputConfigEntry
  | CheckboxInputConfigEntry
  | TextInputConfigEntry;

interface RangeInputConfigEntry extends InputCoonfigEntryBase {
  type: "range";
  min: number;
  max: number;
  formatValue?: (value: number) => number;
}

interface NumberInputConfigEntry extends InputCoonfigEntryBase {
  type: "number";
  max: number;
  min: number;
}

interface DateInputConfigEntry extends InputCoonfigEntryBase {
  type: "date";
  defaultChecked?: boolean;
}

interface CheckboxInputConfigEntry extends InputCoonfigEntryBase {
  type: "checkbox";
  defaultChecked?: boolean;
}

interface TextInputConfigEntry extends InputCoonfigEntryBase {
  type: "text";
  pattern?: RegExp;
  placeholder?: string;
}

export type InputsConfig = InputConfigEntry[];
