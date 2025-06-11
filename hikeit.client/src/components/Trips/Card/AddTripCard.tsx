import { Card, Icon } from "@chakra-ui/react";
import type React from "react";
import { HiPlus } from "react-icons/hi";
import { cardCommonStyles } from "./_commonStyles";

interface Props {
  clickHandler?: React.MouseEventHandler<HTMLDivElement>;
}

function AddTripCard({ clickHandler }: Props) {
  return (
    <Card.Root onClick={clickHandler} {...cardCommonStyles}>
      <Card.Header as="center">
        <Card.Title>New Trip</Card.Title>
      </Card.Header>
      <Card.Body alignItems={"center"}>
        <Icon fontSize={"5xl"} padding={".125em"}>
          <HiPlus />
        </Icon>
      </Card.Body>
    </Card.Root>
  );
}

export default AddTripCard;
