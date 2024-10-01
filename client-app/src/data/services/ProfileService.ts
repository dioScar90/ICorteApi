import { HttpClient } from "@/data/httpClient"
import { IProfileService } from "./interfaces/IProfileService"
import { ProfileType } from "@/schemas/profile"

const MODULE = '/profile'

export class ProfileService implements IProfileService {
  constructor(private readonly httpClient: HttpClient) {}

  async createProfile(data: ProfileType) {
    return await this.httpClient.post(MODULE, data)
  }

  async getProfileById(id: number) {
    return await this.httpClient.get(`${MODULE}/${id}`)
  }

  async updateProfile(id: number, data: ProfileType) {
    return await this.httpClient.put(`${MODULE}/${id}`, data)
  }

  async updateProfileImage(id: number, file: File) {
    const formData = new FormData()
    formData.append('file', file)
    
    return await this.httpClient.patch(`${MODULE}/${id}/image`, formData)
  }
}
