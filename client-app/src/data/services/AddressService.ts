import { AddressType } from "@/schemas/address";
import { HttpClient } from "../httpClient";
import { IAddressService } from "./interfaces/IAddressService";

function getUrl(barberShpoId: number, id?: number) {
  const baseEndpoint = `/barber-shop/${barberShpoId}/address`

  if (!id) {
    return baseEndpoint
  }
  
  return `${baseEndpoint}/${id}`
}

export class AddressService implements IAddressService {
  constructor(private readonly httpClient: HttpClient) {}
  
  async createAddress(barberShpoId: number, data: AddressType) {
    const url = getUrl(barberShpoId)
    return await this.httpClient.post(url, { ...data })
  }

  async getAddress(barberShpoId: number, id: number) {
    const url = getUrl(barberShpoId, id)
    return await this.httpClient.get(url)
  }

  async updateAddress(barberShpoId: number, id: number, data: AddressType) {
    const url = getUrl(barberShpoId, id)
    return await this.httpClient.put(url, { ...data })
  }

  async deleteAddress(barberShpoId: number, id: number) {
    const url = getUrl(barberShpoId, id)
    return await this.httpClient.delete(url)
  }
}
