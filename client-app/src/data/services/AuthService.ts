import { UserLoginType, UserRegisterType } from "@/schemas/user"
import { HttpClient, httpClient } from "../httpClient"
import { IAuthService } from "./interfaces/IAuthService"

function getUrl(final?: string) {
  const baseEndpoint = `/auth`
  
  if (!final) {
    return baseEndpoint
  }
  
  return `${baseEndpoint}/${final}`
}

export class AuthService implements IAuthService {
  constructor(private readonly httpClient: HttpClient) {}

  async register(data: UserRegisterType) {
    const url = getUrl('register')
    return await this.httpClient.post(url, { ...data })
  }

  async login(data: UserLoginType) {
    const url = getUrl('login')
    return await this.httpClient.post(url, { ...data })
  }

  async logout() {
    const url = getUrl('logout')
    return await this.httpClient.post(url)
  }
}
