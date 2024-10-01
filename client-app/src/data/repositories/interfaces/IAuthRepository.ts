import { UserLoginType, UserRegisterType } from "@/schemas/user";

export interface IAuthRepository {
  register(data: UserRegisterType): Promise<any>;
  login(data: UserLoginType): Promise<any>;
  logout(): Promise<any>;
}
