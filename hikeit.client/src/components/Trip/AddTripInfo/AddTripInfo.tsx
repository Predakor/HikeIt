import { IconDownload, IconExternal } from "@/Icons/Icons";
import api from "@/Utils/Api/apiRequest";
import DropFile from "@/components/AddFile/AddFile/DropFile";
import Divider from "@/components/Divider/Divider";
import { apiPath } from "@/data/apiPaths";
import {
  Button,
  Card,
  Field,
  Link,
  LinkOverlay,
  SimpleGrid,
} from "@chakra-ui/react";
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
const createRouteLink =
  'https://mapy.com/pl/zakladni?planovani-trasy&rc=9ih2.xaeREiNLxaPgM&rs=osm&rs=osm&ri=1194918506&ri=6622983&mrp=%7B"c"%3A132%2C"dt"%3A""%2C"d"%3Atrue%7D&xc=%5B%5D&rwp=1%3B9ihK1xadhCe.seqLgshdXkYBcrzh0meSlgyXegWIOeU9gYueCc&x=15.2748531&y=50.8276675&z=12';

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
          <Button colorPalette={"blue"} size={"2xl"} disabled>
            Manual Creation
          </Button>
          <Button colorPalette={"blue"} size={"2xl"}>
            <LinkOverlay asChild href={apiPath + "files/demo/trip"}>
              <Link unstyled />
            </LinkOverlay>
            Use preview file
            <IconDownload />
          </Button>
          <Button colorPalette={"green"} gridColumn={"1 / span 2"} size={"2xl"}>
            <LinkOverlay
              asChild
              href={createRouteLink}
              target="_blank"
              rel="noopener noreferrer"
            >
              <Link unstyled />
            </LinkOverlay>
            Generate route in external website
            <IconExternal />
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
