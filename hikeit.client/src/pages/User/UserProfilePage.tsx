import { ObjectToArray } from "@/Utils/ObjectToArray";
import { UserHeaderCard } from "@/components/User/Profile/UserHeaderCard";
import FetchWrapper from "@/components/Wrappers/Fetching/FetchWrapper";
import { LogoutButton } from "@/components/ui/Buttons";
import DangerButton from "@/components/ui/Buttons/DangerButton";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import useUserData from "@/hooks/User/useUserData";
import { Field, Input, Stack } from "@chakra-ui/react";

function UserProfilePage() {
  const getUser = useUserData();
  return (
    <FetchWrapper request={getUser}>
      {(user) => (
        <Stack>
          <UserHeaderCard user={user.base} />

          <SimpleCard title="Personal Information">
            <MapStats stats={user.personal} />
          </SimpleCard>

          <SimpleCard title="Account Details" footer={<AccountActions />}>
            <MapStats stats={user.accountState} />
          </SimpleCard>
        </Stack>
      )}
    </FetchWrapper>
  );
}

function AccountActions() {
  return (
    <Stack direction={{ base: "column", lg: "row" }}>
      <LogoutButton />
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

function MapStats({ stats }: { stats: object }) {
  const mappedObject = ObjectToArray(stats);

  return mappedObject.map(([key, data]) => (
    <InfoField label={key} value={data} />
  ));
}

interface Props {
  label: string;
  value: string | number;
}

function InfoField({ label, value }: Props) {
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
