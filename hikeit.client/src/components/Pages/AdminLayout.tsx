import { Flex, Input, Spacer, Stack } from "@chakra-ui/react";
import GoBackButton from "../Buttons/GoBackButton";
import PageTitle from "../Titles/PageTitle";
import { Props } from "./AdminPage";

export function AdminLayout({ title, children }: Props) {
  return (
    <Stack gapY={8}>
      <Flex alignItems={"center"} direction={"row"}>
        <GoBackButton />
        <PageTitle title={title} />
        <Spacer />
        <Input
          type="search"
          variant={"subtle"}
          size={"lg"}
          // onChange={(e) => setFilterValue(e.target.value)}
          placeholder="Search regions by name"
        />
      </Flex>
      <Stack>{children}</Stack>
    </Stack>
  );
}
