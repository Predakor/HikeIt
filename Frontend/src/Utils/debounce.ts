/**
 * Creates a debounced version of a function
 * Prevents execution until delay ms after last call
 */
export function debounce<Args extends any[], Return>(
  fn: (...args: Args) => Return,
  delay: number = 500,
): (...args: Args) => void {
  let timeoutId: NodeJS.Timeout | null = null;

  return (...args: Args) => {
    if (timeoutId) {
      clearTimeout(timeoutId);
    }

    timeoutId = setTimeout(() => {
      fn(...args);
    }, delay);
  };
}
