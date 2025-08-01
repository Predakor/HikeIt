import type { Region } from "@/data/types";
import { Card } from "@chakra-ui/react";

function UnprogressedRegion({ region }: { region: Region }) {
  return (
    <Card.Root>
      <Card.Header textAlign={"center"}>
        <Card.Title flexGrow={1} fontSize={"2xl"}>
          {region.name}
        </Card.Title>
      </Card.Header>
      <Card.Body>
        <Card.Description textAlign={"center"}>
          <p>This region hasn't been explored yet.</p>
          <p>Add a trip that visits it to unlock progress.</p>
        </Card.Description>
      </Card.Body>
    </Card.Root>
  );
}

export default UnprogressedRegion;
