import { Profile } from "@/types/profile";
import { Result } from "../result";
import { IProfileRepository } from "./interfaces/IProfileRepository";
import { ProfileType } from "@/schemas/profile";
import { IProfileService } from "../services/interfaces/IProfileService";

export class ProfileRepository implements IProfileRepository {
  constructor(private readonly service: IProfileService) {}

  async createProfile(data: ProfileType) {
    try {
      const res = await this.service.createProfile(data)
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<boolean>(err as Error)
    }
  }

  async getProfileById(id: number) {
    try {
      const res = await this.service.getProfileById(id)
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<Profile>(err as Error)
    }
  }

  async updateProfile(id: number, data: ProfileType) {
    try {
      const res = await this.service.updateProfile(id, data)
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<boolean>(err as Error)
    }
  }

  async updateProfileImage(id: number, file: File) {
    try {
      const res = await this.service.updateProfileImage(id, file)
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<boolean>(err as Error)
    }
  }
}
