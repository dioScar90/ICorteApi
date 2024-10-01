import { Result } from "@/data/result";
import { ProfileType } from "@/schemas/profile";
import { Profile } from "@/types/user";

export interface IProfileRepository {
  createProfile(data: ProfileType): Promise<Result<boolean>>;
  getProfileById(id: number): Promise<Result<Profile>>;
  updateProfile(id: number, data: ProfileType): Promise<Result<boolean>>;
  updateProfileImage(id: number, file: File): Promise<Result<boolean>>;
}
