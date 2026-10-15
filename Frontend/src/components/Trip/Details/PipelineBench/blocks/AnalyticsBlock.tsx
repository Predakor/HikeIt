import type { StageResult } from "@/types/Api/pipelinePreview.types";
import { Text } from "@chakra-ui/react";
import type { BlockStatus } from "../bench.types";
import { StageBlock } from "./StageBlock";

interface AnalyticsBlockProps {
  status: BlockStatus;
  result: StageResult<"analytics"> | undefined;
  onRun: () => void;
}

// Analytics has no config: it is derived from the cleanup output.
export function AnalyticsBlock({ status, result, onRun }: AnalyticsBlockProps) {
  return (
    <StageBlock title={"Analytics"} status={status} result={result} onRun={onRun}>
      <Text fontSize={"sm"} color={"fg.muted"}>
        Derived from cleanup: it has no settings of its own and re-runs whenever cleanup changes.
      </Text>
    </StageBlock>
  );
}
