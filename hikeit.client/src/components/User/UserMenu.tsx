import { userRoutes } from "@/data/routes/userRoutes";
import { useAuth } from "@/hooks/Auth/useAuth";
import { Button, For, Heading, Icon, Menu } from "@chakra-ui/react";
import { FaAngleDown } from "react-icons/fa";
import { NavLink } from "react-router";

export interface Props {
  userName: string;
}

export function UserMenu({ userName }: Props) {
  const basePath = `/${userRoutes.path}/`;
  const { logout } = useAuth();
  return (
    <Menu.Root>
      <Menu.Trigger
        asChild
        _hover={{
          cursor: "pointer",
          scale: 1.1,
        }}
      >
        <Button unstyled display={"flex"} alignItems={"end"} variant={"ghost"}>
          <Heading fontWeight={"semibold"} fontSize={"2xl"}>
            {userName}
          </Heading>
          <Icon size={"lg"}>
            <FaAngleDown />
          </Icon>
        </Button>
      </Menu.Trigger>
      <Menu.Positioner>
        <Menu.Content minWidth={"44"} fontSize={"xl"}>
          <For each={userRoutes.pages}>
            {(route) => (
              <Menu.Item
                asChild
                fontSize={"inherit"}
                cursor={"pointer"}
                value={route.label}
              >
                <NavLink to={basePath + route.path}>{route.label}</NavLink>
              </Menu.Item>
            )}
          </For>

          <Menu.Item
            asChild
            fontSize={"inherit"}
            color={"fg.error"}
            cursor={"pointer"}
            value="logout"
            onClick={logout}
          >
            <NavLink to={"/auth/login"}>Log Out</NavLink>
          </Menu.Item>
        </Menu.Content>
      </Menu.Positioner>
    </Menu.Root>
  );
}
