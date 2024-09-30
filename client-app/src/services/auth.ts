import { UserLoginType, UserRegisterType } from "@/schemas/user"
import { httpClient } from "./axios"

const MODULE = '/auth'

export class AuthService {
  static async register(data: UserRegisterType) {
    return await httpClient.post(`${MODULE}/register`, { ...data })
  }

  static async login(data: UserLoginType) {
    return await httpClient.post(`${MODULE}/login`, { ...data })
  }

  static async logout() {
    return await httpClient.post(`${MODULE}/login`)
  }
}
