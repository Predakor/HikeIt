import api from "@/Utils/Api/apiRequest";
import type { UserProfile, UserType } from "@/types/User/user.types";
import { useMutation, useQueryClient } from "@tanstack/react-query";

function useUserMutations() {
  const queryClient = useQueryClient();
  const updateAvatar = useMutation({
    mutationKey: ["user", "avatar"],
    mutationFn: (file: File) => {
      const form = new FormData().append("file", file);

      return api.post<string>("users/me/data/avatar", form);
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

  return {
    updateAvatar,
  };
}
export default useUserMutations;
