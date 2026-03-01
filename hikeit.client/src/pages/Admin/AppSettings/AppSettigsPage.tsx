import api from "@/Utils/Api/apiRequest";
import AdminPage from "@/components/Pages/AdminPage";
import FetchWrapper from "@/components/Utils/Fetching/FetchWrapper";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import type { InputConfigEntry, InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";
import { PrimaryButton } from "@/components/ui/Buttons";
import { For, Stack } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";
import { Form, useForm } from "react-hook-form";

type PropertyType = "number" | "string" | "integer";

interface AppSettingsEntry<TValue extends object = any> {
  id: number;
  name: string;
  value: TValue;
  schema: {
    type: PropertyType;
    properties: Record<keyof TValue, { type: PropertyType }>;
    required: Partial<keyof TValue[]>;
  };
  settingType: any;
}

const SchemaInputMap: Record<PropertyType, InputConfigEntry["type"]> = {
  string: "text",
  number: "number",
  integer: "number",
};

function AppSettigsPage() {
  const appSettingsValues = useQuery<AppSettingsEntry<any>[]>({
    queryKey: ["settings"],
    queryFn: () => api.get("appSettings"),
  });

  return (
    <AdminPage title={"App Settings"}>
      <FetchWrapper request={appSettingsValues}>
        {(child) => (
          <For each={child}>
            {(setting) => {
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

                return api.put(`AppSettings/${setting.id}`, { body: JSON.stringify(mappedData) });
              });

              return (
                <Stack as={"form"} onSubmit={sendRequest} key={setting.name}>
                  <h3>{setting.name}</h3>
                  <RenderInputs formHook={formHook} config={config} />
                  <PrimaryButton type="submit">Update</PrimaryButton>
                </Stack>
              );
            }}
          </For>
        )}
      </FetchWrapper>
    </AdminPage>
  );
}
export default AppSettigsPage;
