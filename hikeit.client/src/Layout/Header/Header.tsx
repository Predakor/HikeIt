import { Flex, LinkBox, useBreakpointValue } from "@chakra-ui/react";
import { NavLink } from "react-router";
import MobileNav from "../Nav/MobileNav";
import NavContent from "../Nav/NavContent";
import { PageLogo } from "./PageLogo";

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
