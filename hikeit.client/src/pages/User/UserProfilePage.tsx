import { ObjectToArray } from "@/Utils/ObjectToArray";
import FetchWrapper from "@/components/Wrappers/Fetching/FetchWrapper";
import DangerButton from "@/components/ui/Buttons/DangerButton";
import { useAuth } from "@/hooks/Auth/useAuth";
import useUserData from "@/hooks/User/useUserData";
import {
  Avatar,
  ButtonGroup,
  Card,
  Field,
  Input,
  Separator,
  Spacer,
  Stack,
} from "@chakra-ui/react";
import type { ReactElement } from "react";

function UserProfilePage() {
  const getUser = useUserData();
  const { logout } = useAuth();
  return (
    <FetchWrapper request={getUser}>
      {(user) => (
        <Stack>
          <Card.Root>
            <Card.Body asChild>
              <Stack
                alignItems={"center"}
                justifyItems={"center"}
                direction={{ base: "column", md: "row" }}
                gap={4}
              >
                <AccountBase base={user.base} />
                <Spacer />
                <ButtonGroup>
                  <DangerButton
                    onConfirm={logout}
                    alertConfig={{
                      confirmButtonText: "logout",
                      warningDescription:
                        "You are about to log out are you sure?",
                    }}
                  >
                    logout
                  </DangerButton>
                </ButtonGroup>
              </Stack>
            </Card.Body>
          </Card.Root>

          <UserDataCard title="Personal Information">
            <MapStats stats={user.personal} />
          </UserDataCard>

          <UserDataCard title="Account Details" Footer={<AccountActions />}>
            <MapStats stats={user.accountState} />
          </UserDataCard>
        </Stack>
      )}
    </FetchWrapper>
  );
}

interface CardProps {
  title: string;
  children: ReactElement | ReactElement[];
  Footer?: ReactElement | ReactElement[];
}

function AccountActions() {
  return (
    <Stack direction={{ base: "column", lg: "row" }}>
      <DangerButton
        onConfirm={() => console.log("doing something in account actions")}
        variant={"outline"}
      >
        {"Deactivate my account"}
      </DangerButton>
      <DangerButton
        onConfirm={() => console.log("doing something in account actions")}
      >
        {"Remove my account"}
      </DangerButton>
    </Stack>
  );
}

function UserDataCard({ title, children }: CardProps) {
  return (
    <Card.Root size={"lg"}>
      <Card.Body gapY={4}>
        <Card.Title color={"MenuText"} fontSize={"3xl"} fontWeight={"semibold"}>
          {title}
        </Card.Title>
        <Separator />
        {children}
      </Card.Body>
    </Card.Root>
  );
}

function AccountBase({ base }: { base: typeof userBase }) {
  const { userName, email, avatar } = base;
  return (
    <>
      <Avatar.Root
        aspectRatio={"square"}
        width={{ base: "4em", mdDown: "3/4" }}
        height={"auto"}
      >
        <Avatar.Image src={avatar} />
        <Avatar.Fallback>+</Avatar.Fallback>
      </Avatar.Root>

      <Stack>
        <Card.Title fontSize={"4xl"} fontWeight={"black"}>
          {userName}
        </Card.Title>
        <Card.Description fontSize={"xl"}>{email}</Card.Description>
      </Stack>
    </>
  );
}

function MapStats({ stats }: { stats: object }) {
  const mappedObject = ObjectToArray(stats);

  return mappedObject.map(([key, data]) => (
    <MockupField2 label={key} value={data} />
  ));
}

interface Props {
  label: string;
  value: string | number;
}

function MockupField2({ label, value }: Props) {
  return (
    <Field.Root width={{ base: "full", md: "md" }}>
      <Field.Label fontSize={"md"} color={"GrayText"}>
        {label}
      </Field.Label>
      <Input
        type="text"
        value={value}
        readOnly={true}
        size={{ base: "md", lg: "lg" }}
        variant={"flushed"}
      />
    </Field.Root>
  );
}

export default UserProfilePage;
