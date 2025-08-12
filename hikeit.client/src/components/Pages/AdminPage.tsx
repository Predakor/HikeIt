import IsAdminUser from "@/Utils/IsAdminUser";
import { useAdminUser } from "@/hooks/Auth/useUser";
import { Center, Flex, Spacer, Stack } from "@chakra-ui/react";
import type { ReactNode } from "react";
import GoBackButton from "../Buttons/GoBackButton";
import PageTitle from "../Titles/PageTitle";
import FetchWrapper from "../Wrappers/Fetching/FetchWrapper";

interface Props {
  children: ReactNode | ReactNode[];
  title: string;
  header?: ReactNode;
}

export default function AdminPage(props: Props) {
  const getUser = useAdminUser();

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
