import { arrayUtils } from "@/Utils/arrayUtils";
import type { GpxEntry } from "@/types/Api/gpx.types";
import { Box, Text } from "@chakra-ui/react";
import type { FeatureCollection, LineString } from "geojson";
import { TerrainMap } from "@/components/MapVisualisations/TerrainMap";
import { RLayer, RSource } from "maplibre-react-components";
import { memo, useMemo } from "react";
import type { RoutePreviewMapProps } from "./shared/model/bench.types";
import { SeriesLegend } from "./shared/components/SeriesLegend/SeriesLegend";

const baselineColor = "dimgray";
const previewColor = "blue";

function RoutePreviewMap({ baseline, preview, isExpanded = false }: RoutePreviewMapProps) {
  const baselineRoute = useMemo(() => toRouteCollection(baseline?.points), [baseline?.points]);
  const previewRoute = useMemo(() => toRouteCollection(preview?.points), [preview?.points]);

  const baselinePoints = baseline?.points ?? [];
  const previewPoints = preview?.points ?? [];
  const hasBaseline = baselinePoints.length > 0;
  const hasPreview = previewPoints.length > 0;

  if (!hasBaseline && !hasPreview) {
    return <Text color={"fg.muted"}>No data</Text>;
  }

  // Centre on the preview route when there is one, otherwise on the baseline.
  const centreRoute = hasPreview ? previewPoints : baselinePoints;
  const center = {
    Lon: arrayUtils.average(centreRoute, (p) => p.lon),
    Lat: arrayUtils.average(centreRoute, (p) => p.lat),
  };

  const legendItems = [
    ...(hasBaseline ? [{ label: "Baseline", color: baselineColor }] : []),
    ...(hasPreview ? [{ label: "Preview", color: previewColor }] : []),
  ];

  return (
    <Box position={"relative"}>
      <TerrainMap center={[center.Lon, center.Lat]} height={isExpanded ? "75vh" : "28rem"}>
        <RSource id="bench-baseline-route" type="geojson" data={baselineRoute} />
        <RLayer
          type="line"
          source="bench-baseline-route"
          id="bench-baseline-line"
          paint={{
            "line-color": baselineColor,
            "line-width": 7,
            "line-opacity": 0.55,
          }}
        />

        <RSource id="bench-preview-route" type="geojson" data={previewRoute} />
        <RLayer
          type="line"
          source="bench-preview-route"
          id="bench-preview-line"
          paint={{
            "line-color": previewColor,
            "line-width": 2,
            "line-opacity": 0.9,
          }}
        />
      </TerrainMap>

      <Box
        position={"absolute"}
        top={3}
        left={3}
        bg={"bg.panel"}
        borderRadius={"md"}
        px={3}
        py={2}
        boxShadow={"sm"}
      >
        <SeriesLegend items={legendItems} />
      </Box>
    </Box>
  );
}

// Memoised so editing a config (which re-renders the bench) does not re-render the map until its data changes.
export default memo(RoutePreviewMap);

const emptyRoute: FeatureCollection<LineString> = { type: "FeatureCollection", features: [] };

// Empty collection when there is no route, so the source keeps a stable id and only its data changes.
function toRouteCollection(points: GpxEntry[] | undefined): FeatureCollection<LineString> {
  if (!points || points.length === 0) {
    return emptyRoute;
  }

  return {
    type: "FeatureCollection",
    features: [
      {
        type: "Feature",
        properties: {},
        geometry: {
          type: "LineString",
          coordinates: points.map((point) => [point.lon, point.lat]),
        },
      },
    ],
  };
}
