import { Drawer, Icon, Portal, Stack, useDisclosure } from "@chakra-ui/react";
import type { ReactElement } from "react";
import { RxCross2, RxHamburgerMenu } from "react-icons/rx";

export default function MobileNav({ children }: { children: ReactElement }) {
  const fontSize = "4xl";
  const { open, setOpen } = useDisclosure();

  const close = () => setOpen(false);

  return (
    <Drawer.Root open={open}>
      <Drawer.Backdrop />
      <Drawer.Trigger fontSize={fontSize} asChild>
        <Icon onClick={() => setOpen(true)}>
          <RxHamburgerMenu />
        </Icon>
      </Drawer.Trigger>
      <Portal>
        <Drawer.Positioner>
          <Drawer.Content>
            <Drawer.Header fontSize={fontSize} py={6} px={4}>
              <Drawer.Title fontSize={"inherit"}>Navigation</Drawer.Title>
              <Drawer.CloseTrigger fontSize={fontSize} asChild pos="initial">
                <Icon onClick={close}>
                  <RxCross2 />
                </Icon>
              </Drawer.CloseTrigger>
            </Drawer.Header>
            <Drawer.Body display={"flex"}>
              <Stack onClick={close} fontSize={"3xl"} gapY={8}>
                {children}
              </Stack>
            </Drawer.Body>
          </Drawer.Content>
        </Drawer.Positioner>
      </Portal>
    </Drawer.Root>
  );
}
