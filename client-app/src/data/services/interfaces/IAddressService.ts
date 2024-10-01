import { AddressType } from "@/schemas/address";
import { Address } from "@/types/address";
import { AxiosResponse } from "axios";

export interface IAddressService {
  createAddress(barberShpoId: number, data: AddressType): Promise<AxiosResponse<boolean>>;
  getAddress(barberShpoId: number, id: number): Promise<AxiosResponse<Address>>;
  updateAddress(barberShpoId: number, id: number, data: AddressType): Promise<AxiosResponse<boolean>>;
  deleteAddress(barberShpoId: number, id: number): Promise<AxiosResponse<boolean>>;
}
