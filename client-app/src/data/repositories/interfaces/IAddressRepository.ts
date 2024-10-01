import { Result } from "@/data/result";
import { AddressType } from "@/schemas/address";
import { Address } from "@/types/address";

export interface IAddressRepository {
  createAddress(barberShpoId: number, data: AddressType): Promise<Result<boolean>>;
  getAddress(barberShpoId: number, id: number): Promise<Result<Address>>;
  updateAddress(barberShpoId: number, id: number, data: AddressType): Promise<Result<boolean>>;
  deleteAddress(barberShpoId: number, id: number): Promise<Result<boolean>>;
}
