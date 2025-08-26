import { Stack } from "@chakra-ui/react";
import type { PropsWithChildren } from "react";
import Header from "./Header/Header";
import Footer from "./Footer/Footer";
import { Toaster } from "@/components/ui/toaster";
import useUser from "@/hooks/Auth/useUser";

function Layout({ children }: PropsWithChildren) {
  //prefetch the user data
  // if no user it will redirect to
  useUser();

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
      <Toaster />
      <Stack as="main" flexGrow={1}>
        {children}
      </Stack>
      <Footer />
    </Stack>
  );
}

export default Layout;
