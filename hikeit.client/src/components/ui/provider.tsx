"use client";

import { ChakraProvider } from "@chakra-ui/react/styled-system";
import { ColorModeProvider, type ColorModeProviderProps } from "./color-mode";
import { defaultSystem } from "@chakra-ui/react/preset";

export function Provider(props: ColorModeProviderProps) {
  return (
    <ChakraProvider value={defaultSystem}>
      <ColorModeProvider {...props} />
    </ChakraProvider>
  );
}
