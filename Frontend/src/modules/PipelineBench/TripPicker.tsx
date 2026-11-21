import api from "@/Utils/Api/apiRequest";
import { Box, Button, Flex, Input, Show, Stack, Text } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";
import { useEffect, useState } from "react";

export interface SelectedTrip {
  id: string;
  name: string;
  owner: string;
}

interface TripPickerProps {
  selected: SelectedTrip | null;
  onSelect: (trip: SelectedTrip) => void;
  onClear: () => void;
}

const debounceMs = 300;

export function TripPicker({ selected, onSelect, onClear }: TripPickerProps) {
  if (selected) {
    return (
      <Flex align={"center"} gap={3} wrap={"wrap"}>
        <Text fontSize={"sm"}>
          <Text as={"span"} fontWeight={"medium"}>
            {selected.name}
          </Text>
          <Text as={"span"} color={"fg.muted"}>
            {` — ${selected.owner}`}
          </Text>
        </Text>
        <Button size={"sm"} variant={"outline"} onClick={onClear}>
          Change trip
        </Button>
      </Flex>
    );
  }

  return <TripSearch onSelect={onSelect} />;
}

function TripSearch({ onSelect }: { onSelect: (trip: SelectedTrip) => void }) {
  const [search, setSearch] = useState("");
  const [debouncedSearch, setDebouncedSearch] = useState(search);

  useEffect(() => {
    const timer = setTimeout(() => setDebouncedSearch(search), debounceMs);
    return () => clearTimeout(timer);
  }, [search]);

  const { data: trips, isFetching } = useQuery({
    queryKey: ["admin-trip-search", debouncedSearch],
    queryFn: () => api.get<SelectedTrip[]>("admin/trips", { search: debouncedSearch }),
  });

  const hasResults = trips !== undefined && trips.length > 0;
  const showEmpty = !isFetching && trips?.length === 0;
  const isDropdownOpen = hasResults || showEmpty || isFetching;

  return (
    <Box position={"relative"} w={{ base: "full", sm: "sm" }}>
      <Input
        placeholder={"Search trips by name…"}
        value={search}
        onChange={(event) => setSearch(event.target.value)}
      />

      <Show when={isDropdownOpen}>
        <Stack
          position={"absolute"}
          top={"calc(100% + 4px)"}
          insetStart={0}
          w={"full"}
          gap={1}
          p={2}
          bg={"bg.panel"}
          borderWidth={"1px"}
          borderRadius={"md"}
          boxShadow={"md"}
          zIndex={1}
        >
          <Show when={isFetching}>
            <Text fontSize={"sm"} color={"fg.muted"} px={1}>
              Searching…
            </Text>
          </Show>

          <Show when={showEmpty}>
            <Text fontSize={"sm"} color={"fg.muted"} px={1}>
              No trips found.
            </Text>
          </Show>

          {trips?.map((trip) => (
            <Button
              key={trip.id}
              variant={"ghost"}
              justifyContent={"flex-start"}
              h={"auto"}
              py={2}
              onClick={() => onSelect(trip)}
            >
              <Stack gap={0} align={"flex-start"}>
                <Text fontSize={"sm"} fontWeight={"medium"}>
                  {trip.name}
                </Text>
                <Text fontSize={"xs"} color={"fg.muted"}>
                  {trip.owner}
                </Text>
              </Stack>
            </Button>
          ))}
        </Stack>
      </Show>
    </Box>
  );
}
