import { ReportType } from "@/schemas/report";
import { HttpClient } from "@/data/httpClient";
import { IReportService } from "./interfaces/IReportService";

function getUrl(barberShpoId: number, id?: number) {
  const baseEndpoint = `/barber-shop/${barberShpoId}/report`

  if (!id) {
    return baseEndpoint
  }
  
  return `${baseEndpoint}/${id}`
}

export class ReportService implements IReportService {
  constructor(private readonly httpClient: HttpClient) {}
  
  async createReport(barberShpoId: number, data: ReportType) {
    const url = getUrl(barberShpoId)
    return await this.httpClient.post(url, { ...data })
  }

  async getReport(barberShpoId: number, id: number) {
    const url = getUrl(barberShpoId, id)
    return await this.httpClient.get(url)
  }

  async getAllReports(barberShpoId: number) {
    const url = getUrl(barberShpoId)
    return await this.httpClient.get(url)
  }

  async updateReport(barberShpoId: number, id: number, data: ReportType) {
    const url = getUrl(barberShpoId, id)
    return await this.httpClient.put(url, { ...data })
  }

  async deleteReport(barberShpoId: number, id: number) {
    const url = getUrl(barberShpoId, id)
    return await this.httpClient.delete(url)
  }
}
