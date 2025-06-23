import User from "@/components/User/User";
import { routes } from "@/data/routes";
import { For, Spacer } from "@chakra-ui/react";
import { NavItem } from "./NavItem";

export default function NavContent() {
  const visibleRoutes = routes.filter((route) => !route.hidden);

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
