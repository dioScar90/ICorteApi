import { IServiceRepository } from "./interfaces/IServiceRepository";
import { IServiceService } from "../services/interfaces/IServiceService";
import { Result } from "@/data/result";
import { ServiceType } from "@/schemas/service";
import { Service } from "@/types/service";

export class ServiceRepository implements IServiceRepository {
  constructor(private readonly service: IServiceService) {}

  async createService(barberShpoId: number, data: ServiceType) {
    try {
      const res = await this.service.createService(barberShpoId, data);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<boolean>(err as Error)
    }
  }

  async getService(barberShpoId: number, id: number) {
    try {
      const res = await this.service.getService(barberShpoId, id);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<Service>(err as Error)
    }
  }

  async getAllServices(barberShpoId: number) {
    try {
      const res = await this.service.getAllServices(barberShpoId);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<Service[]>(err as Error)
    }
  }

  async updateService(barberShpoId: number, id: number, data: ServiceType) {
    try {
      const res = await this.service.updateService(barberShpoId, id, data);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<boolean>(err as Error)
    }
  }

  async deleteService(barberShpoId: number, id: number) {
    try {
      const res = await this.service.deleteService(barberShpoId, id);
      return Result.Success(res.data)
    } catch (err) {
      return Result.Failure<boolean>(err as Error)
    }
  }
}
