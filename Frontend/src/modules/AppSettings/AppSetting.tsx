import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import type { InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";
import { PrimaryButton } from "@/components/ui/Buttons";
import { Button, Flex, Stack } from "@chakra-ui/react";
import { useForm } from "react-hook-form";
import { Link } from "react-router";
import { AppSettingType, type AppSettingsEntry, SchemaInputMap } from "./appSettingsSchema";
import { toSettingValue } from "./toSettingValue";
import * as AppSettings from "./useAppSettings";

// These settings are filter lists, edited in the pipeline bench instead of the generic form.
const pipelineSettingTypes: string[] = Object.values(AppSettingType);

export function AppSetting({ setting }: { setting: AppSettingsEntry }) {
  if (pipelineSettingTypes.includes(setting.type)) {
    return <PipelineSettingLink setting={setting} />;
  }

  return <AppSettingForm setting={setting} />;
}

function PipelineSettingLink({ setting }: { setting: AppSettingsEntry }) {
  return (
    <Flex align={"center"} justify={"space-between"} gap={3}>
      <h3>{setting.name}</h3>
      <Button asChild size={"sm"} variant={"outline"}>
        <Link to={"../pipeline"} relative={"path"}>
          Edit in pipeline config
        </Link>
      </Button>
    </Flex>
  );
}

function AppSettingForm({ setting }: { setting: AppSettingsEntry }) {
  const formHook = useForm({ defaultValues: setting.value });
  const updateSetting = AppSettings.useUpdate();
  const editableProperties = Object.entries(setting.schema.properties).filter(
    ([, { type }]) => type in SchemaInputMap,
  );
  // A number input defaults to step 1 and rejects fractions (0.88); "any" lets float settings take any decimal.
  const config = editableProperties.map(([key, rules]) => ({
    key,
    type: SchemaInputMap[rules.type],
    step: rules.type === "number" ? "any" : undefined,
  })) as InputsConfig;

  const sendRequest = formHook.handleSubmit((formValues) =>
    updateSetting.mutateAsync({
      id: setting.id,
      value: toSettingValue(setting, editableProperties, formValues),
    }),
  );

  return (
    <Stack as={"form"} onSubmit={sendRequest} key={setting.name}>
      <h3>{setting.name}</h3>
      <RenderInputs formHook={formHook} config={config} />
      <PrimaryButton type="submit">Update</PrimaryButton>
    </Stack>
  );
}
