import { useTripRemove } from "@/hooks/useTrips";
import type { TripDtoFull } from "@/types/ApiTypes/TripDtos";
import { Button, Flex, Heading, Stack, VStack } from "@chakra-ui/react";
import { tripDetailsTabs } from "../Data/tabOrder";
import { TripDetailsMenu } from "./TripDetailsMenu/TripDetailsTabs";

function TripDetails({ data }: { data: TripDtoFull }) {
  const tabOrder = tripDetailsTabs.filter((d) => {
    return !(d.type !== "group" && d.key === "base");
  });

  const deleteTrip = useTripRemove();

  return (
    <VStack alignItems={"start"} gap={"2em"}>
      <Flex alignItems={"center"} gapX={4}>
        <Heading size={{ base: "2xl", lg: "4xl" }}>{data.base.name}</Heading>
        <Heading color={"fg.muted"} size={{ base: "xl", lg: "2xl" }}>
          {data.base.tripDay}
        </Heading>
        <Button
          onClick={() => deleteTrip.mutate(data.id)}
          colorPalette={"red"}
          variant={"solid"}
        >
          Delete
        </Button>
      </Flex>
      <Stack w={"full"} justifyItems={"center"} gap={8}>
        <TripDetailsMenu data={data} config={tabOrder} />
      </Stack>
    </VStack>
  );
}
export default TripDetails;
