import { useAuth } from "@/hooks/Auth/useAuth";
import { DangerButton } from ".";

export function LogoutButton() {
  const { logout } = useAuth();

  return (
    <DangerButton
      onConfirm={logout}
      variant={"outline"}
      alertConfig={{
        confirmButtonText: "logout",
        warningDescription: "You are about to log out are you sure?",
      }}
    >
      Log out
    </DangerButton>
  );
}
