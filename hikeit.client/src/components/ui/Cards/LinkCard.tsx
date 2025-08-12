import { cardCommonStyles } from "@/components/Trips/Card/_commonStyles";
import { Card } from "@chakra-ui/react/card";
import type { ReactNode } from "react";
import { Link } from "react-router";

interface Props {
  to: string;
  Header: ReactNode;
  Description: ReactNode;
  Footer?: ReactNode;
}

export default function LinkCard(props: Props) {
  return (
    <Card.Root as={"article"} {...cardCommonStyles}>
      <Link to={props.to}>
        <Card.Header>{props.Header}</Card.Header>
        <Card.Body>{props.Description}</Card.Body>
        {props.Footer && <Card.Footer>{props.Footer}</Card.Footer>}
      </Link>
    </Card.Root>
  );
}
