import { menuIcons } from "@/components/Trip/Data/statsInfo";
import {
  Flex,
  Group,
  Heading,
  Icon,
  LinkBox,
  Span,
  useBreakpointValue,
} from "@chakra-ui/react";
import type { IconType } from "react-icons";
import { NavLink } from "react-router";
import MobileNav from "../Nav/MobileNav";
import NavContent from "../Nav/NavContent";

export default function Header() {
  const isMobile = useBreakpointValue({ base: true, md: false });

  return (
    <header>
      <Flex fontSize={"xl"} gapX={8} alignItems={"center"} as="nav">
        <LinkBox flexGrow={{ base: 1, lg: 0 }}>
          <NavLink to="/">
            <PageLogo />
          </NavLink>
        </LinkBox>

        {isMobile ? (
          <MobileNav>
            <NavContent />
          </MobileNav>
        ) : (
          <NavContent />
        )}
      </Flex>
    </header>
  );
}

export function PageLogo() {
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
