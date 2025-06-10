import { Heading, Span, Stack } from "@chakra-ui/react";
import type { PropsWithChildren } from "react";
import Nav from "./Nav/Nav";

function Layout({ children }: PropsWithChildren) {
  return (
    <Stack
      py={4}
      px={6}
      minH={"100vh"}
      overflowX={"clip"}
      align={"stretch"}
      gap={8}
      gapY={16}
    >
      <header>
        <Heading fontSize={"4xl"}>
          Hike
          <Span fontSize={"4xl"} color={"blue"}>
            IT
          </Span>
        </Heading>
        <Nav />
      </header>
      <Stack as="main" flexGrow={1}>
        {children}
      </Stack>
      <footer>Footer placeholder </footer>
    </Stack>
  );
}

export default Layout;
