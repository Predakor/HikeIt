import { IconPlus } from "@/Icons/Icons";
import { Dialog } from "@/components/ui/Dialog";
import { Flex, Icon } from "@chakra-ui/react";
import { useState } from "react";
import { UserAvatar } from "./UserAvatar";
import { UploadAvatarForm } from "./UploadAvatarForm";

function EditableAvatar({ user }: { user: { avatar: string } }) {
  const [showUpload, setShowUpload] = useState(false);

  return (
    <>
      <Flex
        className="group"
        justify={"center"}
        position={"relative"}
        _hover={{ cursor: "pointer" }}
        onClick={() => setShowUpload(true)}
      >
        <UserAvatar user={user} />
        <Icon
          asChild
          opacity={0}
          scale={"0.25"}
          _groupHover={{
            opacity: 1,
            scale: "0.5",
          }}
          transition={"scale opacity"}
          transitionDuration={"slow"}
          position={"absolute"}
          h={"full"}
          w={"full"}
          inset={0}
          aria-label="Change user avatar"
        >
          <IconPlus />
        </Icon>
      </Flex>

      <Dialog
        title="Upload avatar"
        open={showUpload}
        onOpenChange={({ open }) => setShowUpload(open)}
      >
        <UploadAvatarForm />
      </Dialog>
    </>
  );
}
export default EditableAvatar;
