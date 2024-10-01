import { ProfileType } from "@/schemas/profile";
import { Profile } from "@/types/profile";
import { AxiosResponse } from "axios";

export interface IProfileService {
  createProfile(data: ProfileType): Promise<AxiosResponse<boolean>>;
  getProfileById(id: number): Promise<AxiosResponse<Profile>>;
  updateProfile(id: number, data: ProfileType): Promise<AxiosResponse<boolean>>;
  updateProfileImage(id: number, file: File): Promise<AxiosResponse<boolean>>;
}
