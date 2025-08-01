import { userRoutes } from "@/data/routes/userRoutes";
import useUser from "@/hooks/useUser";
import { Avatar, Button, Heading, Icon, Menu, Stack } from "@chakra-ui/react";
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

  const basePath = `/${userRoutes.path}/`;

  return (
    <Stack alignItems={"center"} direction={"row"}>
      <Avatar.Root>
        <Avatar.Fallback></Avatar.Fallback>
        <Avatar.Image src={user.avatar}></Avatar.Image>
      </Avatar.Root>

      <Menu.Root>
        <Menu.Trigger asChild _hover={{ cursor: "pointer", scale: 1.1 }}>
          <Button
            unstyled
            display={"flex"}
            alignItems={"end"}
            size={"sm"}
            variant={"ghost"}
          >
            <Heading fontWeight={"semibold"} fontSize={"2xl"}>
              {user.userName}
            </Heading>
            <Icon size={"lg"}>
              <FaAngleDown />
            </Icon>
          </Button>
        </Menu.Trigger>
        <Menu.Positioner>
          <Menu.Content>
            {userRoutes.pages.map(({ path, label }) => (
              <Menu.Item value={label}>
                <NavLink to={basePath + path}>{label}</NavLink>
              </Menu.Item>
            ))}

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
