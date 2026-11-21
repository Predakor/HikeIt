import type { StageResult } from "@/modules/PipelineBench/api/pipelinePreview.types";
import { Text } from "@chakra-ui/react";
import type { BlockStatus } from "../../shared/model/bench.types";
import { StageBlock } from "../../shared/components/StageBlock/StageBlock";

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
