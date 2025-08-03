import { Button, Icon } from "@chakra-ui/react";
import { MdOutlineArrowBack } from "react-icons/md";
import { useNavigate } from "react-router";

export default function GoBackButton() {
  const navigation = useNavigate();

  const goBack = () => navigation(-1);

  return (
    <Button onClick={goBack} variant={"ghost"}>
      <Icon size={"2xl"}>
        <MdOutlineArrowBack />
      </Icon>
    </Button>
  );
}
