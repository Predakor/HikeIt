import AdminPage from "@/components/Pages/AdminPage";
import FetchWrapper from "@/components/Utils/Fetching/FetchWrapper";
import { For } from "@chakra-ui/react";
import * as AppSettings from "@components/AppSettings";

function AppSettigsPage() {
  const appSettingsValues = AppSettings.useAll();

  return (
    <AdminPage title={"App Settings"}>
      <FetchWrapper request={appSettingsValues}>
        {(presentAppsettings) => (
          <For each={presentAppsettings}>{(setting) => <AppSettings.Item setting={setting} />}</For>
        )}
      </FetchWrapper>
    </AdminPage>
  );
}
export default AppSettigsPage;
