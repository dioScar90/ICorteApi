import { TimeOnly, DateOnly } from "./date"

const genders = ['Female', 'Male'] as const
type Gender = typeof genders[number]

const roles = [
  'Guest',
  'Client',
  'BarberShop',
  'Admin',
] as const

export type UserRole = typeof roles[number]

const stateEnum = [
  'AC', 'AL', 'AP', 'AM', 'BA', 'CE', 'DF', 'ES', 'GO', 'MA', 'MT', 'MS', 'MG', 'PA',
  'PB', 'PR', 'PE', 'PI', 'RJ', 'RN', 'RS', 'RO', 'RR', 'SC', 'SP', 'SE', 'TO',
] as const

type State = typeof stateEnum[number]

export type Address = {
  id: number
  barberShopId: number
  street: string
  number: string
  complement?: string
  neighborhood: string
  city: string
  state: State
  postalCode: string
  country: string
}

export type RecurringSchedule = {
  dayOfWeek: 0 | 1 | 2 | 3 | 4 | 5 | 6
  barberShopId: number
  openTime: TimeOnly
  closeTime: TimeOnly
  isActive: boolean
}

export type SpecialSchedule = {
  date: DateOnly
  barberShopId: number
  notes?: string
  openTime?: TimeOnly
  closeTime?: TimeOnly
  isClosed: boolean
}

export type Service = {
  id: number
  barberShopId: number
  name: string
  description?: string
  price: number
  duration: TimeOnly
}

export type Report = {
  id: number
  barberShopId: number
  title?: string,
  content?: string,
  rating: 1 | 2 | 3 | 4 | 5
}

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

export type Profile = {
  id: number
  firstName: string
  lastName: string
  fullName: string
  gender: Gender
  imageUrl?: string
}

export type User = {
  id: number
  email: string
  phoneNumber: string
  roles: UserRole[]
  profile?: Profile
  barberShop?: BarberShop
}
