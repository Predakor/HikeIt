import type { UserType } from "@/types/User/user.types";

export default function IsAdminUser(user: UserType) {
  if (!user || user.roles.length === 0) {
    return false;
  }

  if (!user.roles.find((u) => u.toLowerCase() === "admin")) {
    return false;
  }

  return true;
}
