import type { ReactNode } from "react";

export interface CardProps {
  title?: string;
  children: ReactNode;
  header?: ReactNode;
  footer?: ReactNode;
  headerCta?: ReactNode;
}
