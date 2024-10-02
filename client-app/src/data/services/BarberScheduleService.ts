import { DateOnly } from "@/types/date";
import { IBarberScheduleService } from "./interfaces/IBarberScheduleService";
import { HttpClient } from "../httpClient";

type Final = 'dates' | 'slots'

function getUrl(date: DateOnly, final?: Final, barberShopId?: number) {
  const baseEndpoint = `/barber-schedule`

  if (!final) {
    return `${baseEndpoint}/top-barbers/${date}`
  }

  return `${baseEndpoint}/${barberShopId!}/${final}/${date}`
}

export class BarberScheduleService implements IBarberScheduleService {
  constructor(private readonly httpClient: HttpClient) {}

  async getAvailableDatesForBarber(barberShopId: number, dateOfWeek: DateOnly) {
    const url = getUrl(dateOfWeek, 'dates', barberShopId)
    return await this.httpClient.get(url)
  }
  
  async getAvailableSlots(barberShopId: number, date: DateOnly) {
    const url = getUrl(date, 'slots', barberShopId)
    return await this.httpClient.get(url)
  }
  
  async getTopBarbersWithAvailability(dateOfWeek: DateOnly) {
    const url = getUrl(dateOfWeek)
    return await this.httpClient.get(url)
  }
  
}
