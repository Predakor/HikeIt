import { Button, ButtonGroup, EmptyState, VStack } from "@chakra-ui/react";
import { HiColorSwatch } from "react-icons/hi";
import { Link } from "react-router";

export function NoTrips() {
  return (
    <EmptyState.Root>
      <EmptyState.Content>
        <EmptyState.Indicator>
          <HiColorSwatch />
        </EmptyState.Indicator>
        <VStack textAlign="center">
          <EmptyState.Title>Your first trip</EmptyState.Title>
          <EmptyState.Description>
            Add your trips to continue
          </EmptyState.Description>
        </VStack>
        <ButtonGroup>
          <Link to="/trips/add">
            <Button>Create your first trip</Button>
          </Link>
        </ButtonGroup>
      </EmptyState.Content>
    </EmptyState.Root>
  );
}
export default NoTrips;
