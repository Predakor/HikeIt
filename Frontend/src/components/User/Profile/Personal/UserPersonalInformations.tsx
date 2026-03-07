import { IconEdit } from "@/Icons/Icons";
import { PrimaryButton } from "@/components/ui/Buttons";
import SimpleCard from "@/components/ui/Cards/SimpleCard";
import type { UserPersonal } from "@/types/User/user.types";
import { Icon, Span } from "@chakra-ui/react";
import { MapStats } from "../Shared/shared";
import { useState } from "react";
import { Dialog } from "@/components/ui/Dialog";
import UpdatePersonalInformationsForm from "./UpdatePersonalInformationsForm";

export function UserPersonalInformations({ data }: { data: UserPersonal }) {
  const [showForm, setShowForm] = useState<boolean>(false);

  return (
    <>
      <SimpleCard
        title="Personal Information"
        headerCta={<EditButton onClick={() => setShowForm(true)} />}
      >
        <MapStats stats={data} />
      </SimpleCard>
      <Dialog
        title="Update personal data"
        open={showForm}
        onOpenChange={({ open }) => setShowForm(open)}
      >
        <UpdatePersonalInformationsForm data={data} />
      </Dialog>
    </>
  );
}

function EditButton({ onClick }: { onClick: () => void }) {
  return (
    <PrimaryButton onClick={onClick} variant={"outline"} size={"md"}>
      <Span display={{ base: "none", lg: "block" }}>Edit</Span>
      <Icon asChild size={"sm"}>
        <IconEdit />
      </Icon>
    </PrimaryButton>
  );
}
