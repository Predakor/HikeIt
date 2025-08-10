import { NavItem } from "@/Layout/Nav/NavItem";
import AdminPage from "@/components/Pages/AdminPage";
import PageTitle from "@/components/Titles/PageTitle";
import SubTitle from "@/components/Titles/SubTitle";
import { adminRoutes } from "@/data/routes/admin/adminRoutes";
import { For, Stack } from "@chakra-ui/react";

function ManageAdminPage() {
  return (
    <AdminPage>
      <PageTitle title="Welcome to admin center" />
      <SubTitle title="See what action you can do bellow" />
      <Stack>
        <For each={adminRoutes.pages}>
          {(route) => (
            <NavItem path={route.path} label={route.label} key={route.path} />
          )}
        </For>
      </Stack>
    </AdminPage>
  );
}
export default ManageAdminPage;
