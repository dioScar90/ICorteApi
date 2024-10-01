import { UserEmailUpdateType, UserLoginType, UserPasswordUpdateType, UserPhoneNumberUpdateType, UserRegisterType } from "@/schemas/user";
import { IUserRepository } from "./interfaces/IUserRepository";
import { UserMe } from "@/types/user";
import { Result } from "../result";
import { IUserService } from "../services/interfaces/IUserService";

export class UserRepository implements IUserRepository {
  constructor(private readonly userService: IUserService) {}

  async getMe() {
    try {
      const res = await this.userService.getMe();
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<UserMe>(err as Error)
    }
  }
  
  changeEmail(data: UserEmailUpdateType): Promise<Result<boolean>> {
    throw new Error("Method not implemented.");
  }
  changePassword(data: UserPasswordUpdateType): Promise<Result<boolean>> {
    throw new Error("Method not implemented.");
  }
  changePhoneNumber(data: UserPhoneNumberUpdateType): Promise<Result<boolean>> {
    throw new Error("Method not implemented.");
  }
  delete(): Promise<Result<boolean>> {
    throw new Error("Method not implemented.");
  }
}
