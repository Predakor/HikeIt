import ExternalLink from "@/components/ui/Links/ExternalLink";
import { Center, Flex, Stack, Text } from "@chakra-ui/react";
import { FaGithub } from "react-icons/fa";

export default function Footer() {
  return (
    <Center as="footer">
      <Stack direction={{ base: "column", lg: "row" }} gap={8}>
        <Flex gap={2} alignItems={"center"}>
          <Text>Created and developed by</Text>
          <ExternalLink
            fontSize={"lg"}
            href="https://github.com/Predakor"
            label="Patryk Buśko"
            icon={<FaGithub />}
          />
        </Flex>
      </Stack>
    </Center>
  );
}
