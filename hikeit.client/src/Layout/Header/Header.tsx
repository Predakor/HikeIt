import { IconLogo } from "@/Icons/Icons";
import {
  Flex,
  Group,
  Heading,
  Icon,
  LinkBox,
  Span,
  useBreakpointValue,
} from "@chakra-ui/react";
import { NavLink } from "react-router";
import MobileNav from "../Nav/MobileNav";
import NavContent from "../Nav/NavContent";

export default function Header() {
  const isMobile = useBreakpointValue({ base: true, md: false });

  return (
    <header>
      <Flex fontSize={"2xl"} gapX={8} alignItems={"center"} as="nav">
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
