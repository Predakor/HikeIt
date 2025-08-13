import { Avatar } from "@chakra-ui/react";

export function UserAvatar({ user }: { user: { avatar: string } }) {
  return (
    <Avatar.Root
      aspectRatio={"square"}
      width={{
        base: "5em",
        mdDown: "3/4",
      }}
      height={"auto"}
    >
      <Avatar.Image src={user.avatar} />
      <Avatar.Fallback>+</Avatar.Fallback>
    </Avatar.Root>
  );
}
