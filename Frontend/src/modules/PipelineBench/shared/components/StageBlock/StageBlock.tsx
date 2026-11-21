import { PrimaryButton } from "@/components/ui/Buttons";
import type { StageResult } from "@/modules/PipelineBench/api/pipelinePreview.types";
import { Badge, Button, Flex, Heading, Stack, Text } from "@chakra-ui/react";
import { type ReactNode, useContext } from "react";
import { FillHeightContext } from "../../context/fillHeightContext";
import type { BlockStatus } from "../../model/bench.types";
import { statusBadge } from "../../model/statusBadge";

interface StageBlockProps {
  title: string;
  status: BlockStatus;
  result: StageResult | undefined;
  onRun: () => void;
  isPrimaryRun?: boolean;
  children?: ReactNode;
}

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
  const isFillingHeight = useContext(FillHeightContext);

  const runButton =
    isPrimaryRun ?
      <PrimaryButton onClick={onRun} disabled={isRunning}>
        Run
      </PrimaryButton>
    : <Button size={"sm"} variant={"outline"} onClick={onRun} disabled={isRunning}>
        Run
      </Button>;

  return (
    <Stack
      borderWidth={"1px"}
      borderRadius={"md"}
      p={4}
      gap={3}
      flex={isFillingHeight ? 1 : undefined}
      minH={isFillingHeight ? 0 : undefined}
    >
      <Flex align={"center"} justify={"space-between"} gap={3} wrap={"wrap"}>
        <Flex align={"center"} gap={2}>
          <Heading size={"sm"}>{title}</Heading>
          <Badge colorPalette={badge.colorPalette} variant={"subtle"}>
            {badge.label}
          </Badge>

          {result?.cacheHit && (
            <Badge colorPalette={"teal"} variant={"subtle"}>
              Cache hit
            </Badge>
          )}
        </Flex>
        {runButton}
      </Flex>

      <Stack gap={3} flex={isFillingHeight ? 1 : undefined} minH={0}>
        {children}
      </Stack>

      {result?.error && (
        <Text fontSize={"xs"} color={"fg.error"}>
          {result.error}
        </Text>
      )}
    </Stack>
  );
}
