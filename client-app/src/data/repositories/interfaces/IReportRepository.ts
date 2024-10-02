import { Result } from "@/data/result";
import { ReportType } from "@/schemas/report";
import { Report } from "@/types/report";

export interface IReportRepository {
  createReport(barberShpoId: number, data: ReportType): Promise<Result<boolean>>;
  getReport(barberShpoId: number, id: number): Promise<Result<Report>>;
  getAllReports(barberShpoId: number): Promise<Result<Report[]>>;
  updateReport(barberShpoId: number, id: number, data: ReportType): Promise<Result<boolean>>;
  deleteReport(barberShpoId: number, id: number): Promise<Result<boolean>>;
}
