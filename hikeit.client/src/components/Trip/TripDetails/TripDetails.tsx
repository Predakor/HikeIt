import apiClient from "@/Utils/Api/ApiClient";
import GoBackButton from "@/components/Buttons/GoBackButton";
import { useTripRemove } from "@/hooks/useTrips";
import type { TripDto, TripDtoFull } from "@/types/ApiTypes/TripDtos";
import { Button, Flex, Heading, Stack, VStack } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";
import { tripDetailsTabs } from "../Data/tabOrder";
import { TripDetailsMenu } from "./TripDetailsMenu/TripDetailsTabs";
import PageTitle from "@/components/Titles/PageTitle";

function TripDetails({ data }: { data: TripDtoFull }) {
  const tabOrder = tripDetailsTabs;

  const baseInfo = useQuery<TripDto>({
    queryKey: ["trip-base", data.id],
    queryFn: () => apiClient<TripDto>(`trips/${data.id}/`),
    staleTime: 1000 * 60 * 30,
  });

  const deleteTrip = useTripRemove();

  return (
    <VStack alignItems={"start"} gap={8}>
      <Flex alignItems={"center"} gapX={4}>
        <GoBackButton />
        {baseInfo.isSuccess && (
          <>
            <PageTitle title={baseInfo.data.name} />
            <Heading color={"fg.muted"} size={{ base: "xl", lg: "2xl" }}>
              {baseInfo.data.tripDay}
            </Heading>
          </>
        )}
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
