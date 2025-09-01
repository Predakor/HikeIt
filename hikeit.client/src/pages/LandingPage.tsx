import { PrimaryButton, SecondaryButton } from "@/components/ui/Buttons";
import PageTitle from "@/components/ui/Titles/PageTitle";
import SubTitle from "@/components/ui/Titles/SubTitle";
import {
  Card,
  Center,
  Heading,
  Icon,
  SimpleGrid,
  Stack,
  Text,
} from "@chakra-ui/react";
import { FaChartLine, FaHiking, FaMapMarkedAlt } from "react-icons/fa";
import { useNavigate } from "react-router";

function LandingPage() {
  const navigate = useNavigate();

  const goToLoginPage = () => navigate("/login");

  return (
    <Stack gap={16}>
      <PageTitle title={"Plan. Track. Conquer."} />

      <Text fontSize="xl">
        HikeIt helps you plan and analyze your hiking adventures with ease.
      </Text>

      <SimpleGrid columns={{ base: 1, md: 3 }} gap={16}>
        <Feature
          icon={FaMapMarkedAlt}
          title="Trip Management"
          text="Create, update, and track your hiking trips with GPX file support."
        />
        <Feature
          icon={FaChartLine}
          title="Analytics"
          text="Get insights on distance, elevation gain, time, and peaks reached."
        />
        <Feature
          icon={FaHiking}
          title="Achievements"
          text="Track your progress across regions and unlock hiking ranks."
        />
      </SimpleGrid>

      <Center>
        <Stack gap={4}>
          <Heading as="h2" size="xl">
            Ready to start your next adventure?
          </Heading>
          <Stack
            direction={{ base: "column", sm: "row" }}
            gap={4}
            justify="center"
          >
            <PrimaryButton onClick={goToLoginPage}>Get Started</PrimaryButton>
            <SecondaryButton onClick={goToLoginPage}>
              Try our demo
            </SecondaryButton>
          </Stack>
        </Stack>
      </Center>
    </Stack>
  );
}

function Feature({
  icon,
  title,
  text,
}: {
  icon: any;
  title: string;
  text: string;
}) {
  return (
    <Card.Root
      className="group"
      alignItems={"center"}
      justifyItems={"center"}
      padding={8}
    >
      <Icon as={icon} w={10} h={10} color={"blue.500"} />

      <Card.Header>
        <SubTitle fontSize={"2xl"} textAlign={"center"} title={title} />
      </Card.Header>

      <Card.Body textAlign={"center"}>
        <Text fontSize={"lg"}>{text}</Text>
      </Card.Body>
    </Card.Root>
  );
}
export default LandingPage;
