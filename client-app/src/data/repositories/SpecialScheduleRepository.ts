import { ISpecialScheduleRepository } from "./interfaces/ISpecialScheduleRepository";
import { ISpecialScheduleService } from "../services/interfaces/ISpecialScheduleService";
import { Result } from "@/data/result";
import { SpecialScheduleType } from "@/schemas/specialSchedule";
import { SpecialSchedule } from "@/types/specialSchedule";
import { DateOnly } from "@/types/date";

export class SpecialScheduleRepository implements ISpecialScheduleRepository {
  constructor(private readonly service: ISpecialScheduleService) {}

  async createSpecialSchedule(barberShpoId: number, data: SpecialScheduleType) {
    try {
      const res = await this.service.createSpecialSchedule(barberShpoId, data);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<boolean>(err as Error)
    }
  }

  async getSpecialSchedule(barberShpoId: number, date: DateOnly) {
    try {
      const res = await this.service.getSpecialSchedule(barberShpoId, date);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<SpecialSchedule>(err as Error)
    }
  }

  async getAllSpecialSchedules(barberShpoId: number) {
    try {
      const res = await this.service.getAllSpecialSchedules(barberShpoId);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<SpecialSchedule[]>(err as Error)
    }
  }

  async updateSpecialSchedule(barberShpoId: number, date: DateOnly, data: SpecialScheduleType) {
    try {
      const res = await this.service.updateSpecialSchedule(barberShpoId, date, data);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<boolean>(err as Error)
    }
  }

  async deleteSpecialSchedule(barberShpoId: number, date: DateOnly) {
    try {
      const res = await this.service.deleteSpecialSchedule(barberShpoId, date);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<boolean>(err as Error)
    }
  }
}
