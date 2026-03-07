import { NavItem } from "@/Layout/Nav/NavItem";
import IsAdminUser from "@/Utils/IsAdminUser";
import useUser from "@/hooks/Auth/useUser";
import {
  Avatar,
  Box,
  Flex,
  HStack,
  Skeleton,
  SkeletonCircle,
  Stack,
  useBreakpointValue,
} from "@chakra-ui/react";
import { NavLink } from "react-router";
import FetchWrapper from "../Utils/Fetching/FetchWrapper";
import { UserMenu } from "./UserMenu";

export default function User() {
  const getUser = useUser();
  const isMobile = useBreakpointValue({ base: true, md: false });

  return (
    <FetchWrapper LoadingComponent={UserSkeleton} request={getUser}>
      {(user) => (
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
              <UserMenu userName={user.userName} />
            )}
          </Flex>
        </Stack>
      )}
    </FetchWrapper>
  );
}

function UserSkeleton() {
  return (
    <HStack>
      <SkeletonCircle size={10} />
      <Skeleton height={"8"} width={"24"} />
    </HStack>
  );
}
