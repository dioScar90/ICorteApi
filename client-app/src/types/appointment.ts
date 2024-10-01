import { DateOnly, TimeOnly } from "./date"
import { Service } from "./service"

const paymentEnum = [
  'Card',
  'Cash',
  'Transfer',
  'Other',
] as const

type PaymentType = typeof paymentEnum[number]

const statusEnum = [
  'Pending',
  'Completed',
] as const

type AppointmentStatus = typeof statusEnum[number]

export type Appointment = {
  id: number
  clientId: number
  barberShopId: number
  date: DateOnly
  startTime: TimeOnly
  totalDuration: TimeOnly
  notes?: string
  paymentType: PaymentType
  totalPrice: number
  services: Service[]
  status: AppointmentStatus
}
