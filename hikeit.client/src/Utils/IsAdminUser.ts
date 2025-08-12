import type { UserType } from "@/components/User/User";

export default function IsAdminUser(user: UserType) {
  return user?.roles?.find((r) => r.toLocaleLowerCase() === "admin") ?? false;
}
