import type { TripDtoFull } from "@/types/ApiTypes/TripDtos";
import { Flex, Heading, VStack } from "@chakra-ui/react";
import { tripDetailsTabs } from "../Data/tabOrder";
import { TripDetailsMenu } from "./TripDetailsMenu/TripDetailsTabs";

function TripDetails({ data }: { data: TripDtoFull }) {
  const tabOrder = tripDetailsTabs.filter((d) => {
    return !(d.type !== "group" && d.key === "base");
  });
  console.log(data);

  return (
    <VStack alignItems={"start"} w={{ base: "full", md: "60vw" }} gap={"2em"}>
      <Flex alignItems={"flex-start"} gap={40}>
        <Heading fontSize={"4xl"}>{data.base.name}</Heading>
        <Heading fontSize={"2xl"}>{data.base.tripDay}</Heading>
      </Flex>
      <TripDetailsMenu data={data} config={tabOrder} />
    </VStack>
  );
}
export default TripDetails;
