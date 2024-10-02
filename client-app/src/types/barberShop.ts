import { Address } from "./address"
import { RecurringSchedule } from "./recurringSchedule"
import { Report } from "./report"
import { Service } from "./service"
import { SpecialSchedule } from "./specialSchedule"

export type BarberShop = {
  id: number
  ownerId: number
  name: string
  description?: string
  comercialNumber: string
  comercialEmail: string
  address?: Address
  recurringSchedule: RecurringSchedule[],
  specialSchedules: SpecialSchedule[],
  services: Service[],
  reports: Report[],
}

export type TopBarberShop = Pick<BarberShop, 'id' | 'name' | 'description'> & Pick<Report, 'rating'>
