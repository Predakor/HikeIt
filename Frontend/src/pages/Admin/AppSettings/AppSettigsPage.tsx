import AdminLayout from "@/Layout/AdminLayout";
import FetchWrapper from "@/components/Utils/Fetching/FetchWrapper";
import { For } from "@chakra-ui/react";
import * as AppSettings from "@modules/AppSettings";

function AppSettigsPage() {
  const appSettingsValues = AppSettings.useAll();

  return (
    <AdminLayout title={"App Settings"}>
      <FetchWrapper request={appSettingsValues}>
        {(presentAppsettings) => (
          <For each={presentAppsettings}>{(setting) => <AppSettings.Item setting={setting} />}</For>
        )}
      </FetchWrapper>
    </AdminLayout>
  );
}
export default AppSettigsPage;
