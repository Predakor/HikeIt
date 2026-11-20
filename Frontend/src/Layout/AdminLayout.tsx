import IsAdminUser from "@/Utils/IsAdminUser";
import useUser from "@/hooks/Auth/useUser";
import { Center, Flex, Spacer, Stack } from "@chakra-ui/react";
import FetchWrapper from "@components/Utils/Fetching/FetchWrapper";
import { GoBackButton } from "@components/ui/Buttons";
import PageTitle from "@components/ui/Titles/PageTitle";
import type { ReactNode } from "react";

interface Props {
  children: ReactNode | ReactNode[];
  title: string;
  header?: ReactNode;
}

export default function AdminLayout(props: Props) {
  const getUser = useUser();

  return (
    <FetchWrapper request={getUser}>
      {(user) => (IsAdminUser(user) ? <AdminFrame {...props} /> : <NoAcces />)}
    </FetchWrapper>
  );
}

function AdminFrame({ title, children, header }: Props) {
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
