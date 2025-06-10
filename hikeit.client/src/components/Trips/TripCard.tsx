import { dateOnlyToString } from "@/Utils/Formatters/dateFormats";
import useRegion from "@/hooks/useRegion";
import type { TripDto } from "@/types/ApiTypes/TripDtos";
import { Card, Stat } from "@chakra-ui/react";
import { Link } from "react-router";

interface Props {
  data: TripDto;
}

function TripCard({ data }: Props) {
  const trip = data.base;
  const formatedDate = dateOnlyToString(trip.tripDay as string);
  const { data: region } = useRegion(data.regionId);

  return (
    <Link to={`${data.id}`}>
      <Card.Root
        w={"20vw"}
        transition="all .25s ease-in"
        _hover={{
          bg: "bg.emphasized",
          transform: "scale(1.08)",
          borderRadius: "5%",
          transition: "all .15s ease-out",
        }}
      >
        <Card.Header>{region && region.name}</Card.Header>
        <Card.Body>
          <Stat.Root>
            <Stat.ValueText alignItems="baseline">
              {data.base.name}{" "}
            </Stat.ValueText>
          </Stat.Root>
        </Card.Body>

        <Card.Footer>{formatedDate}</Card.Footer>
      </Card.Root>
    </Link>
  );
}

export default TripCard;
