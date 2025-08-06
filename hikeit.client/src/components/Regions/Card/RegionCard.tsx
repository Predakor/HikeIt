import { cardCommonStyles } from "@/components/Trips/Card/_commonStyles";
import { Card } from "@chakra-ui/react";
import type { ReactNode } from "react";

interface Props {
  Header: ReactNode;
  Description: ReactNode;
  Footer?: ReactNode;
}

function RegionCard({ Header, Description, Footer }: Props) {
  return (
    <Card.Root {...cardCommonStyles}>
      <Card.Header>{Header}</Card.Header>
      <Card.Body>
        <Card.Description>{Description}</Card.Description>
      </Card.Body>
      {Footer && <Card.Footer>{Footer}</Card.Footer>}
    </Card.Root>
  );
}
export default RegionCard;
