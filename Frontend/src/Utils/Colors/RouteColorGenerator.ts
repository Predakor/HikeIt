export default function getRandomColor(index: number): string {
  const hue = (index * 137.5) % 360; // Golden angle for color distribution
  return `hsl(${hue}, 90%, 50%)`;
}
