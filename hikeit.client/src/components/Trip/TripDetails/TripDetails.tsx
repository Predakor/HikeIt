import type { TripDtoFull } from "@/types/ApiTypes/TripDtos";
import { Stack, VStack } from "@chakra-ui/react";
import { tripDetailsTabs } from "../Data/tabOrder";
import { TripDetailsMenu } from "./TripDetailsMenu/TripDetailsTabs";
import TripDetailsHeader from "./TripDetailsHeader";

function TripDetails({ data }: { data: TripDtoFull }) {
  const tabOrder = tripDetailsTabs;

  return (
    <VStack alignItems={"start"} gap={8}>
      <TripDetailsHeader id={data.id} />
      <Stack w={"full"} justifyItems={"center"} gap={8}>
        <TripDetailsMenu data={data} config={tabOrder} />
      </Stack>
    </VStack>
  );
}
export default TripDetails;
