import { Stack } from "@chakra-ui/react";
import type { PropsWithChildren } from "react";
import Header from "./Header/Header";
import Footer from "./Footer/Footer";

function Layout({ children }: PropsWithChildren) {
  return (
    <Stack minH={"100vh"} align={"stretch"} overflowX={"clip"} p={4} gap={4}>
      <Header />
      <Stack as="main" flexGrow={1}>
        {children}
      </Stack>
      <Footer />
    </Stack>
  );
}

export default Layout;
