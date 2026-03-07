import { Slider } from "@chakra-ui/react";
import { TimeSpan } from "@/Utils/Formatters/Duration/Duration";

interface Props {
  start: TimeSpan;
  end: TimeSpan;
}

export function TimeInDaySlider({ start, end }: Props) {
  const marks = new Array(24 / hourSkip + 1).fill(null).map((_, i) => {
    const time = 60 * i * hourSkip;

    return {
      value: time,
      label: time / 60,
    };
  });

  const endsInNewDay = end.toMinutes() < start.toMinutes();
  const [sliderColor, backgroundColor] = endsInNewDay
    ? colors.reversed
    : colors.normal;

  return (
    <Slider.Root
      min={0}
      max={dayInMinutes}
      width="full"
      value={[start.toMinutes(), end.toMinutes()]}
      role="none"
    >
      <Slider.Control>
        <Slider.Track bg={backgroundColor}>
          <Slider.Range bg={sliderColor} />
        </Slider.Track>
        <Slider.Marks marks={marks} />
        <Slider.Thumbs boxSize={2} />
      </Slider.Control>
    </Slider.Root>
  );
}

const dayInMinutes = 60 * 24;
const hourSkip = 3;
const colors = {
  normal: ["blue.600", "bg.emphasized"],
  reversed: ["bg.emphasized", "blue.600"],
};
