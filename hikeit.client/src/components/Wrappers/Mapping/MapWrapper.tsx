import type { ReactNode } from "react";

interface RenderListProps<T> {
  items: T[];
  renderItem: (item: T, index: number) => ReactNode;
  fallback?: ReactNode;
  wrapper?: (children: ReactNode) => ReactNode;
}

function MapWrapper<T>({
  items,
  renderItem,
  fallback = null,
  wrapper,
}: RenderListProps<T>) {
  if (!items.length) return <>{fallback}</>;

  const content = items.map(renderItem);
  return <>{wrapper ? wrapper(content) : content}</>;
}

export default MapWrapper;
