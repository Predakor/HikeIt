import { IconPlus } from "@/Icons/Icons";
import AdminPage from "@/components/Pages/AdminPage";
import FetchWrapper from "@/components/Wrappers/Fetching/FetchWrapper";
import LinkCard from "@/components/ui/Cards/LinkCard";
import useRegions from "@/hooks/Regions/useRegions";
import type { Region } from "@/types/ApiTypes/region.types";
import { Button, For, Input, Separator, SimpleGrid } from "@chakra-ui/react";
import { useState } from "react";

const filters = {
  withValue: (region: Region, value: string) =>
    region.name.toLowerCase().includes(value.toLowerCase()),
  withId: (region: Region, id: number) => region.id === id,
};

export default function ManageRegionsAdminPage() {
  const getRegions = useRegions();
  const [filterValue, setFilterValue] = useState("");

  return (
    <AdminPage
      title="Manage Regions"
      header={
        <Button size={"lg"} colorPalette={"blue"}>
          Add Region <IconPlus />
        </Button>
      }
    >
      <Input
        type="search"
        onChange={(e) => setFilterValue(e.target.value)}
        variant={"subtle"}
        size={"lg"}
        placeholder="Search regions by name"
      />
      <Separator />
      <FetchWrapper request={getRegions}>
        {(regions) => (
          <ManageRegions
            filter={(region) => filters.withValue(region, filterValue)}
            regions={regions}
          />
        )}
      </FetchWrapper>
    </AdminPage>
  );
}

interface Props {
  filter: (region: Region) => boolean;
  regions: Region[];
}

function ManageRegions({ regions, filter }: Props) {
  const filteredRegions = regions.filter(filter);
  return (
    <SimpleGrid columns={{ base: 1, lg: 3 }} gap={4}>
      <For each={filteredRegions}>
        {(region) => (
          <LinkCard
            to={`${region.id}`}
            Header={region.id}
            Description={region.name}
          />
        )}
      </For>
    </SimpleGrid>
  );
}
