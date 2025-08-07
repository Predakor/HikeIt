import { HStack, Progress } from "@chakra-ui/react";

interface Props {
  total: number;
  finished: number;
}

export default function RegionCompletion({ total, finished }: Props) {
  const finishedPercentage = Math.floor((finished / total) * 100);
  const isComplete = finished === total;

  const color = isComplete ? "fg" : "fg.muted";
  const progressColor = finishedPercentage > 65 ? "fg" : "fg.muted";

  return (
    <Progress.Root defaultValue={40} value={finished} max={total} size={"xl"}>
      <HStack gap="5">
        <Progress.Label color={color}>Completed</Progress.Label>
        <Progress.Track flexGrow={1}>
          <Progress.Range bgColor={progressColor} />
        </Progress.Track>
        <Progress.ValueText>{finishedPercentage}%</Progress.ValueText>
      </HStack>
    </Progress.Root>
  );
}
