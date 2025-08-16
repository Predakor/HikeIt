import { SimpleGrid, Skeleton } from "@chakra-ui/react";

interface Props {
  height?: number;
  amount?: number;
}

function SkeletonGrid({ amount = 8, height = 200 }: Props) {
  return (
    <>
      {Array.from({ length: amount }).map((_, i) => (
        <Skeleton key={i} height={`${height}px`} />
      ))}
    </>
  );
}
export default SkeletonGrid;
