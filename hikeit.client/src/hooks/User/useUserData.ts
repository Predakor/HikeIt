import api from "@/Utils/Api/apiRequest";
import { cacheTimes } from "@/Utils/Api/staleTimes";
import type { UserBaseProfile } from "@/types/User/user.types";
import { useQuery } from "@tanstack/react-query";

const userBase: UserBaseProfile = {
  userName: "janusz",
  rank: "Novice hiker",
  avatar: "https://assets.puzzlefactory.com/puzzle/190/564/original.jpg",
  totalTrips: 1,
  totalDistance: 14008,
  totalPeaks: 2,
};

const accountState = {
  status: "active",
  role: "user",
  createdAt: new Date().toDateString(),
};

const userPersonal = {
  firstName: "janusz",
  lastName: "janusz",
  email: "mistrzbiznesu.wp.pl",
  birthDay: undefined,
  country: "poland",
  gender: "Male",
};

const __mockupUser__ = {
  base: userBase,
  personal: userPersonal,
  accountState,
};

export default function useUserData() {
  const getUseProfile = useQuery({
    initialData: __mockupUser__,
    queryKey: ["user", "profile"],
    queryFn: () => api.get<typeof __mockupUser__>("users/profile"),
    staleTime: cacheTimes.day,
  });

  return getUseProfile;
}
