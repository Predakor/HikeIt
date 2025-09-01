import SubTitle from "@/components/ui/Titles/SubTitle";
import { Flex, Show, Spacer, Stack, type StackProps } from "@chakra-ui/react";
import { Card } from "@chakra-ui/react/card";
import type { CardProps } from "./Common/card.types";

interface Props extends CardProps {
  bodyStyles?: Partial<StackProps>;
}

export default function SimpleCard({
  title,
  children,
  header,
  footer,
  headerCta,
  bodyStyles,
}: Props) {
  return (
    <Card.Root as={"article"} height={"full"}>
      <Card.Header as={"header"}>
        {header ? (
          header
        ) : (
          <Flex>
            <Show when={title}>
              <Card.Title asChild>
                <SubTitle title={title!} />
              </Card.Title>
            </Show>
            <Show when={headerCta}>
              <Spacer />
              {headerCta}
            </Show>
          </Flex>
        )}
      </Card.Header>
      <Card.Body asChild p={{ base: 4, lg: 8 }}>
        <Stack justify={"space-around"} gapY={8} {...bodyStyles}>
          {children}
        </Stack>
      </Card.Body>
      <Show when={footer}>
        <Card.Footer as={"footer"}>{footer}</Card.Footer>
      </Show>
    </Card.Root>
  );
}
