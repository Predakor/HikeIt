import { Button, Icon } from "@chakra-ui/react";
import { MdOutlineArrowBack } from "react-icons/md";
import { useNavigate } from "react-router";

export default function GoBackButton() {
  const navigation = useNavigate();

  const goBack = () => navigation(-1);

  return (
    <Button onClick={goBack} variant={"ghost"} aria-label="Go to previous page">
      <Icon as={MdOutlineArrowBack} boxSize={10} />
    </Button>
  );
}
