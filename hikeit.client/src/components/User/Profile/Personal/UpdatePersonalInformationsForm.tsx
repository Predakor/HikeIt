import { diffObjects } from "@/Utils/objectHelpers";
import RenderInputs from "@/components/Utils/RenderInputs/RenderInputs";
import type { InputsConfig } from "@/components/Utils/RenderInputs/inputTypes";
import { PrimaryButton } from "@/components/ui/Buttons";
import { MutationResult } from "@/components/ui/Results/MutationResult";
import useUserMutations from "@/hooks/User/useUserMutations";
import type { UserPersonal } from "@/types/User/user.types";
import { Separator, Stack } from "@chakra-ui/react";
import { useForm } from "react-hook-form";

interface Props {
  data: UserPersonal;
}

export default function UpdatePersonalInformationsForm(props: Props) {
  const { email, ...initData } = props.data;
  const { personalInfo } = useUserMutations();

  const formHook = useForm({
    defaultValues: {
      ...initData,
      gender: [initData?.gender ? initData.gender : undefined],
    },
  });

  const handleUpdate = formHook.handleSubmit((currentForm) => {
    console.log(currentForm);

    const changes = diffObjects(initData, {
      ...currentForm,
      gender: currentForm?.gender[0] ?? undefined,
    });

    if (Object.entries(changes).length === 0) {
      alert("No changes detecetd");
      return;
    }

    return personalInfo.mutateAsync(changes);
  });

  return (
    <form onSubmit={handleUpdate}>
      <Stack gapY={4}>
        <MutationResult requestState={personalInfo} />
        <Stack gapY={4}>
          <RenderInputs
            formHook={formHook}
            config={UpdatePersonalInfoConfig}
            displayOptions={{ size: "lg" }}
          />
        </Stack>
        <Separator />
        <PrimaryButton
          type="submit"
          loading={personalInfo.isPending}
          loadingText={"Loading"}
        >
          Upload
        </PrimaryButton>
      </Stack>
    </form>
  );
}

const UpdatePersonalInfoConfig: InputsConfig = [
  { key: "firstName", label: "", type: "text", min: 3, max: 64 },
  { key: "lastName", label: "", type: "text", min: 3, max: 64 },
  {
    key: "birthDay",
    label: "",
    type: "date",
    min: 1,
    max: Date.now(),
  },
  { key: "country", label: "", type: "text", min: 3, max: 255 },
  {
    key: "gender",
    label: "",
    type: "select",
    collection: {
      type: "static",
      items: [
        { value: "Male ", label: "Male" },
        { value: "Female", label: "Female" },
        { value: "Other", label: "Other" },
      ],
    },
  },
];
