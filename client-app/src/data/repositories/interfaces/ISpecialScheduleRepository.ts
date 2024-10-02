import { Result } from "@/data/result";
import { SpecialScheduleType } from "@/schemas/specialSchedule";
import { DateOnly } from "@/types/date";
import { SpecialSchedule } from "@/types/specialSchedule";

export interface ISpecialScheduleRepository {
  createSpecialSchedule(barberShpoId: number, data: SpecialScheduleType): Promise<Result<boolean>>;
  getSpecialSchedule(barberShpoId: number, date: DateOnly): Promise<Result<SpecialSchedule>>;
  getAllSpecialSchedules(barberShpoId: number): Promise<Result<SpecialSchedule[]>>;
  updateSpecialSchedule(barberShpoId: number, date: DateOnly, data: SpecialScheduleType): Promise<Result<boolean>>;
  deleteSpecialSchedule(barberShpoId: number, date: DateOnly): Promise<Result<boolean>>;
}
