import { BarberShop } from "./barberShop"
import { Profile } from "./profile"

const roles = [
  'Guest',
  'Client',
  'BarberShop',
  'Admin',
] as const

export type UserRole = typeof roles[number]

export type UserMe = {
  id: number
  email: string
  phoneNumber: string
  roles: UserRole[]
  profile?: Profile
  barberShop?: BarberShop
}
