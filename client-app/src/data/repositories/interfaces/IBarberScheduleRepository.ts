import { Result } from "@/data/result";
import { TopBarberShop } from "@/types/barberShop";
import { DateOnly, TimeOnly } from "@/types/date";

export interface IBarberScheduleRepository {
  getAvailableDatesForBarber(barberShopId: number, dateOfWeek: DateOnly): Promise<Result<DateOnly[]>>;
  getAvailableSlots(barberShopId: number, date: DateOnly): Promise<Result<TimeOnly[]>>;
  getTopBarbersWithAvailability(dateOfWeek: DateOnly): Promise<Result<TopBarberShop[]>>;
}
