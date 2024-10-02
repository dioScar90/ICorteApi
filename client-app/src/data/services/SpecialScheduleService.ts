import { SpecialScheduleType } from "@/schemas/specialSchedule";
import { HttpClient } from "@/data/httpClient";
import { ISpecialScheduleService } from "./interfaces/ISpecialScheduleService";
import { DateOnly } from "@/types/date";

function getUrl(barberShpoId: number, date?: DateOnly) {
  const baseEndpoint = `/barber-shop/${barberShpoId}/special-schedule`

  if (date === undefined) {
    return baseEndpoint
  }
  
  return `${baseEndpoint}/${date}`
}

export class SpecialScheduleService implements ISpecialScheduleService {
  constructor(private readonly httpClient: HttpClient) {}
  
  async createSpecialSchedule(barberShpoId: number, data: SpecialScheduleType) {
    const url = getUrl(barberShpoId)
    return await this.httpClient.post(url, { ...data })
  }

  async getSpecialSchedule(barberShpoId: number, date: DateOnly) {
    const url = getUrl(barberShpoId, date)
    return await this.httpClient.get(url)
  }

  async getAllSpecialSchedules(barberShpoId: number) {
    const url = getUrl(barberShpoId)
    return await this.httpClient.get(url)
  }

  async updateSpecialSchedule(barberShpoId: number, date: DateOnly, data: SpecialScheduleType) {
    const url = getUrl(barberShpoId, date)
    return await this.httpClient.put(url, { ...data })
  }

  async deleteSpecialSchedule(barberShpoId: number, date: DateOnly) {
    const url = getUrl(barberShpoId, date)
    return await this.httpClient.delete(url)
  }
}
