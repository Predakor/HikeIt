import AdminLayout from "@/Layout/AdminLayout";
import { NavItem } from "@/Layout/Nav/NavItem";
import SubTitle from "@/components/ui/Titles/SubTitle";
import { adminRoutes } from "@/data/routes/admin/adminRoutes";
import { For, Stack } from "@chakra-ui/react";

export default function ManageAdminPage() {
  const routes = adminRoutes.pages.filter((p) => p.path !== "");
  return (
    <AdminLayout title="Welcome to admin center">
      <SubTitle title="See what action you can do bellow" />
      <Stack direction={"row"} gapX={8}>
        <For each={routes}>
          {(route) => <NavItem path={route.path} label={route.label} key={route.path} />}
        </For>
      </Stack>
    </AdminLayout>
  );
}
