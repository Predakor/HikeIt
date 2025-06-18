import type { Region } from "@/data/types";
import { Badge, Box, Card, Flex } from "@chakra-ui/react";
import RegionProgress from "./RegionProgress";

interface Props {
  region: Region;
}

function RegionCard({ region }: Props) {
  const total = 20;
  const randomRange = Math.floor(Math.random() * total + 1);

  const isComplete = randomRange === total;
  return (
    <Card.Root>
      <Card.Header>
        <Flex>
          <Card.Title flexGrow={1} fontSize={"2xl"}>
            {region.name}
          </Card.Title>
          {isComplete && <Badge colorPalette={"green"}>Complete</Badge>}
        </Flex>
      </Card.Header>
      <Card.Body>
        <Card.Description>
          Lorem ipsum dolor sit amet consectetur adipisicing elit. Reiciendis,
          eveniet perspiciatis laudantium eos itaque aperiam in molestiae sed
          distinctio! Facilis?
        </Card.Description>
      </Card.Body>
      <Card.Footer>
        <Box w="100%">
          <RegionProgress total={total} finished={randomRange} />
        </Box>
      </Card.Footer>
    </Card.Root>
  );
}
export default RegionCard;
