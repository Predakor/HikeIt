import api from "@/Utils/Api/apiRequest";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import type { InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";
import { PrimaryButton } from "@/components/ui/Buttons";
import { Stack } from "@chakra-ui/react";
import { useForm } from "react-hook-form";
import { type AppSettingsEntry, SchemaInputMap } from "@pages/Admin/AppSettings/AppSettigsPage";

export function AppSetting({ setting }: { setting: AppSettingsEntry<any> }) {
  const formHook = useForm({ defaultValues: setting.value });
  const propertiesMap = Object.entries(setting.schema.properties);
  const config = propertiesMap.map(([key, rules]) => ({
    key,
    type: SchemaInputMap[rules.type],
  })) as InputsConfig;

  const sendRequest = formHook.handleSubmit((d) => {
    const mappedData = { ...d };
    propertiesMap
      .filter(([, { type }]) => type === "integer" || type === "number")
      .map(([key]) => key)
      .forEach((key) => (mappedData[key] = Number(mappedData[key])));

    //test worklow
    //move to react query hook and update the cache
    return api.put(`AppSettings/${setting.id}`, { body: JSON.stringify(mappedData) });
  });

  return (
    <Stack as={"form"} onSubmit={sendRequest} key={setting.name}>
      <h3>{setting.name}</h3>
      <RenderInputs formHook={formHook} config={config} />
      <PrimaryButton type="submit">Update</PrimaryButton>
    </Stack>
  );
}
