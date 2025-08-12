export function diffObjects<T extends object>(
  original: T,
  updated: Partial<T>
): Partial<T> {
  const diff: Partial<T> = {};

  for (const key in updated) {
    if (updated[key] !== original[key]) {
      diff[key] = updated[key];
    }
  }

  return diff;
}

export function applyChanges<T extends object>(
  original: T,
  updated: Partial<T>
) {
  return { ...original, ...updated };
}
