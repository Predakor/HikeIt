import type { TripDtoFull } from "@/types/ApiTypes/TripDtos";
import { Button, Flex, Heading, VStack } from "@chakra-ui/react";
import { tripDetailsTabs } from "../Data/tabOrder";
import { TripDetailsMenu } from "./TripDetailsMenu/TripDetailsTabs";
import apiClient from "@/Utils/Api/ApiClient";
import { useNavigate } from "react-router";

function TripDetails({ data }: { data: TripDtoFull }) {
  const tabOrder = tripDetailsTabs.filter((d) => {
    return !(d.type !== "group" && d.key === "base");
  });

  const navigation = useNavigate();

  const deleteHandler = async () => {
    const res = await apiClient("trips/" + data.id, { method: "DELETE" });
    if (res === null) {
      navigation(-1);
    }
  };

  return (
    <VStack alignItems={"start"} w={{ base: "full", md: "60vw" }} gap={"2em"}>
      <Flex alignItems={"flex-start"} gap={40}>
        <Heading fontSize={"4xl"}>{data.base.name}</Heading>
        <Heading fontSize={"2xl"}>{data.base.tripDay}</Heading>
        <Button onClick={deleteHandler} colorPalette={"red"} variant={"solid"}>
          Delete
        </Button>
      </Flex>
      <TripDetailsMenu data={data} config={tabOrder} />
    </VStack>
  );
}
export default TripDetails;
