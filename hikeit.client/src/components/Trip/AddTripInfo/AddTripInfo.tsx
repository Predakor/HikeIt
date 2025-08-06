import DropFile from "@/components/AddFile/AddFile/DropFile";
import Divider from "@/components/Divider/Divider";
import { Button, Card, Field, SimpleGrid } from "@chakra-ui/react";
import type { ReactElement } from "react";

const fileOptionDescription = `
    Great we will autocomplete the form from your file. 
    Just upload your file bellow
`;
const noFileOptionDescription = `
    No worries! If you don't have a GPX file, you can still:\n
    Describe your hike and submit it. 
    Generate a file in the external website
    Get a test file from us
`;

interface Props {
  fileChangeHandler: (file: File) => void;
}
function AddTripInfo({ fileChangeHandler }: Props) {
  return (
    <SimpleGrid
      gridTemplateColumns={{ base: "", lg: "1fr auto 1fr" }}
      gap={{ base: 4, lg: 24 }}
    >
      <OptionCard title={"Use Gpx File"} description={fileOptionDescription}>
        <Field.Root flexGrow={1}>
          <DropFile allowedFiles={[".gpx"]} onFileChange={fileChangeHandler} />
        </Field.Root>
      </OptionCard>
      <Divider />

      <OptionCard
        title={"Create Mannualy"}
        description={noFileOptionDescription}
      >
        <SimpleGrid templateColumns={"1fr 1fr"} gap={4}>
          <Button colorPalette={"blue"} size={"2xl"}>
            Manual Creation
          </Button>
          <Button colorPalette={"blue"} size={"2xl"}>
            Use preview file
          </Button>
          <Button colorPalette={"green"} gridColumn={"1 / span 2"} size={"2xl"}>
            Generate route in external website
          </Button>
        </SimpleGrid>
      </OptionCard>
    </SimpleGrid>
  );
}

interface OptionCardProps {
  title: string;
  description: string;
  children: ReactElement;
}

function OptionCard({ title, description, children }: OptionCardProps) {
  return (
    <Card.Root
      transition={"scale .5s"}
      _hover={{
        scale: "1.1",
      }}
    >
      <Card.Header alignItems={"center"}>
        <Card.Title fontSize={"4xl"} fontWeight={"bold"}>
          {title}
        </Card.Title>
      </Card.Header>
      <Card.Body>
        <Card.Description whiteSpace={"pre-line"} fontSize={"xl"}>
          {description}
        </Card.Description>
      </Card.Body>
      <Card.Footer display={"block"}>{children}</Card.Footer>
    </Card.Root>
  );
}

export default AddTripInfo;
