import { SpecialScheduleType } from "@/schemas/specialSchedule";
import { DateOnly } from "@/types/date";
import { SpecialSchedule } from "@/types/specialSchedule";
import { AxiosResponse } from "axios";

export interface ISpecialScheduleService {
  createSpecialSchedule(barberShpoId: number, data: SpecialScheduleType): Promise<AxiosResponse<boolean>>;
  getSpecialSchedule(barberShpoId: number, date: DateOnly): Promise<AxiosResponse<SpecialSchedule>>;
  getAllSpecialSchedules(barberShpoId: number): Promise<AxiosResponse<SpecialSchedule[]>>;
  updateSpecialSchedule(barberShpoId: number, date: DateOnly, data: SpecialScheduleType): Promise<AxiosResponse<boolean>>;
  deleteSpecialSchedule(barberShpoId: number, date: DateOnly): Promise<AxiosResponse<boolean>>;
}
