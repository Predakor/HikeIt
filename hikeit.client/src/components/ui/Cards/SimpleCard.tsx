import SubTitle from "@/components/Titles/SubTitle";
import { Flex, Spacer } from "@chakra-ui/react";
import { Card } from "@chakra-ui/react/card";
import type { CardProps } from "./Common/card.types";

export default function SimpleCard({
  title,
  children,
  header,
  footer,
  headerCta,
}: CardProps) {
  return (
    <Card.Root as={"article"}>
      <Card.Header as={"header"}>
        {header ? (
          header
        ) : (
          <Flex>
            {title && (
              <Card.Title asChild>
                <SubTitle title={title} />
              </Card.Title>
            )}
            {headerCta && (
              <>
                <Spacer />
                {headerCta}
              </>
            )}
          </Flex>
        )}
      </Card.Header>
      <Card.Body>{children}</Card.Body>
      {footer && <Card.Footer>{footer}</Card.Footer>}
    </Card.Root>
  );
}
