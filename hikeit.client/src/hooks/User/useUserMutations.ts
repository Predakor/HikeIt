import api from "@/Utils/Api/apiRequest";
import type {
  UserPersonal,
  UserProfile,
  UserType,
} from "@/types/User/user.types";
import { useMutation, useQueryClient } from "@tanstack/react-query";

const requestBase = "users/me/data";

export type PersonalInfoUpdate = Partial<Omit<UserPersonal, "email">>;

function useUserMutations() {
  const queryClient = useQueryClient();

  const updateAvatar = useMutation({
    mutationKey: ["user", "avatar"],
    mutationFn: (file: File) => {
      const form = new FormData();
      form.append("file", file);
      return api.post<string>(`${requestBase}/avatar`, form);
    },
    onSuccess: (newAvatar) => {
      queryClient.setQueryData(["user"], (u: UserType) => {
        return {
          ...u,
          avatar: `${newAvatar}?v${Date.now()}`,
        };
      });
      queryClient.setQueryData(["user", "profile"], (u: UserProfile) => {
        return {
          ...u,
          summary: { ...u.summary, avatar: `${newAvatar}?v${Date.now()}` },
        };
      });
    },
  });

  const personalInfo = useMutation({
    mutationKey: ["user", "personal"],
    mutationFn: (data: PersonalInfoUpdate) =>
      api.patch(`${requestBase}/personal`, data),
    onSuccess: (_, request) => {
      queryClient.setQueryData(["user", "profile"], (profile: UserProfile) => {
        console.log({ request, profile });

        return {
          ...profile,
          personal: { ...profile.personal, ...request },
        };
      });
    },
  });

  return {
    updateAvatar,
    personalInfo,
  };
}
export default useUserMutations;
