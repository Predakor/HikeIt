import api from "@/Utils/Api/apiRequest";
import { ObjectToArray } from "@/Utils/ObjectToArray";
import DangerButton from "@/components/Buttons/DangerButton";
import Divider from "@/components/Divider/Divider";
import {
  Avatar,
  ButtonGroup,
  Card,
  Field,
  Input,
  Separator,
  Stack,
  Stat,
} from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";
import type { ReactElement } from "react";

const userBase = {
  userName: "janusz",
  email: "mistrzbiznesu.wp.pl",
  avatar: "https://assets.puzzlefactory.com/puzzle/190/564/original.jpg",
};

const accountState = {
  status: "active",
  role: "user",
  createdAt: new Date().toDateString(),
};

const userPersonal = {
  firstName: "janusz",
  lastName: "janusz",
  birthDay: undefined,
  country: "poland",
  gender: "Male",
};

const __mockupUser__ = {
  base: userBase,
  personal: userPersonal,
  accountState,
};

function UserProfilePage() {
  const request = useQuery({
    queryKey: ["profile"],
    queryFn: () => api.get("users/profile"),
  });

  const user = __mockupUser__;

  return (
    <Stack>
      <Card.Root>
        <Card.Body>
          <Stack
            alignItems={"center"}
            justifyItems={"center"}
            direction={{ base: "column", md: "row" }}
            gap={4}
          >
            <AccountBase base={user.base} />
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
      <DangerButton variant={"outline"}>{"Deactivate my account"}</DangerButton>
      <DangerButton>{"Remove my account"}</DangerButton>
    </Stack>
  );
}

function UserDataCard({ title, children, Footer }: CardProps) {
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

function MockupField({ label, value }: Props) {
  return (
    <Stat.Root flexFlow={"row"} alignItems={"baseline"} gapX={2}>
      <Stat.Label>{label}</Stat.Label>
      <Stat.ValueText>{value}</Stat.ValueText>
    </Stat.Root>
  );
}

export default UserProfilePage;
