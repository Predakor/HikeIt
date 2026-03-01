import api from "@/Utils/Api/apiRequest";
import { ObjectToArray } from "@/Utils/ObjectToArray";
import AdminPage from "@/components/Pages/AdminPage";
import { RowStat } from "@/components/Stats";
import FetchWrapper from "@/components/Utils/Fetching/FetchWrapper";
import { For, Stack } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";

interface AppSettingsEntry<TValue> {
  id: number;
  name: string;
  value: TValue;
  settingType: any;
}

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
              console.log(setting);

              const fields = ObjectToArray(setting.value);

              return (
                <div>
                  <h3>{setting.name}</h3>
                  <Stack>
                    <For each={fields}>
                      {([key, value]) => <RowStat value={value} label={key as string}></RowStat>}
                    </For>
                  </Stack>
                </div>
              );
            }}
          </For>
        )}
      </FetchWrapper>
    </AdminPage>
  );
}
export default AppSettigsPage;
