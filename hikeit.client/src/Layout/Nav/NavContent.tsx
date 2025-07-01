import User from "@/components/User/User";
import { navRoutes } from "@/data/routes/navRoutes";
import { For, Spacer } from "@chakra-ui/react";
import { NavItem } from "./NavItem";

export default function NavContent() {
  const visibleRoutes = navRoutes.filter((route) => !route.hidden);

  return (
    <>
      <For each={visibleRoutes}>
        {(route) => <NavItem path={route.path} label={route.label} />}
      </For>

      <Spacer />
      <User />
    </>
  );
}
