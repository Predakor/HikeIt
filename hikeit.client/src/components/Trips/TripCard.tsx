import type { Trip } from "@/data/types";
import { dateOnlyToString } from "@/Utils/DateFormats";
import { Card, Stat } from "@chakra-ui/react";

function TripCard({ trip }: { trip: Trip }) {
  const formatedDate = dateOnlyToString(trip.tripDay as string);

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
}

export default TripCard;
