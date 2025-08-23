import { createListCollection, type ListCollection } from "@chakra-ui/react";
import { useState, useEffect } from "react";

export type CollectionType = StaticCollection | AsyncCollection;

export interface StaticCollection {
  type: "static";
  items: CollectionEntry[];
}

export interface AsyncCollection {
  type: "async";
  items: () => Promise<CollectionEntry[]>;
}

export interface CollectionEntry {
  value: string;
  label: string;
}

function useCollection(itemCollection: CollectionType) {
  const [collection, setCollection] =
    useState<ListCollection<CollectionEntry>>();

  useEffect(() => {
    const { type, items } = itemCollection;

    if (type === "static") {
      setCollection(mapToCollection(items));
    }
    if (type === "async") {
      items().then((data) => setCollection(mapToCollection(data)));
    }
  }, []);

  return [collection];
}

const mapToCollection = (items: CollectionEntry[]) => {
  return createListCollection({
    items: items,
  });
};

export default useCollection;
