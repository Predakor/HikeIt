export type UserRole = "user" | "moderator" | "admin" | "demo";

export interface UserType {
  userName: string;
  firstName: string;
  lastName: string;
  email: string;
  avatar: string;
  roles: UserRole[];
}

export interface UserBaseProfile {
  userName: string;
  avatar: string;
  rank: string;
  trips: number;
  peaks: number;
  traveled: number;
}

export interface UserProfile {
  summary: UserBaseProfile;

  personal: {
    firstName: string;
    lastName: string;
    email: string;
    birthDay: string;
    country: string;
    gender: string | null;
  };

  accountState: {
    role: string;
    createdAt: string;
    status: string;
  };
}
