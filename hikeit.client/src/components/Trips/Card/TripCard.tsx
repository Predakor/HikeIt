import { dateOnlyToString } from "@/Utils/Formatters/dateFormats";
import useRegion from "@/hooks/useRegion";
import type { TripDto } from "@/types/ApiTypes/TripDtos";
import { Badge, Card, CardTitle, Flex, Stack, Text } from "@chakra-ui/react";
import type { UtilityValues } from "node_modules/@chakra-ui/react/dist/types/styled-system/generated/prop-types.gen";
import { Link } from "react-router";
import { cardCommonStyles } from "./_commonStyles";

interface BadgeMeta {
  name: string;
  color?: UtilityValues["colorPalette"];
}

interface Props {
  data: TripDto;
}

function TripCard({ data }: Props) {
  const trip = data.base;
  const formatedDate = dateOnlyToString(trip.tripDay as string);
  const { data: region } = useRegion(data.regionId);

  const baseInfo = [
    { label: "Region", value: region?.name },
    { label: "Date", value: formatedDate },
  ] as const;

  const badges: BadgeMeta[] = [
    { name: "Analytics", color: "blue" },
    { name: "Graph", color: "green" },
  ].filter(() => Math.random() > 0.5) as BadgeMeta[];

  return (
    <Link to={`${data.id}`}>
      <Card.Root {...cardCommonStyles}>
        <Card.Header>
          <Flex justifyItems={"center"}>
            <CardTitle fontSize={"3xl"}>{data.base.name}</CardTitle>
          </Flex>
        </Card.Header>
        <Card.Body>
          <Card.Description>
            <Stack fontSize={"lg"}>
              {baseInfo.map((item) => (
                <Flex gap={2} key={item.label}>
                  <span>{item.label}:</span>
                  <Text color={"HighlightText"}>{item.value}</Text>
                </Flex>
              ))}
            </Stack>
          </Card.Description>
        </Card.Body>

        <Card.Footer>
          {badges.length > 0 && (
            <Stack direction={"row"}>
              {badges.map(({ name, color }) => (
                <Badge size={"sm"} colorPalette={color}>
                  {name}
                </Badge>
              ))}
            </Stack>
          )}
        </Card.Footer>
      </Card.Root>
    </Link>
  );
}

export default TripCard;
