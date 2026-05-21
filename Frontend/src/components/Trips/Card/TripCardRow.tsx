import { toUkDate } from "@/Utils/Formatters/dateFormatter";
import { formatter } from "@/components/User/Stats/Utils/formatter";
import type { OptionalTripDto } from "@/types/Api/TripDtos";
import { Card, CardTitle, Flex, Span, Stack } from "@chakra-ui/react";
import { Link } from "react-router";
import { cardCommonStyles } from "./_commonStyles";

interface Props {
  data: OptionalTripDto;
}

export default function TripCardRow({ data }: Props) {
  const { id, name, tripDay, region, duration, distance } = data;
  const formatedDate = toUkDate(tripDay);

  const baseInfo = [
    { label: "Region", value: region?.name },
    { label: "Date", value: formatedDate },
    { label: "Distance", value: `${formatter.toKm(distance)} km` },
    { label: "Duration", value: duration ? formatter.toDuration(duration) : "" },
  ].filter((item) => item.value);

  return (
    <Link to={`/trips/${id}`}>
      <Card.Root {...cardCommonStyles}>
        <Flex alignItems={"center"} justifyContent={"space-between"} gap={6} wrap={"wrap"}>
          <Card.Header paddingTop={0}>
            <CardTitle fontSize={"3xl"}>{name}</CardTitle>
          </Card.Header>

          <Card.Body>
            <Stack direction={"row"} fontSize={"lg"} gap={6} wrap={"wrap"}>
              {baseInfo.map((item) => (
                <Span display={"flex"} justifyItems={"start"} gap={2} key={item.label}>
                  {item.label}:
                  <Span fontSize={"lg"} color={"HighlightText"}>
                    {item.value}
                  </Span>
                </Span>
              ))}
            </Stack>
          </Card.Body>
        </Flex>
      </Card.Root>
    </Link>
  );
}
