import SimpleCard from "@/components/ui/Cards/SimpleCard";
import { SimpleGrid, type ConditionalValue } from "@chakra-ui/react";
import type { ReactNode } from "react";

interface Props {
  title: string;
  children: ReactNode;
  columns?: ConditionalValue<number>;
}

export default function StatsCard({ title, children, columns }: Props) {
  const cols = columns || { base: 2, lg: 3 };
  return (
    <SimpleCard title={title}>
      <SimpleGrid columns={cols} gap={8}>
        {children}
      </SimpleGrid>
    </SimpleCard>
  );
}
