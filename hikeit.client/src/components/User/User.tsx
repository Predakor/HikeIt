import { NavItem } from "@/Layout/Nav/NavItem";
import IsAdminUser from "@/Utils/IsAdminUser";
import useUser from "@/hooks/Auth/useUser";
import { Avatar, Box, Flex, Stack, useBreakpointValue } from "@chakra-ui/react";
import { NavLink } from "react-router";
import { UserMenu } from "./UserMenu";

export default function User() {
  const [user, { logout }] = useUser();
  const isMobile = useBreakpointValue({ base: true, md: false });

  if (!user) {
    return;
  }

  return (
    <Stack
      direction={{ base: "column", lg: "row" }}
      alignItems={"center"}
      gap={8}
    >
      {IsAdminUser(user) && (
        <Box pr={4}>
          <NavItem path={"/admin"} label={"Admin Center"} />
        </Box>
      )}

      <Flex align={"center"} gapX={4}>
        <Avatar.Root>
          <Avatar.Fallback></Avatar.Fallback>
          <Avatar.Image src={user.avatar}></Avatar.Image>
        </Avatar.Root>

        {isMobile ? (
          <NavLink to={"user/profile"}>{user.userName}</NavLink>
        ) : (
          <UserMenu userName={user.userName} logout={logout} />
        )}
      </Flex>
    </Stack>
  );
}
