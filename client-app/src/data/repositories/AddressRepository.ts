import { IAddressRepository } from "./interfaces/IAddressRepository";
import { IAddressService } from "../services/interfaces/IAddressService";
import { Result } from "@/data/result";
import { AddressType } from "@/schemas/address";
import { Address } from "@/types/address";

export class AddressRepository implements IAddressRepository {
  constructor(private readonly service: IAddressService) {}

  async createAddress(barberShpoId: number, data: AddressType) {
    try {
      const res = await this.service.createAddress(barberShpoId, data);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<boolean>(err as Error)
    }
  }

  async getAddress(barberShpoId: number, id: number) {
    try {
      const res = await this.service.getAddress(barberShpoId, id);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<Address>(err as Error)
    }
  }

  async updateAddress(barberShpoId: number, id: number, data: AddressType) {
    try {
      const res = await this.service.updateAddress(barberShpoId, id, data);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<boolean>(err as Error)
    }
  }

  async deleteAddress(barberShpoId: number, id: number) {
    try {
      const res = await this.service.deleteAddress(barberShpoId, id);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<boolean>(err as Error)
    }
  }
}
