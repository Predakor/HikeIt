import type { Region } from "@/types/ApiTypes/region.types";
import { Card, Span } from "@chakra-ui/react";
import { Link } from "react-router";
import RegionCard from "./RegionCard";

function UnprogressedRegion({ region }: { region: Region }) {
  return (
    <Link to={`/regions/${region.id}`}>
      <RegionCard
        Header={
          <Card.Title textAlign={"center"} flexGrow={1} fontSize={"2xl"}>
            {region.name}
          </Card.Title>
        }
        Description={
          <Span color={"fg.muted"} textAlign={"center"}>
            <span>This region hasn't been explored yet.</span>
            <br />
            <span>Add a trip that visits it to unlock progress.</span>
          </Span>
        }
      />
    </Link>
  );
}

export default UnprogressedRegion;
