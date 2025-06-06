import { dateOnlyToString } from "@/Utils/Formatters/dateFormats";
import useRegion from "@/hooks/useRegion";
import { Card, Stat } from "@chakra-ui/react";
import { Link } from "react-router";
import type { TripDto } from "../../types/types";

interface Props {
  data: TripDto;
}

function TripCard({ data }: Props) {
  const trip = data.base;
  const formatedDate = dateOnlyToString(trip.tripDay as string);
  const { data: region } = useRegion(data.regionId);

  return (
    <Link to={`${data.id}`}>
      <Card.Root>
        <Card.Header>{region && region.name}</Card.Header>
        <Card.Body>
          <Stat.Root>
            <Stat.ValueText alignItems="baseline">
              Total Distance {trip.distance} <Stat.ValueUnit>km</Stat.ValueUnit>
            </Stat.ValueText>
          </Stat.Root>

          <Stat.Root>
            <Stat.ValueText alignItems="baseline">
              Total uphil {trip.height} <Stat.ValueUnit>Meters</Stat.ValueUnit>
            </Stat.ValueText>
          </Stat.Root>

          <Stat.Root>
            <Stat.ValueText alignItems="baseline">
              Duration {trip.duration} <Stat.ValueUnit>Minutes</Stat.ValueUnit>
            </Stat.ValueText>
          </Stat.Root>
        </Card.Body>

        <Card.Footer>{formatedDate}</Card.Footer>
      </Card.Root>
    </Link>
  );
}

export default TripCard;
