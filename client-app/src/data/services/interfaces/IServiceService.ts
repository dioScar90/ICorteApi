import { ServiceType } from "@/schemas/service";
import { Service } from "@/types/service";
import { AxiosResponse } from "axios";

export interface IServiceService {
  createService(barberShpoId: number, data: ServiceType): Promise<AxiosResponse<boolean>>;
  getService(barberShpoId: number, id: number): Promise<AxiosResponse<Service>>;
  getAllServices(barberShpoId: number): Promise<AxiosResponse<Service[]>>;
  updateService(barberShpoId: number, id: number, data: ServiceType): Promise<AxiosResponse<boolean>>;
  deleteService(barberShpoId: number, id: number): Promise<AxiosResponse<boolean>>;
}
