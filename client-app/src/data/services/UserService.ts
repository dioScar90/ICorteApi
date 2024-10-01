import { UserEmailUpdateType, UserPasswordUpdateType, UserPhoneNumberUpdateType } from "@/schemas/user"
import { UserMe } from "@/types/user"
import { IUserService } from "./interfaces/IUserService"
import { HttpClient } from "@/data/httpClient"

function getUrl(final?: string) {
  const baseEndpoint = `/user`
  
  if (!final) {
    return `${baseEndpoint}`
  }
  
  return `${baseEndpoint}/${final}`
}

export class UserService implements IUserService {
  constructor(private readonly httpClient: HttpClient) {}

  async getMe() {
    const url = getUrl('me')
    return await this.httpClient.get<UserMe>(url)
  }
  
  async changeEmail(data: UserEmailUpdateType) {
    const url = getUrl('changeEmail')
    return await this.httpClient.patch(url, { ...data })
  }

  async changePassword(data: UserPasswordUpdateType) {
    const url = getUrl('changePassword')
    return await this.httpClient.patch(url, { ...data })
  }

  async changePhoneNumber(data: UserPhoneNumberUpdateType) {
    const url = getUrl('changePhoneNumber')
    return await this.httpClient.patch(url,  { ...data })
  }

  async delete() {
    const url = getUrl()
    return await this.httpClient.delete(url)
  }
}
