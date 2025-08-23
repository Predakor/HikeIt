import { formatDate } from "@/Utils/Formatters";
import PageTitle from "@/components/ui/Titles/PageTitle";
import GoBackButton from "@/components/ui/Buttons/GoBackButton";
import { useTripRemove } from "@/hooks/UseTrips/useTripRemove";
import type { TripWithBasicAnalytics } from "@/types/ApiTypes/TripDtos";
import { Button, Flex, Heading } from "@chakra-ui/react";

interface Props {
  trip: TripWithBasicAnalytics;
}

export default function TripDetailsHeader({ trip }: Props) {
  const deleteTrip = useTripRemove();

  return (
    <Flex alignItems={"center"} width={"full"} gapX={4}>
      <GoBackButton />
      <Flex flexGrow={1} alignItems={"center"} gapX={8}>
        <PageTitle title={trip.name} />
        <Heading
          display={{ base: "none", lg: "block" }}
          color={"fg.muted"}
          size={{ base: "xl", lg: "2xl" }}
        >
          {formatDate.toUkDate(trip.tripDay)}
        </Heading>
      </Flex>
      <Button
        onClick={() => deleteTrip.mutate(trip.id)}
        colorPalette={"red"}
        variant={"solid"}
      >
        Delete
      </Button>
    </Flex>
  );
}
