import SubTitle from "@/components/ui/Titles/SubTitle";
import { Flex, Spacer, Stack } from "@chakra-ui/react";
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
    <Card.Root as={"article"} height={"full"}>
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
      <Card.Body asChild p={{ base: 4, lg: 8 }}>
        <Stack gapY={8}>{children}</Stack>
      </Card.Body>
      {footer && <Card.Footer>{footer}</Card.Footer>}
    </Card.Root>
  );
}
