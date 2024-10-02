import { Result } from "@/data/result";
import { ServiceType } from "@/schemas/service";
import { Service } from "@/types/service";

export interface IServiceRepository {
  createService(barberShpoId: number, data: ServiceType): Promise<Result<boolean>>;
  getService(barberShpoId: number, id: number): Promise<Result<Service>>;
  getAllServices(barberShpoId: number): Promise<Result<Service[]>>;
  updateService(barberShpoId: number, id: number, data: ServiceType): Promise<Result<boolean>>;
  deleteService(barberShpoId: number, id: number): Promise<Result<boolean>>;
}
