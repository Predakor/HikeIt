import { preload } from "@/data/routes/routes";
import { useEffect } from "react";

export default function usePagePreload(path: string) {
  useEffect(() => {
    preload(path);
  }, [path]);
}
