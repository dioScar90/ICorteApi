import { UserLoginType, UserRegisterType } from "@/schemas/user";
import { AxiosResponse } from "axios";

export interface IAuthService {
  register(data: UserRegisterType): Promise<AxiosResponse<any>>;
  login(data: UserLoginType): Promise<AxiosResponse<any>>;
  logout(): Promise<AxiosResponse<any>>;
}
