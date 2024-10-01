import { UserEmailUpdateType, UserLoginType, UserPasswordUpdateType, UserPhoneNumberUpdateType, UserRegisterType } from "@/schemas/user"
import { UserMe } from "@/types/user"
import { IUserService } from "./interfaces/IUserService"
import { HttpClient } from "@/data/httpClient"

const MODULE = '/user'

export class UserService implements IUserService {
  constructor(private readonly httpClient: HttpClient) {}

  async getMe() {
    return await this.httpClient.get<UserMe>(`${MODULE}/me`)
  }

  // async findById(id) {
  //   return await this.httpClient.get(`${MODULE}/${id}`)
  // }

  async changeEmail(data: UserEmailUpdateType) {
    return await this.httpClient.patch(`${MODULE}/changeEmail`, { ...data })
  }

  async changePassword(data: UserPasswordUpdateType) {
    return await this.httpClient.patch(`${MODULE}/changePassword`, { ...data })
  }

  async changePhoneNumber(data: UserPhoneNumberUpdateType) {
    return await this.httpClient.patch(`${MODULE}/changePhoneNumber`,  { ...data })
  }

  async delete() {
    return await this.httpClient.delete(MODULE)
  }
}
