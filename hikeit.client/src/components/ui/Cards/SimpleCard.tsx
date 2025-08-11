import SubTitle from "@/components/Titles/SubTitle";
import { Flex, Spacer } from "@chakra-ui/react";
import { Card } from "@chakra-ui/react/card";
import type { ReactNode } from "react";

interface Props {
  title: string;
  children: ReactNode;
  footer?: ReactNode;
  headerCta?: ReactNode;
}

export default function SimpleCard({
  title,
  children,
  footer,
  headerCta,
}: Props) {
  return (
    <Card.Root as={"article"}>
      <Card.Header>
        <Flex>
          <Card.Title asChild>
            <SubTitle title={title} />
          </Card.Title>
          {headerCta && (
            <>
              <Spacer />
              {headerCta}
            </>
          )}
        </Flex>
      </Card.Header>
      <Card.Body>{children}</Card.Body>
      {footer && <Card.Footer>{footer}</Card.Footer>}
    </Card.Root>
  );
}
