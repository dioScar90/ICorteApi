import { Result } from "@/data/result";
import { UserLoginType, UserRegisterType } from "@/schemas/user";

export interface IAuthRepository {
  register(data: UserRegisterType): Promise<Result<any>>;
  login(data: UserLoginType): Promise<Result<any>>;
  logout(): Promise<Result<any>>;
}
