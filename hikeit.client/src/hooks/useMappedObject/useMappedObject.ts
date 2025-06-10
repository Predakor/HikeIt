import { ObjectToArray } from "@/Utils/ObjectToArray";

export default function useMappedObject<T extends object>(data: T) {
  return ObjectToArray<T>(data);
}
