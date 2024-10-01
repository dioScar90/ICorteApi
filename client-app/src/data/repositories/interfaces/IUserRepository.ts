import { Result } from "@/data/result";
import { UserEmailUpdateType, UserLoginType, UserPasswordUpdateType, UserPhoneNumberUpdateType, UserRegisterType } from "@/schemas/user";
import { UserMe } from "@/types/user";

export interface IUserRepository {
  getMe(): Promise<Result<UserMe>>;
  changeEmail(data: UserEmailUpdateType): Promise<Result<boolean>>;
  changePassword(data: UserPasswordUpdateType): Promise<Result<boolean>>;
  changePhoneNumber(data: UserPhoneNumberUpdateType): Promise<Result<boolean>>;
  delete(): Promise<Result<boolean>>;
}
