import { ReportType } from "@/schemas/report";
import { Report } from "@/types/report";
import { AxiosResponse } from "axios";

export interface IReportService {
  createReport(barberShpoId: number, data: ReportType): Promise<AxiosResponse<boolean>>;
  getReport(barberShpoId: number, id: number): Promise<AxiosResponse<Report>>;
  getAllReports(barberShpoId: number): Promise<AxiosResponse<Report[]>>;
  updateReport(barberShpoId: number, id: number, data: ReportType): Promise<AxiosResponse<boolean>>;
  deleteReport(barberShpoId: number, id: number): Promise<AxiosResponse<boolean>>;
}
