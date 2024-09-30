import { UserEmailUpdateType, UserLoginType, UserPasswordUpdateType, UserPhoneNumberUpdateType, UserRegisterType } from "@/schemas/user"
import { httpClient } from "./axios"
import { UserMe } from "@/types/user"

const MODULE = '/user'

/*
group.MapGet("me", GetMe)
    .RequireAuthorization(nameof(PolicyUserRole.FreeIfAuthenticated));

group.MapPatch("changeEmail", UpdateUserEmail)
    .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

group.MapPatch("changePassword", UpdateUserPassword)
    .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

group.MapPatch("changePhoneNumber", UpdateUserPhoneNumber)
    .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

group.MapDelete(INDEX, DeleteUser)
    .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));
*/

export class UserService {
  static async getMe() {
    return await httpClient.get<UserMe>(`${MODULE}/me`)
  }

  // static async findById(id) {
  //   return await httpClient.get(`${MODULE}/${id}`)
  // }

  static async changeEmail(data: UserEmailUpdateType) {
    return await httpClient.patch(`${MODULE}/changeEmail`, { ...data })
  }

  static async changePassword(data: UserPasswordUpdateType) {
    return await httpClient.patch(`${MODULE}/changePassword`, { ...data })
  }

  static async changePhoneNumber(data: UserPhoneNumberUpdateType) {
    return await httpClient.patch(`${MODULE}/changePhoneNumber`,  { ...data })
  }

  static async delete() {
    return await httpClient.delete(MODULE)
  }
}
