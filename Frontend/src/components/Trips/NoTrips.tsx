import {
  Button,
  ButtonGroup,
  EmptyState,
  GridItem,
  VStack,
} from "@chakra-ui/react";
import { HiColorSwatch } from "react-icons/hi";
import { Link } from "react-router";

export function NoTrips() {
  return (
    <GridItem colSpan={4}>
      <EmptyState.Root alignSelf={"center"} size={"lg"}>
        <EmptyState.Content>
          <EmptyState.Indicator>
            <HiColorSwatch />
          </EmptyState.Indicator>
          <VStack textAlign="center">
            <EmptyState.Title>You have no trips yet</EmptyState.Title>
            <EmptyState.Description>
              Add your first trip to continue
            </EmptyState.Description>
          </VStack>
          <ButtonGroup>
            <Link to="/trips/add">
              <Button>Create your first trip</Button>
            </Link>
          </ButtonGroup>
        </EmptyState.Content>
      </EmptyState.Root>
    </GridItem>
  );
}
export default NoTrips;
