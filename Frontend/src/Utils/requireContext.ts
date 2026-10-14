/** Returns the context value, or throws a clear error when a hook is used outside its provider. */
export function requireContext<T>(value: T | undefined, hookName: string, providerName: string): T {
  if (value === undefined) {
    throw new Error(`${hookName} must be used within ${providerName}`);
  }
  return value;
}
