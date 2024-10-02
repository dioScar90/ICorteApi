import { BarberShop } from "@/types/barberShop";
import { Result } from "@/data/result";
import { IBarberShopRepository } from "./interfaces/IBarberShopRepository";
import { BarberShopType } from "@/schemas/barberShop";
import { IBarberShopService } from "../services/interfaces/IBarberShopService";

export class BarberShopRepository implements IBarberShopRepository {
  constructor(private readonly service: IBarberShopService) {}

  async createBarberShop(data: BarberShopType) {
    try {
      const res = await this.service.createBarberShop(data);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<boolean>(err as Error)
    }
  }

  async getBarberShop(id: number) {
    try {
      const res = await this.service.getBarberShop(id);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<BarberShop>(err as Error)
    }
  }

  async updateBarberShop(id: number, data: BarberShopType) {
    try {
      const res = await this.service.updateBarberShop(id, data);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<boolean>(err as Error)
    }
  }

  async deleteBarberShop(id: number) {
    try {
      const res = await this.service.deleteBarberShop(id);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<boolean>(err as Error)
    }
  }
}
