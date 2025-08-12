import type { UserType } from "@/components/User/User";

export default function IsAdminUser(user: UserType) {
  if (!user) {
    return false;
  }

  return user.roles.find((r) => r.toLocaleLowerCase() === "admin");
}
