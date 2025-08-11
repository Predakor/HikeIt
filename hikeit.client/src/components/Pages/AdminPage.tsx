import IsAdminUser from "@/Utils/IsAdminUser";
import { useAdminUser } from "@/hooks/Auth/useUser";
import type { ReactNode } from "react";
import FetchWrapper from "../Wrappers/Fetching/FetchWrapper";
import PageTitle from "../Titles/PageTitle";
import { Center, Flex, Stack } from "@chakra-ui/react";
import GoBackButton from "../Buttons/GoBackButton";

interface Props {
  children: ReactNode | ReactNode[];
  title: string;
}

export default function AdminPage({ children, title }: Props) {
  const getUser = useAdminUser();

  return (
    <FetchWrapper request={getUser}>
      {(user) =>
        IsAdminUser(user) ? (
          <AdminLayout children={children} title={title} />
        ) : (
          <NoAcces />
        )
      }
    </FetchWrapper>
  );
}

function AdminLayout({ title, children }: Props) {
  return (
    <Stack gapY={8}>
      <Flex alignItems={"center"} direction={"row"}>
        <GoBackButton />
        <PageTitle title={title} />
      </Flex>
      <Stack>{children}</Stack>
    </Stack>
  );
}

function NoAcces() {
  return (
    <Flex>
      <Center>
        <PageTitle title="You are not authorized to view this page" />
      </Center>
    </Flex>
  );
}
