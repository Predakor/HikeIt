import { Center, Heading, Span, Stack } from "@chakra-ui/react";
import type { PropsWithChildren } from "react";
import Nav from "./Nav/Nav";

function Layout({ children }: PropsWithChildren) {
  return (
    <Stack p={"1em"} minH={"100vh"} align={"stretch"} gap={"2em"}>
      <header>
        <Heading fontSize={"4xl"}>
          Hike
          <Span fontSize={"4xl"} color={"blue"}>
            IT
          </Span>
        </Heading>
        <Nav />
      </header>
      <Center as="main" flexGrow={1}>
        {children}
      </Center>
      <footer>Footer placeholder </footer>
    </Stack>
  );
}

export default Layout;
