import { BarberShopType } from "@/schemas/barberShop";
import { HttpClient } from "../httpClient";
import { IBarberShopService } from "./interfaces/IBarberShopService";

function getUrl(id?: number) {
  const baseEndpoint = `/barber-shop`

  if (!id) {
    return baseEndpoint
  }
  
  return `${baseEndpoint}/${id}`
}

export class BarberShopService implements IBarberShopService {
  constructor(private readonly httpClient: HttpClient) {}

  async createBarberShop(data: BarberShopType) {
    const url = getUrl()
    return await this.httpClient.post(url, { ...data })
  }

  async getBarberShop(id: number) {
    const url = getUrl(id)
    return await this.httpClient.get(url)
  }

  async updateBarberShop(id: number, data: BarberShopType) {
    const url = getUrl(id)
    return await this.httpClient.put(url, { ...data })
  }

  async deleteBarberShop(id: number) {
    const url = getUrl(id)
    return await this.httpClient.delete(url)
  }
}
