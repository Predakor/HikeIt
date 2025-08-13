import { Card, CardTitle, Stack } from "@chakra-ui/react";
import type { CardProps } from "./Common/card.types";

export default function HorizontalCard(props: CardProps) {
  const { title, children, header, footer } = props;

  return (
    <Stack direction={{ base: "column", lg: "row" }} asChild>
      <Card.Root as={"article"}>
        <Card.Header>
          {title && <CardTitle title={title} />}
          {header && header}
        </Card.Header>
        <Card.Body>{children}</Card.Body>
        {footer && <Card.Footer>{footer}</Card.Footer>}
      </Card.Root>
    </Stack>
  );
}
