import { TimeOnly, DateOnly } from "./date"

export type SpecialSchedule = {
  date: DateOnly
  barberShopId: number
  notes?: string
  openTime?: TimeOnly
  closeTime?: TimeOnly
  isClosed: boolean
}
