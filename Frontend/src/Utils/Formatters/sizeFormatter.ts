const bytesPerKilobyte = 1024;

export function toKilobytes(bytes: number): number {
  return bytes / bytesPerKilobyte;
}
