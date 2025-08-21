import UserProfile from "@/components/User/Profile";
import FetchWrapper from "@/components/Wrappers/Fetching/FetchWrapper";
import useUserProfile from "@/hooks/User/useUserProfile";
import { Stack } from "@chakra-ui/react";

export default function UserProfilePage() {
  const getUser = useUserProfile();

  return (
    <FetchWrapper request={getUser}>
      {(user) => (
        <Stack>
          <UserProfile.PublicProfile user={user.summary} />

          <UserProfile.Personal data={user.personal} />

          <UserProfile.AccountDetails data={user.accountState} />
        </Stack>
      )}
    </FetchWrapper>
  );
}
