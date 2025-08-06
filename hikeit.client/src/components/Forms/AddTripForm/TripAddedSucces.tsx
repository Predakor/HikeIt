import { IconArrowRight, IconPlus } from "@/Icons/Icons";
import Divider from "@/components/Divider/Divider";
import { Button, Card, Stack } from "@chakra-ui/react";
import { useNavigate } from "react-router";

interface TripAddedSuccesProps {
  tripLocation: string;
  resetForm: () => void;
}
export function TripAddedSucces({
  tripLocation,
  resetForm,
}: TripAddedSuccesProps) {
  const navigation = useNavigate();

  const navigateToTrip = () => navigation(tripLocation);

  return (
    <Card.Root>
      <Card.Header textAlign={"center"}>
        <Card.Title fontSize={{ base: "xl", lg: "3xl" }}>
          🎉 Trip Created Successfully 🎉
        </Card.Title>
      </Card.Header>
      <Card.Body>
        <Card.Description fontSize={{ base: "md", lg: "xl" }}>
          Your trip has been saved.
          <br />
          You can view its details or create another one below.
        </Card.Description>
      </Card.Body>
      <Card.Footer>
        <Stack
          width={"full"}
          justifyContent={"space-around"}
          direction={{ base: "column", lg: "row" }}
          gap={4}
        >
          <Button
            size={{ base: "md", lg: "xl" }}
            colorPalette="blue"
            onClick={resetForm}
          >
            Add New Trip <IconPlus />
          </Button>

          <Divider />

          <Button
            size={{ base: "md", lg: "xl" }}
            colorPalette={"green"}
            onClick={navigateToTrip}
          >
            View Your Trip <IconArrowRight />
          </Button>
        </Stack>
      </Card.Footer>
    </Card.Root>
  );
}
