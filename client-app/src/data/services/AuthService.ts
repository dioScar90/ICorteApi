import { UserLoginType, UserRegisterType } from "@/schemas/user"
import { httpClient } from "../httpClient"

function getUrl(final?: string) {
  const baseEndpoint = `/auth`
  
  if (!final) {
    return baseEndpoint
  }
  
  return `${baseEndpoint}/${final}`
}

export class AuthService {
  static async register(data: UserRegisterType) {
    const url = getUrl('register')
    return await httpClient.post(url, { ...data })
  }

  static async login(data: UserLoginType) {
    const url = getUrl('login')
    return await httpClient.post(url, { ...data })
  }

  static async logout() {
    const url = getUrl('logout')
    return await httpClient.post(url)
  }
}
