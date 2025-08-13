import SubTitle from "@/components/Titles/SubTitle";
import { Card } from "@chakra-ui/react";

function CardTitle({ title }: { title: string }) {
  return (
    <Card.Title asChild>
      <SubTitle title={title} />
    </Card.Title>
  );
}
export default CardTitle;
