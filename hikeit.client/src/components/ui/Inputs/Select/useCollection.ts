import api from "@/Utils/Api/apiRequest";
import { createListCollection, type ListCollection } from "@chakra-ui/react";
import { useQuery } from "@tanstack/react-query";
import { useState, useEffect, useMemo } from "react";

export type CollectionType<T> = StaticCollection | AsyncCollection<T>;

export interface StaticCollection {
  type: "static";
  items: CollectionEntry[];
}

export interface AsyncCollection<TItem = any> {
  type: "async";
  url: string;
  mapper?: (item: TItem) => CollectionEntry;
}

export interface CollectionEntry {
  value: string | number;
  label: string;
}

function useCollection<T>(itemCollection: CollectionType<T>) {
  const asyncUrl = itemCollection.type === "async" ? itemCollection.url : "";
  const { data, status } = useQuery({
    queryKey: [asyncUrl],
    queryFn: () => api.get<any[]>(asyncUrl),
    staleTime: 1000 * 5,
    enabled: !!asyncUrl,
  });

  const collection = useMemo(() => {
    if (itemCollection.type === "static") {
      return mapToCollection(itemCollection.items);
    }

    if (itemCollection.type === "async") {
      if (status === "pending" || !data) {
        return mapToCollection([]);
      }

      return mapToCollection(
        itemCollection.mapper ? data.map(itemCollection.mapper) : (data as CollectionEntry[]),
      );
    }
  }, [itemCollection, data]);

  return [collection];
}

const mapToCollection = (items: CollectionEntry[]) => {
  return createListCollection({
    items: items,
  });
};

export default useCollection;
