import type { FunctionComponent, ReactNode } from "react";

interface RenderListProps<T> {
  items: T[];
  renderItem: (item: T, index: number) => ReactNode;
  fallback?: ReactNode;
  Wrapper?: FunctionComponent<{
    children: ReactNode;
  }>;
}

function MapWrapper<T>({
  items,
  renderItem,
  fallback = null,
  Wrapper,
}: RenderListProps<T>) {
  if (!items.length) return <>{fallback}</>;

  const content = items.map(renderItem);
  return <>{Wrapper ? <Wrapper>{content}</Wrapper> : content}</>;
}

export default MapWrapper;
