import type { AppSettingsEntry } from "./appSettingsSchema";

// Form inputs hand back strings, so numeric properties are converted before they are saved.
export function toSettingValue(
  setting: AppSettingsEntry,
  editableProperties: [string, { type: string }][],
  formValues: Record<string, unknown>,
) {
  const value = { ...setting.value };

  editableProperties.forEach(([key, { type }]) => {
    const isNumeric = type === "integer" || type === "number";
    value[key] = isNumeric ? Number(formValues[key]) : formValues[key];
  });

  return value;
}
