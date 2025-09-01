import { IconLogo } from "@/Icons/Icons";
import { Group, Heading, Icon, Span } from "@chakra-ui/react";

export function PageLogo() {
  return (
    <Group className="group" fontSize="4xl" gapX={2}>
      <Icon
        _groupHover={{
          color: "fg",
        }}
        transition="color 100ms"
        color="fg.muted"
        size="inherit"
      >
        <IconLogo />
      </Icon>
      <Heading color="fg" fontSize={"inherit"} fontWeight={"semibold"}>
        Hike
        <Span fontSize={"smaller"} color={"blue"} fontWeight={"bold"}>
          IT
        </Span>
      </Heading>
    </Group>
  );
}
