import { Stack } from "@chakra-ui/react";
import type { PropsWithChildren } from "react";
import Header from "./Header/Header";
import Footer from "./Footer/Footer";

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
      <Header />
      <Stack as="main" flexGrow={1}>
        {children}
      </Stack>
      <Footer />
    </Stack>
  );
}

export default Layout;
