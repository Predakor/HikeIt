import { SimpleGrid, Skeleton } from "@chakra-ui/react";

interface Props {
  height?: number;
  amount?: number;
}

function SkeletonGrid({ amount = 8, height = 200 }: Props) {
  return (
    <SimpleGrid columns={{ base: 1, lg: 4 }} gap={8}>
      {Array.from({ length: amount }).map((_, i) => (
        <Skeleton key={i} height={`${height}px`} />
      ))}
    </SimpleGrid>
  );
}
export default SkeletonGrid;
