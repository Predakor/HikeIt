import { DangerButton, LogoutButton } from "@/components/ui/Buttons";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import type { UserAccountState } from "@/types/User/user.types";
import { Stack } from "@chakra-ui/react";
import { MapStats } from "../Shared/shared";

export function UserAccountDetails({ data }: { data: UserAccountState }) {
  return (
    <SimpleCard title="Account Details" footer={<AccountActionButtons />}>
      <MapStats stats={data} />
    </SimpleCard>
  );
}

function AccountActionButtons() {
  return (
    <Stack
      direction={{
        base: "column",
        lg: "row",
      }}
    >
      <LogoutButton />

      <DangerButton
        onConfirm={() => console.log("doing something in account actions")}
        variant={"outline"}
        children={"Deactivate my account"}
      />

      <DangerButton
        onConfirm={() => console.log("doing something in account actions")}
        children={"Remove my account"}
      />
    </Stack>
  );
}
