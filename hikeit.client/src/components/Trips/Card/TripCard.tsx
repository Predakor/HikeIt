import { toUkDate } from "@/Utils/Formatters/dateFormatter";
import useRegion from "@/hooks/useRegion";
import type { TripDto } from "@/types/ApiTypes/TripDtos";
import { Badge, Card, CardTitle, Flex, Span, Stack } from "@chakra-ui/react";
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
  const formatedDate = toUkDate(trip.tripDay as string);
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
          <Stack fontSize={"lg"}>
            <Card.Description fontSize={"md"}>
              {baseInfo.map((item) => (
                <Span
                  display={"flex"}
                  justifyItems={"start"}
                  gap={2}
                  key={item.label}
                >
                  {item.label}:
                  <Span fontSize={"lg"} color={"HighlightText"}>
                    {item.value}
                  </Span>
                </Span>
              ))}
            </Card.Description>
          </Stack>
        </Card.Body>

        <Card.Footer>
          {badges.length > 0 && (
            <Stack direction={"row"}>
              {badges.map(({ name, color }) => (
                <Badge size={"sm"} colorPalette={color} key={name}>
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
