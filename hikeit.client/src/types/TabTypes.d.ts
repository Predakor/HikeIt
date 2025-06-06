export type TabEntry<TObject> = {
  key: keyof TObject;
  label: string;
  Icon?: IconType;
  Component: FunctionComponent<{ data: any }>;
};
