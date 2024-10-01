import { TimeOnly, DateOnly } from "./date"

export type RecurringSchedule = {
  dayOfWeek: 0 | 1 | 2 | 3 | 4 | 5 | 6
  barberShopId: number
  openTime: TimeOnly
  closeTime: TimeOnly
  isActive: boolean
}
