import { HttpClient } from "@/data/httpClient"
import { IProfileService } from "./interfaces/IProfileService"
import { ProfileType } from "@/schemas/profile"

function getUrl(id?: number, final?: string) {
  const baseEndpoint = `/profile`

  if (!id) {
    return baseEndpoint
  }

  if (!final) {
    return `${baseEndpoint}/${id}`
  }
  
  return `${baseEndpoint}/${id}/${final}`
}

export class ProfileService implements IProfileService {
  constructor(private readonly httpClient: HttpClient) {}

  async createProfile(data: ProfileType) {
    const url = getUrl()
    return await this.httpClient.post(url, data)
  }

  async getProfileById(id: number) {
    const url = getUrl(id)
    return await this.httpClient.get(url)
  }

  async updateProfile(id: number, data: ProfileType) {
    const url = getUrl(id)
    return await this.httpClient.put(url, data)
  }

  async updateProfileImage(id: number, file: File) {
    const formData = new FormData()
    formData.append('file', file)
    
    const url = getUrl(id, 'image')
    return await this.httpClient.patch(url, formData)
  }
}
