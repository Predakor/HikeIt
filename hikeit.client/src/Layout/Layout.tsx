import { Stack } from "@chakra-ui/react";
import type { PropsWithChildren } from "react";
import Header from "./Header/Header";
import Footer from "./Footer/Footer";

function Layout({ children }: PropsWithChildren) {
  return (
    <Stack
      minH={"100vh"}
      align={"stretch"}
      overflowX={"clip"}
      py={4}
      px={{ base: 4, lg: 8 }}
      gap={12}
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
