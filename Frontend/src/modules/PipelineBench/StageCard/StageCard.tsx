import { IconCompress, IconExpand } from "@/Icons/Icons";
import type { StageId, StageResults } from "@/modules/PipelineBench/api/pipelinePreview.types";
import { Box, Flex, Grid, IconButton, Show, Stack } from "@chakra-ui/react";
import { useState, type ReactNode } from "react";
import { FillHeightContext } from "../shared/context/fillHeightContext";
import { StatsTable } from "../StatsTable/StatsTable";

const fillMinHeight = "max(26rem, 60dvh)";

const stageLists: Record<StageId, StageId[]> = {
  cleanup: ["cleanup"],
  analytics: ["analytics"],
  elevationProfile: ["elevationProfile"],
  peakMatching: ["peakMatching"],
  route: ["route"],
};

interface StageCardProps {
  stage: StageId;
  baseline: StageResults;
  preview: StageResults;
  belowStats?: ReactNode | ((isExpanded: boolean) => ReactNode);
  isExpandable?: boolean;
  fillHeight?: boolean;
  children: ReactNode;
}

export function StageCard({
  stage,
  baseline,
  preview,
  belowStats,
  isExpandable = false,
  fillHeight = false,
  children,
}: StageCardProps) {
  const [isExpanded, setIsExpanded] = useState(false);
  const isShownFullWidth = isExpandable && isExpanded;
  const previewContent =
    typeof belowStats === "function" ? belowStats(isShownFullWidth) : belowStats;
  const previewPanel = (
    <Show when={previewContent}>
      <Box position={"relative"} minW={0}>
        {previewContent}
        <Show when={isExpandable}>
          <IconButton
            size={"xs"}
            variant={"subtle"}
            position={"absolute"}
            top={2}
            insetEnd={2}
            zIndex={1}
            aria-label={isExpanded ? "Collapse preview" : "Expand preview to full width"}
            title={isExpanded ? "Collapse preview" : "Expand preview to full width"}
            onClick={() => setIsExpanded((current) => !current)}
          >
            {isExpanded ?
              <IconCompress />
            : <IconExpand />}
          </IconButton>
        </Show>
      </Box>
    </Show>
  );

  // Filling: the config is taken out of the flow, so the row is as tall as the stats column or the minimum
  // height (whichever is more) and the filter list stretches (and scrolls) inside it.
  const config =
    fillHeight ?
      <Box position={"relative"} minW={0} minH={fillMinHeight}>
        <Flex position={"absolute"} inset={0} direction={"column"}>
          <FillHeightContext.Provider value={true}>{children}</FillHeightContext.Provider>
        </Flex>
      </Box>
    : <Box minW={0}>{children}</Box>;

  return (
    <Stack gap={6}>
      <Grid
        templateColumns={{ base: "1fr", lg: "repeat(2, minmax(0, 1fr))" }}
        gap={6}
        alignItems={fillHeight ? "stretch" : "start"}
      >
        {config}
        <Stack gap={4} minW={0}>
          <StatsTable stages={stageLists[stage]} baseline={baseline} preview={preview} />
          <Show when={!isShownFullWidth}>{previewPanel}</Show>
        </Stack>
      </Grid>

      <Show when={isShownFullWidth}>{previewPanel}</Show>
    </Stack>
  );
}
