import { BarberShopType } from "@/schemas/barberShop";
import { BarberShop } from "@/types/barberShop";
import { AxiosResponse } from "axios";

export interface IBarberShopService {
  createBarberShop(data: BarberShopType): Promise<AxiosResponse<boolean>>;
  getBarberShop(id: number): Promise<AxiosResponse<BarberShop>>;
  updateBarberShop(id: number, data: BarberShopType): Promise<AxiosResponse<boolean>>;
  deleteBarberShop(id: number): Promise<AxiosResponse<boolean>>;
}
