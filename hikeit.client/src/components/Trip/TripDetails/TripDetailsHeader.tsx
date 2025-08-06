import apiClient from "@/Utils/Api/ApiClient";
import { formatDate } from "@/Utils/Formatters";
import GoBackButton from "@/components/Buttons/GoBackButton";
import PageTitle from "@/components/Titles/PageTitle";
import { useTripRemove } from "@/hooks/UseTrips/useTripRemove";
import type { TripDto } from "@/types/ApiTypes/TripDtos";
import { Button, Flex, Heading, Skeleton } from "@chakra-ui/react";
import { useQuery, type UseQueryResult } from "@tanstack/react-query";

interface Props {
  id: string;
}

function TripDetailsHeader({ id }: Props) {
  const deleteTrip = useTripRemove();

  const baseInfo = useQuery<TripDto>({
    queryKey: ["trip-base", id],
    queryFn: () => apiClient<TripDto>(`trips/${id}/`),
    staleTime: 1000 * 60 * 30,
  });

  return (
    <Flex alignItems={"center"} width={"full"} gapX={4}>
      <GoBackButton />
      <Flex flexGrow={1} alignItems={"center"} gapX={8}>
        <TripName data={baseInfo} />
      </Flex>
      <Button
        onClick={() => deleteTrip.mutate(id)}
        colorPalette={"red"}
        variant={"solid"}
      >
        Delete
      </Button>
    </Flex>
  );
}

function TripName({ data: request }: { data: UseQueryResult<TripDto> }) {
  if (request.isLoading) {
    return <Skeleton width={"full"} height={12}></Skeleton>;
  }

  if (!request.data) {
    return;
  }

  const data = request.data.base;

  return (
    <>
      <PageTitle title={data.name} />
      <Heading
        display={{ base: "none", lg: "block" }}
        color={"fg.muted"}
        size={{ base: "xl", lg: "2xl" }}
      >
        {formatDate.toUkDate(data.tripDay)}
      </Heading>
    </>
  );
}

export default TripDetailsHeader;
