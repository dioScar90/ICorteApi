import { UserRegisterType, UserLoginType } from "@/schemas/user";
import { IAuthRepository } from "./interfaces/IAuthRepository";
import { IAuthService } from "../services/interfaces/IAuthService";

export class AuthRepository implements IAuthRepository {
  constructor(private readonly authService: IAuthService) {}

  async register(data: UserRegisterType) {
    return this.authService.register(data);
  }
  async login(data: UserLoginType) {
    return this.authService.login(data);
  }
  async logout() {
    return this.authService.logout();
  }
}
