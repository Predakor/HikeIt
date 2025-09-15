import { PrimaryButton } from "@/components/ui/Buttons";
import NavButton from "@/components/ui/Buttons/NavButton";
import useUser from "@/hooks/Auth/useUser";
import {
  Flex,
  LinkBox,
  Show,
  Spacer,
  useBreakpointValue,
} from "@chakra-ui/react";
import { NavLink } from "react-router";
import MobileNav from "../Nav/MobileNav";
import NavContent from "../Nav/NavContent";
import { PageLogo } from "./PageLogo";

export default function Header() {
  const isMobile = useBreakpointValue({ base: true, md: false });
  const { data: isLogged } = useUser();
  return (
    <header>
      <Flex fontSize={"2xl"} gapX={8} alignItems={"center"} as="nav">
        <LinkBox flexGrow={{ base: 1, lg: 0 }}>
          <NavLink to="/">
            <PageLogo />
          </NavLink>
        </LinkBox>

        <Show when={isLogged}>
          {isMobile ? (
            <MobileNav>
              <NavContent />
            </MobileNav>
          ) : (
            <NavContent />
          )}
        </Show>
        <Show when={!isLogged}>
          <Spacer />
          <NavButton to="auth/login" label="Login" />
          <PrimaryButton fontWeight={"bolder"} size={"md"}>
            <NavLink to={"auth/register"}>Register</NavLink>
          </PrimaryButton>
        </Show>
      </Flex>
    </header>
  );
}
