import type { StageResult } from "@/types/Api/pipelinePreview.types";
import { PrimaryButton } from "@/components/ui/Buttons";
import { Badge, Button, Flex, Heading, Stack, Text } from "@chakra-ui/react";
import type { ReactNode } from "react";
import type { BlockStatus } from "../bench.types";

interface StageBlockProps {
  title: string;
  status: BlockStatus;
  result: StageResult | undefined;
  onRun: () => void;
  isPrimaryRun?: boolean;
  children?: ReactNode;
}

const statusBadge: Record<BlockStatus, { label: string; colorPalette: string }> = {
  idle: { label: "Idle", colorPalette: "gray" },
  stale: { label: "Stale", colorPalette: "orange" },
  running: { label: "Running", colorPalette: "blue" },
  done: { label: "Done", colorPalette: "green" },
  error: { label: "Error", colorPalette: "red" },
};

export function StageBlock({
  title,
  status,
  result,
  onRun,
  isPrimaryRun = false,
  children,
}: StageBlockProps) {
  const badge = statusBadge[status];
  const isRunning = status === "running";

  const runButton =
    isPrimaryRun ?
      <PrimaryButton onClick={onRun} disabled={isRunning}>
        Run
      </PrimaryButton>
    : <Button size={"sm"} variant={"outline"} onClick={onRun} disabled={isRunning}>
        Run
      </Button>;

  return (
    <Stack borderWidth={"1px"} borderRadius={"md"} p={4} gap={3}>
      <Flex align={"center"} justify={"space-between"} gap={3} wrap={"wrap"}>
        <Flex align={"center"} gap={2}>
          <Heading size={"sm"}>{title}</Heading>
          <Badge colorPalette={badge.colorPalette} variant={"subtle"}>
            {badge.label}
          </Badge>
        </Flex>
        {runButton}
      </Flex>

      {children}

      {result ? <ResultInfo result={result} /> : null}
    </Stack>
  );
}

function ResultInfo({ result }: { result: StageResult }) {
  return (
    <Stack gap={1}>
      <Flex align={"center"} gap={3} wrap={"wrap"} fontSize={"xs"} color={"fg.muted"}>
        <Text as={"span"}>{result.durationMs} ms</Text>
        {result.cacheHit ?
          <Badge size={"sm"} colorPalette={"teal"} variant={"subtle"}>
            Cache hit
          </Badge>
        : null}
        <Text as={"span"}>{result.pointCount.toLocaleString()} points</Text>
        <Text as={"span"}>{formatBytes(result.payloadBytes)}</Text>
      </Flex>

      {result.error ?
        <Text fontSize={"xs"} color={"fg.error"}>
          {result.error}
        </Text>
      : null}
    </Stack>
  );
}

function formatBytes(bytes: number): string {
  const kilobyte = 1024;

  if (bytes < kilobyte) {
    return `${bytes} B`;
  }

  return `${(bytes / kilobyte).toFixed(1)} KB`;
}
