import IsAdminUser from "@/Utils/IsAdminUser";
import useUser from "@/hooks/Auth/useUser";
import { Center, Flex, Spacer, Stack } from "@chakra-ui/react";
import type { ReactNode } from "react";
import PageTitle from "../ui/Titles/PageTitle";
import FetchWrapper from "../Utils/Fetching/FetchWrapper";
import { GoBackButton } from "../ui/Buttons";

interface Props {
  children: ReactNode | ReactNode[];
  title: string;
  header?: ReactNode;
}

export default function AdminPage(props: Props) {
  const getUser = useUser();

  return (
    <FetchWrapper request={getUser}>
      {(user) => (IsAdminUser(user) ? <AdminLayout {...props} /> : <NoAcces />)}
    </FetchWrapper>
  );
}

function AdminLayout({ title, children, header }: Props) {
  return (
    <Stack gapY={8}>
      <Stack alignItems={"center"} direction={{ base: "column", lg: "row" }}>
        <Flex alignItems={"center"}>
          <GoBackButton />
          <PageTitle title={title} />
        </Flex>
        {header && (
          <>
            <Spacer />
            {header}
          </>
        )}
      </Stack>
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
