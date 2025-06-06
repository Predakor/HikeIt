export function NotNullOrObject(value: any): unknown {
  return value !== null && typeof value != "object";
}
export function ToTitleCase(key: string): any {
  return key.charAt(0).toUpperCase() + key.slice(1);
}

export function ObjectToArray<T extends object>(object: T) {
  return Object.entries(object).filter(([, value]) =>
    NotNullOrObject(value)
  ) as [keyof T, string | number][];
}
