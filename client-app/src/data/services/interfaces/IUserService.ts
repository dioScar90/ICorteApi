import { UserEmailUpdateType, UserPasswordUpdateType, UserPhoneNumberUpdateType } from "@/schemas/user"
import { AxiosResponse } from "axios";

export interface IUserService {
  getMe(): Promise<AxiosResponse<any>>;
  changeEmail(data: UserEmailUpdateType): Promise<AxiosResponse<any>>;
  changePassword(data: UserPasswordUpdateType): Promise<AxiosResponse<any>>;
  changePhoneNumber(data: UserPhoneNumberUpdateType): Promise<AxiosResponse<any>>;
  delete(): Promise<AxiosResponse<any>>;
}
