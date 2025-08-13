import SubTitle from "@/components/Titles/SubTitle";
import { Dialog as ChakraDialog } from "@chakra-ui/react";

export function DialogTitle({ title }: { title?: string }) {
  return (
    <ChakraDialog.Title asChild>
      {title && <SubTitle title={title} />}
    </ChakraDialog.Title>
  );
}
