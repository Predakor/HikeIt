import { DangerButton } from "@/components/ui/Buttons";
import * as AppSettings from "@/modules/AppSettings";
import type {
  PaceFilterConfig,
  PeakFilterConfig,
  PointFilterConfig,
  TerrainFilterConfig,
} from "@/modules/PipelineBench/shared/types/FilterConfig";
import { Flex, Show, Text } from "@chakra-ui/react";

type AppliedFilter = PointFilterConfig | PeakFilterConfig | PaceFilterConfig | TerrainFilterConfig;

const confirmConfig = {
  title: "Apply to production?",
  confirmButtonText: "apply",
  warningDescription: "This replaces the production setting for every trip.",
};

interface ApplyToProductionButtonProps<TFilter extends AppliedFilter> {
  settingType: AppSettings.AppSettingTypeId;
  filters: TFilter[];
}

export function ApplyToProductionButton<TFilter extends AppliedFilter>({
  settingType,
  filters,
}: ApplyToProductionButtonProps<TFilter>) {
  const { data: settings } = AppSettings.useAll();

  const settingId = settings?.find((setting) => setting.type === settingType)?.id;
  const mutation = AppSettings.useUpdate();

  const apply = () => {
    if (settingId !== undefined) {
      mutation.mutate({ id: settingId, value: { filters } });
    }
  };

  const disabledReason = settingId === undefined ? "Loading the production setting…" : undefined;

  return (
    <Flex align={"center"} gap={2}>
      <DangerButton
        size={"sm"}
        fontWeight={"normal"}
        disabled={disabledReason !== undefined}
        title={disabledReason}
        loading={mutation.isPending}
        onConfirm={apply}
        alertConfig={confirmConfig}
      >
        Apply to production
      </DangerButton>

      <Show when={mutation.isSuccess}>
        <Text fontSize={"xs"} color={"fg.success"}>
          Applied
        </Text>
      </Show>

      <Show when={mutation.isError}>
        <Text fontSize={"xs"} color={"fg.error"}>
          Failed to apply
        </Text>
      </Show>
    </Flex>
  );
}
