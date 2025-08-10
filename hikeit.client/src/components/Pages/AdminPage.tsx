import IsAdminUser from "@/Utils/IsAdminUser";
import { useAdminUser } from "@/hooks/Auth/useUser";
import type { ReactNode } from "react";
import FetchWrapper from "../Wrappers/Fetching/FetchWrapper";

interface Props {
  children: ReactNode | ReactNode[];
}

export default function AdminPage({ children }: Props) {
  const getUser = useAdminUser();

  return (
    <FetchWrapper request={getUser}>
      {(user) => (IsAdminUser(user) ? children : "No Acces")}
    </FetchWrapper>
  );
}
