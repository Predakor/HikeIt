import { useAdminUser } from "@/hooks/Auth/useUser";
import type { ReactNode } from "react";
import FetchWrapper from "../Wrappers/Fetching";

interface Props {
  children: ReactNode;
}

function AdminPage(props: Props) {
  const getUser = useAdminUser();

  return (
    <FetchWrapper request={getUser}>
      {(user) => (user.role === "admin" ? props.children : "No acces bitch")}
    </FetchWrapper>
  );
}
export default AdminPage;
