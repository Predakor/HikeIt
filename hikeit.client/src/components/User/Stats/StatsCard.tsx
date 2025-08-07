import PageTitle from "@/components/Titles/PageTitle";
import { Card, Center, SimpleGrid } from "@chakra-ui/react";
import type { ReactNode } from "react";

interface Props {
  title: string;
  children: ReactNode;
}

export default function StatsCard({ title, children }: Props) {
  return (
    <Card.Root gapY={4}>
      <Card.Header>
        <Center>
          <PageTitle title={title} />
        </Center>
      </Card.Header>
      <Card.Body>
        <SimpleGrid columns={{ base: 2, lg: 3 }} gap={8}>
          {children}
        </SimpleGrid>
      </Card.Body>
    </Card.Root>
  );
}
