import { Flex, Box, Em } from "@chakra-ui/react";

function Divider() {
  return (
    <Flex
      direction={{
        base: "row",
        md: "column",
      }}
      align="center"
      alignSelf="stretch"
    >
      <Box flex="1" minHeight={1} minWidth=".125em" bgColor={"bg.emphasized"} />
      <Em mx={2} my={2}>
        Or
      </Em>
      <Box flex="1" minHeight={1} minWidth=".125em" bgColor={"bg.emphasized"} />
    </Flex>
  );
}
export default Divider;
