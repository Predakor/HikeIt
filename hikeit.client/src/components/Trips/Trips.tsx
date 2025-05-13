import useFetch from "@/hooks/useFetch";
import { Card, Flex, Stat } from "@chakra-ui/react";

interface Trip {
  id: number;
  height: number; // Required float
  length: number; // Required float
  duration: number; // Required float
  tripDay?: string; // DateOnly will be represented as a string in TypeScript (ISO format)

  regionID: number; // Required int
}

interface Region {
  id: number;
  name: string;
}

const formatDate = (dateString: string): string => {
  const date = new Date(dateString);
  return date.toLocaleDateString("en-US", {
    year: "numeric",
    month: "long",
    day: "2-digit",
  });
};

function Trips() {
  const { data, error, loading } = useFetch<Trip[]>("trips");

  if (loading) {
    return <p>Wait a momment api is thinking</p>;
  }

  if (error) {
    return <p>Hmmm looks like api is not aping</p>;
  }

  if (!data) {
    return <p>No data ;c</p>;
  }

  return (
    <Flex gap={5}>
      {data.map((trip) => {
        const formatedDate = formatDate(trip.tripDay as string);

        return (
          <Card.Root>
            <Card.Header></Card.Header>
            <Card.Body>
              <Stat.Root>
                <Stat.ValueText alignItems="baseline">
                  {trip.length} <Stat.ValueUnit>km</Stat.ValueUnit>
                </Stat.ValueText>
              </Stat.Root>

              <Stat.Root>
                <Stat.ValueText alignItems="baseline">
                  {trip.height} <Stat.ValueUnit>Meters</Stat.ValueUnit>
                </Stat.ValueText>
              </Stat.Root>

              <Stat.Root>
                <Stat.ValueText alignItems="baseline">
                  {trip.duration} <Stat.ValueUnit>Minutes</Stat.ValueUnit>
                </Stat.ValueText>
              </Stat.Root>
            </Card.Body>

            <Card.Footer>{formatedDate}</Card.Footer>
          </Card.Root>
        );
      })}
    </Flex>
  );
}

export default Trips;
