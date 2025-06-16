import { menuIcons } from "@/components/Trip/Data/statsInfo";
import { Group, Heading, Icon, LinkBox, Span } from "@chakra-ui/react";
import type { IconType } from "react-icons";
import { NavLink } from "react-router";
import NavContent from "../Nav/Nav";

export default function Header() {
  return (
    <header>
      <Group gapX={8} justifyItems={"end"} as="nav">
        <LinkBox asChild>
          <NavLink to="/">
            <PageLogo />
          </NavLink>
        </LinkBox>
        <NavContent />
      </Group>
    </header>
  );
}

function PageLogo() {
  const IconSource = menuIcons["reachedPeaks"] as IconType;

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
        <IconSource />
      </Icon>
      <Heading color="fg" fontSize={"inherit"}>
        Hike
        <Span fontSize={"smaller"} color={"blue"}>
          IT
        </Span>
      </Heading>
    </Group>
  );
}
