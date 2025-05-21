import { createListCollection } from "@chakra-ui/react";
import SelectWrapper from "./SelectWrapper";

function RegionSelect() {
  return (
    <SelectWrapper collection={regions}>
      {(region) => <div>{region.name}</div>}
    </SelectWrapper>
  );
}

const regions = createListCollection({
  items: [
    { id: 1, name: "Tatry" },
    { id: 2, name: "Pieniny" },
    { id: 3, name: "Beskid Śląski" },
    { id: 4, name: "Beskid Żywiecki" },
    { id: 5, name: "Beskid Mały" },
    { id: 6, name: "Beskid Makowski" },
    { id: 7, name: "Beskid Wyspowy" },
    { id: 8, name: "Gorce" },
    { id: 9, name: "Beskid Sądecki" },
    { id: 10, name: "Beskid Niski" },
    { id: 11, name: "Bieszczady" },
    { id: 12, name: "Góry Świętokrzyskie" },
    { id: 13, name: "Góry Sowie" },
    { id: 14, name: "Góry Stołowe" },
    { id: 15, name: "Góry Bystrzyckie" },
    { id: 16, name: "Góry Orlickie" },
    { id: 17, name: "Góry Bialskie" },
    { id: 18, name: "Góry Złote" },
    { id: 19, name: "Góry Opawskie" },
    { id: 20, name: "Góry Bardzkie" },
    { id: 21, name: "Masyw Śnieżnika" },
    { id: 22, name: "Karkonosze" },
    { id: 23, name: "Góry Izerskie" },
    { id: 24, name: "Rudawy Janowickie" },
    { id: 25, name: "Sudety Wałbrzyskie" },
    { id: 26, name: "string" },
  ],
  itemToString: (item) => item.name,
  itemToValue: (item) => item.id.toString(),
});

export default RegionSelect;
