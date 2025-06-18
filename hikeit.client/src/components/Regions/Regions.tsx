import type { Region } from "@/data/types";
import MapWrapper from "../Wrappers/Mapping";
import RegionCard from "./Card/RegionCard";

function Regions({ data }: { data: Region[] }) {
  return (
    <MapWrapper
      items={data}
      renderItem={(itemData) => (
        <RegionCard region={itemData} key={itemData.id} />
      )}
    />
  );
}
export default Regions;
