import type { Region } from "@/types/ApiTypes/types";
import { Card } from "@chakra-ui/react";
import { Link } from "react-router";

function UnprogressedRegion({ region }: { region: Region }) {
  return (
    <Link to={`regions/${region.id}`}>
      <Card.Root>
        <Card.Header textAlign={"center"}>
          <Card.Title flexGrow={1} fontSize={"2xl"}>
            {region.name}
          </Card.Title>
        </Card.Header>
        <Card.Body>
          <Card.Description textAlign={"center"}>
            <span>This region hasn't been explored yet.</span>
            <br />
            <span>Add a trip that visits it to unlock progress.</span>
          </Card.Description>
        </Card.Body>
      </Card.Root>
    </Link>
  );
}

export default UnprogressedRegion;
