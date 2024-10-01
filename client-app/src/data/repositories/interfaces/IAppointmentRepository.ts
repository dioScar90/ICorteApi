import { Result } from "@/data/result";
import { AppointmentType } from "@/schemas/appointment";
import { Appointment } from "@/types/appointment";

export interface IAppointmentRepository {
  createAppointment(data: AppointmentType): Promise<Result<boolean>>;
  getAppointment(id: number): Promise<Result<Appointment>>;
  getAllAppointments(): Promise<Result<Appointment[]>>;
  updateAppointment(id: number, data: AppointmentType): Promise<Result<boolean>>;
  deleteAppointment(id: number): Promise<Result<boolean>>;
}
