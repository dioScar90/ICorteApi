import { UserRegisterType, UserLoginType } from "@/schemas/user";
import { IAuthRepository } from "./interfaces/IAuthRepository";
import { IAuthService } from "../services/interfaces/IAuthService";
import { Result } from "../result";

export class AuthRepository implements IAuthRepository {
  constructor(private readonly service: IAuthService) {}

  async register(data: UserRegisterType) {
    try {
      const res = await this.service.register(data);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<boolean>(err as Error)
    }
  }

  async login(data: UserLoginType) {
    try {
      const res = await this.service.login(data);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<boolean>(err as Error)
    }
  }
  
  async logout() {
    try {
      const res = await this.service.logout();
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<boolean>(err as Error)
    }
  }
}
