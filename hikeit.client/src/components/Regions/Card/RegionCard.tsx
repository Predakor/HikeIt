import type { Region } from "@/data/types";
import { Box, Card, Heading, Text } from "@chakra-ui/react";
import RegionProgress from "./RegionProgress";

interface Props {
  region: Region;
}

function RegionCard({ region }: Props) {
  const randomRange = Math.floor(Math.random() * 20);
  const total = 20;
  return (
    <Card.Root>
      <Card.Header>
        <Heading>{region.name}</Heading>
      </Card.Header>
      <Card.Body>
        <Text>
          Lorem ipsum dolor sit amet consectetur adipisicing elit. Reiciendis,
          eveniet perspiciatis laudantium eos itaque aperiam in molestiae sed
          distinctio! Facilis?
        </Text>
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
