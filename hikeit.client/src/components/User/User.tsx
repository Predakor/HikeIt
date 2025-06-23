import useUser from "@/hooks/useUser";
import { Avatar, Button, Icon, LinkBox, Menu, Stack } from "@chakra-ui/react";
import { FaAngleDown } from "react-icons/fa";
import { NavLink } from "react-router";

export interface UserType {
  userName: string;
  firstName: string;
  lastName: string;
  email: string;
  avatar: string;
}

export default function User() {
  const [user, { logout }] = useUser();

  if (!user) {
    return;
  }

  return (
    <Stack alignItems={"center"} direction={"row"}>
      <Avatar.Root>
        <Avatar.Fallback></Avatar.Fallback>
        <Avatar.Image src={user.avatar}></Avatar.Image>
      </Avatar.Root>
      <LinkBox fontWeight={"semibold"} fontSize={"2xl"} asChild>
        <NavLink to={"/profile"}>{user.userName}</NavLink>
      </LinkBox>

      <Menu.Root>
        <Menu.Trigger asChild _hover={{ cursor: "pointer", scale: 1.1 }}>
          <Button unstyled size={"sm"} variant={"ghost"}>
            <Icon size={"lg"}>
              <FaAngleDown />
            </Icon>
          </Button>
        </Menu.Trigger>
        <Menu.Positioner>
          <Menu.Content>
            <Menu.Item value="rename">
              <NavLink to={"/profile"}>Profile</NavLink>
            </Menu.Item>

            <Menu.Item value="settings">
              <NavLink to={"/settings"}>Settings</NavLink>
            </Menu.Item>

            <Menu.Item color={"fg.error"} value="logout" asChild>
              <Button cursor={"pointer"} unstyled onClick={logout}>
                Log Out
              </Button>
            </Menu.Item>
          </Menu.Content>
        </Menu.Positioner>
      </Menu.Root>
    </Stack>
  );
}
