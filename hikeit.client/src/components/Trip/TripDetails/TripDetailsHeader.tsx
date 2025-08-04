import apiClient from "@/Utils/Api/ApiClient";
import GoBackButton from "@/components/Buttons/GoBackButton";
import PageTitle from "@/components/Titles/PageTitle";
import { useTripRemove } from "@/hooks/UseTrips/useTripRemove";
import type { TripDto } from "@/types/ApiTypes/TripDtos";
import { Button, Flex, Heading, Skeleton } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";

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

  if (baseInfo.isLoading) {
    return <Skeleton width={"full"} height={12}></Skeleton>;
  }

  const data = baseInfo.data?.base;

  return (
    <Flex alignItems={"center"} gapX={4}>
      <GoBackButton />
      {data && (
        <>
          <PageTitle title={data.name} />
          <Heading color={"fg.muted"} size={{ base: "xl", lg: "2xl" }}>
            {data.tripDay}
          </Heading>
        </>
      )}
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
export default TripDetailsHeader;
