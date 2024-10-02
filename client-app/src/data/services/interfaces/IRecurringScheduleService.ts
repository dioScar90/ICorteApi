import { RecurringScheduleType } from "@/schemas/recurringSchedule";
import { DayOfWeek } from "@/types/date";
import { RecurringSchedule } from "@/types/recurringSchedule";
import { AxiosResponse } from "axios";

export interface IRecurringScheduleService {
  createRecurringSchedule(barberShpoId: number, data: RecurringScheduleType): Promise<AxiosResponse<boolean>>;
  getRecurringSchedule(barberShpoId: number, dayOfWeek: DayOfWeek): Promise<AxiosResponse<RecurringSchedule>>;
  getAllRecurringSchedules(barberShpoId: number): Promise<AxiosResponse<RecurringSchedule[]>>;
  updateRecurringSchedule(barberShpoId: number, dayOfWeek: DayOfWeek, data: RecurringScheduleType): Promise<AxiosResponse<boolean>>;
  deleteRecurringSchedule(barberShpoId: number, dayOfWeek: DayOfWeek): Promise<AxiosResponse<boolean>>;
}
