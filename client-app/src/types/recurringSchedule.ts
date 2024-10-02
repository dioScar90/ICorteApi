import { DayOfWeek, TimeOnly } from "./date"

export type RecurringSchedule = {
  dayOfWeek: DayOfWeek
  barberShopId: number
  openTime: TimeOnly
  closeTime: TimeOnly
  isActive: boolean
}
