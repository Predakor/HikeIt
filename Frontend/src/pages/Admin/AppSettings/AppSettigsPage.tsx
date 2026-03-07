import api from "@/Utils/Api/apiRequest";
import AdminPage from "@/components/Pages/AdminPage";
import FetchWrapper from "@/components/Utils/Fetching/FetchWrapper";
import type { InputConfigEntry } from "@/components/Utils/RenderInputs/inputTypes";
import { For } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";
import { AppSetting } from "../../../components/AppSettings/AppSetting";

type PropertyType = "number" | "string" | "integer";

export interface AppSettingsEntry<TValue extends object = any> {
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

export const SchemaInputMap: Record<PropertyType, InputConfigEntry["type"]> = {
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
        {(presentAppsettings) => (
          <For each={presentAppsettings}>{(setting) => <AppSetting setting={setting} />}</For>
        )}
      </FetchWrapper>
    </AdminPage>
  );
}
export default AppSettigsPage;
