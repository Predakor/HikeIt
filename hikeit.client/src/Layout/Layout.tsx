import { Center, Heading, Span, Stack } from "@chakra-ui/react";
import type { PropsWithChildren } from "react";
import Nav from "./Nav/Nav";

function Layout({ children }: PropsWithChildren) {
  return (
    <Stack py={"1em"} minH={"100vh"} align={"center"}>
      <header>
        <Heading fontSize={"4xl"}>
          Hike
          <Span fontSize={"4xl"} color={"blue"}>
            IT
          </Span>
        </Heading>
        <Nav />
      </header>
      <Center flexGrow={1}>
        <main>{children}</main>
      </Center>
      <footer>Footer placeholder </footer>
    </Stack>
  );
}

export default Layout;
