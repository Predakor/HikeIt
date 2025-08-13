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
  rank: string;
  avatar: string;
  totalTrips: number;
  totalDistance: number;
  totalPeaks: number;
}
