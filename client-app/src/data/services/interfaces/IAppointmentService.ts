import { AppointmentType } from "@/schemas/appointment";
import { Appointment } from "@/types/appointment";
import { AxiosResponse } from "axios";

export interface IAppointmentService {
  createAppointment(data: AppointmentType): Promise<AxiosResponse<boolean>>;
  getAppointment(id: number): Promise<AxiosResponse<Appointment>>;
  getAllAppointments(): Promise<AxiosResponse<Appointment[]>>;
  updateAppointment(id: number, data: AppointmentType): Promise<AxiosResponse<boolean>>;
  deleteAppointment(id: number): Promise<AxiosResponse<boolean>>;
}
