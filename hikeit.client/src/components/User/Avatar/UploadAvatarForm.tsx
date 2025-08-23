import DropFile from "@/components/ui/AddFile/DropFile";
import { PrimaryButton } from "@/components/ui/Buttons";
import { MutationResult } from "@/components/ui/Results/MutationResult";
import useUserMutations from "@/hooks/User/useUserMutations";
import { Avatar, Field, Flex, Show, Stack } from "@chakra-ui/react";
import { useEffect, useState } from "react";

export function UploadAvatarForm() {
  const [avatarPreview, setAvatarPreview] = useState<string>();
  const [avatar, setAvatar] = useState<File>();
  const { updateAvatar } = useUserMutations();

  useEffect(() => {
    if (avatar) {
      setAvatarPreview(URL.createObjectURL(avatar));
    }
  }, [avatar]);

  const handleUpload = () => {
    if (updateAvatar.isPending || !avatar) {
      return;
    }
    updateAvatar.mutateAsync(avatar);
  };

  return (
    <Stack gapY={8}>
      <Show when={updateAvatar.isError}>
        <MutationResult
          requestState={updateAvatar}
          errorMessage={{
            title: "Failed to upload avatar",
            description: updateAvatar.error?.message!,
          }}
        />
      </Show>

      <Show when={avatarPreview}>
        <Flex justify={"center"}>
          <Avatar.Root width={"300px"} height={"300px"}>
            <Avatar.Image src={avatarPreview} />
            <Avatar.Fallback />
          </Avatar.Root>
        </Flex>
      </Show>

      <Field.Root>
        <DropFile
          allowedFiles={[".png", ".jpg", ".webp", ".avif"]}
          onFileChange={setAvatar}
          label="Add user avatar"
        />
      </Field.Root>

      <Show when={avatarPreview}>
        <PrimaryButton
          onClick={handleUpload}
          loading={updateAvatar.isPending}
          loadingText={"Uploading"}
        >
          Upload
        </PrimaryButton>
      </Show>
    </Stack>
  );
}
