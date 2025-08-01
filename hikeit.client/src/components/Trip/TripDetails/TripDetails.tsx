import { useTripRemove } from "@/hooks/useTrips";
import type { TripDto, TripDtoFull } from "@/types/ApiTypes/TripDtos";
import { Button, Flex, Heading, Icon, Stack, VStack } from "@chakra-ui/react";
import { tripDetailsTabs } from "../Data/tabOrder";
import { TripDetailsMenu } from "./TripDetailsMenu/TripDetailsTabs";
import apiClient from "@/Utils/Api/ApiClient";
import { useQuery } from "@tanstack/react-query";
import { MdOutlineArrowBack } from "react-icons/md";
import { useNavigate } from "react-router";

function TripDetails({ data }: { data: TripDtoFull }) {
  const tabOrder = tripDetailsTabs;

  const baseInfo = useQuery<TripDto>({
    queryKey: ["trip-base", data.id],
    queryFn: () => apiClient<TripDto>(`trips/${data.id}/`),
    staleTime: 1000 * 60 * 30,
  });

  const navigation = useNavigate();
  const deleteTrip = useTripRemove();

  return (
    <VStack alignItems={"start"} gap={8}>
      <Flex alignItems={"center"} gapX={4}>
        <Button onClick={() => navigation(-1)} variant={"ghost"}>
          <Icon size={"2xl"}>
            <MdOutlineArrowBack />
          </Icon>
        </Button>
        {baseInfo.isSuccess && (
          <>
            <Heading size={{ base: "2xl", lg: "4xl" }}>
              {baseInfo.data.name}
            </Heading>
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
