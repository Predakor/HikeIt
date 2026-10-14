import { IconMountain } from "@/Icons/Icons";
import { Box, Heading, Stack, Text } from "@chakra-ui/react";
import HoverMarker from "../Markers/HoverMarker";
import type { PeakMarkerData } from "./usePeakMarkers";

export default function PeakMarker({ peak }: { peak: PeakMarkerData }) {
  return (
    <HoverMarker
      kind="peak"
      id={peak.id}
      longitude={peak.lon}
      latitude={peak.lat}
      popup={<PeakPopup peak={peak} />}
    >
      {(isHovered) => (
        <Box position="relative" cursor="pointer">
          <Box
            display="flex"
            alignItems="center"
            justifyContent="center"
            boxSize={8}
            rounded="full"
            bg={peak.reached ? "blue.500" : "gray.500"}
            color="white"
            borderWidth="2px"
            borderColor="white"
            boxShadow="md"
            opacity={peak.reached ? 1 : 0.85}
            transform={isHovered ? "scale(1.25)" : "scale(1)"}
            transition="transform 0.15s"
          >
            <IconMountain size={16} />
          </Box>
          {isHovered && <PeakLabel name={peak.name} />}
        </Box>
      )}
    </HoverMarker>
  );
}

// outside the scaled badge so the text keeps its size; pointer events off so it can't steal the hover
function PeakLabel({ name }: { name: string }) {
  return (
    <Text
      position="absolute"
      bottom="calc(100% + 6px)"
      left="50%"
      transform="translateX(-50%)"
      whiteSpace="nowrap"
      px={2}
      py={0.5}
      rounded="md"
      bg="blackAlpha.800"
      color="white"
      fontSize="xs"
      fontWeight="medium"
      pointerEvents="none"
    >
      {name}
    </Text>
  );
}

// popup sits on a white maplibre background, so text colors are explicit
function PeakPopup({ peak }: { peak: PeakMarkerData }) {
  return (
    <Stack gap={1} color="gray.900">
      <Heading size="sm">{peak.name}</Heading>
      <Text>{peak.height}m</Text>
      <Text color={peak.reached ? "green.700" : "gray.600"}>
        {peak.reached ? "Reached" : "Not reached"}
      </Text>
    </Stack>
  );
}
