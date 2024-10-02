import { TopBarberShop } from "@/types/barberShop";
import { Result } from "@/data/result";
import { IBarberScheduleRepository } from "./interfaces/IBarberScheduleRepository";
import { IBarberScheduleService } from "../services/interfaces/IBarberScheduleService";
import { DateOnly, TimeOnly } from "@/types/date";

export class BarberScheduleRepository implements IBarberScheduleRepository {
  constructor(private readonly service: IBarberScheduleService) {}

  async getAvailableDatesForBarber(barberShopId: number, dateOfWeek: DateOnly) {
    try {
      const res = await this.service.getAvailableDatesForBarber(barberShopId, dateOfWeek);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<DateOnly[]>(err as Error)
    }
  }

  async getAvailableSlots(barberShopId: number, date: DateOnly) {
    try {
      const res = await this.service.getAvailableSlots(barberShopId, date);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<TimeOnly[]>(err as Error)
    }
  }

  async getTopBarbersWithAvailability(dateOfWeek: DateOnly) {
    try {
      const res = await this.service.getTopBarbersWithAvailability(dateOfWeek);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<TopBarberShop[]>(err as Error)
    }
  }
}
