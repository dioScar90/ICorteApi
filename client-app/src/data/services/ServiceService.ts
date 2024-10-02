import { ServiceType } from "@/schemas/service";
import { HttpClient } from "@/data/httpClient";
import { IServiceService } from "./interfaces/IServiceService";

function getUrl(barberShpoId: number, id?: number) {
  const baseEndpoint = `/barber-shop/${barberShpoId}/service`

  if (!id) {
    return baseEndpoint
  }
  
  return `${baseEndpoint}/${id}`
}

export class ServiceService implements IServiceService {
  constructor(private readonly httpClient: HttpClient) {}
  
  async createService(barberShpoId: number, data: ServiceType) {
    const url = getUrl(barberShpoId)
    return await this.httpClient.post(url, { ...data })
  }

  async getService(barberShpoId: number, id: number) {
    const url = getUrl(barberShpoId, id)
    return await this.httpClient.get(url)
  }

  async getAllServices(barberShpoId: number) {
    const url = getUrl(barberShpoId)
    return await this.httpClient.get(url)
  }

  async updateService(barberShpoId: number, id: number, data: ServiceType) {
    const url = getUrl(barberShpoId, id)
    return await this.httpClient.put(url, { ...data })
  }

  async deleteService(barberShpoId: number, id: number) {
    const url = getUrl(barberShpoId, id)
    return await this.httpClient.delete(url)
  }
}
