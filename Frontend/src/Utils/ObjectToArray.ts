type MappedObject<TKey, TValue> = [keyof TKey, TValue][];

export function NotNullOrObject<T>(value: T): value is Exclude<T, object | null> {
  return value !== null && typeof value !== "object" && typeof value !== "function";
}
export function ToTitleCase(key: string): string {
  return key.charAt(0).toUpperCase() + key.slice(1);
}

export function ObjectToArray<T extends object>(object: T) {
  if (!object) {
    throw new Error("passed null object");
  }

  return Object.entries(object).filter(([, value]) => NotNullOrObject(value)) as MappedObject<
    T,
    string | number
  >;
}

export function AllObjectEntriesToArray<T extends object>(object: T) {
  return Object.entries(object) as MappedObject<T, unknown>;
}

export function NestedObjectsToArray<T extends object>(object: T) {
  return Object.entries(object).filter(([, value]) => typeof value === "object") as MappedObject<
    T,
    object
  >;
}
