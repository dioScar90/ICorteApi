import { Result } from "@/data/result";
import { BarberShopType } from "@/schemas/barberShop";
import { BarberShop } from "@/types/barberShop";

export interface IBarberShopRepository {
  createBarberShop(data: BarberShopType): Promise<Result<boolean>>;
  getBarberShop(id: number): Promise<Result<BarberShop>>;
  updateBarberShop(id: number, data: BarberShopType): Promise<Result<boolean>>;
  deleteBarberShop(id: number): Promise<Result<boolean>>;
}
