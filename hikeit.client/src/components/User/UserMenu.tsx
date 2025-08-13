import { userRoutes } from "@/data/routes/userRoutes";
import { Button, For, Heading, Icon, Menu } from "@chakra-ui/react";
import { FaAngleDown } from "react-icons/fa";
import { NavLink } from "react-router";

export interface Props {
  userName: string;
  logout: () => void;
}

export function UserMenu({ userName, logout }: Props) {
  const basePath = `/${userRoutes.path}/`;

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
            onConfirm={logout}
          >
            <NavLink to={"/auth/login"}>Log Out</NavLink>
          </Menu.Item>
        </Menu.Content>
      </Menu.Positioner>
    </Menu.Root>
  );
}
