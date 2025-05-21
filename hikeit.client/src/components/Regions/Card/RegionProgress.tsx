import { HStack, Progress } from "@chakra-ui/react";

interface Props {
  total: number;
  finished: number;
}

function RegionProgress({ total, finished }: Props) {
  const finishedPercentage = Math.floor((finished / total) * 100);
  return (
    <Progress.Root defaultValue={40} value={finished} max={total} size={"lg"}>
      <HStack gap="5">
        <Progress.Label>Completed</Progress.Label>
        <Progress.Track flexGrow={1}>
          <Progress.Range />
        </Progress.Track>
        <Progress.ValueText>{finishedPercentage}%</Progress.ValueText>
      </HStack>
    </Progress.Root>
  );
}
export default RegionProgress;
