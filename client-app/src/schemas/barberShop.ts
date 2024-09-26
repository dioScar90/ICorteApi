import { z } from 'zod'
import { phoneNumberValidator } from './sharedValidators/phoneNumberValidator'
import { emailValidator } from './sharedValidators/emailValidator'

export const barberShopSchema = z.object({
  name: z.string()
    .min(1, { message: 'Nome obrigatório' })
    .min(3, { message: 'Nome precisa ter pelo menos 3 caracteres' }),

  description: z.string()
    .min(3, { message: 'Descrição precisa ter pelo menos 3 caracteres' })
    .optional(),

  comercialNumber: phoneNumberValidator('Telefone comercial'),
  comercialEmail: emailValidator('Email comercial'),
})

export type BarberShopType = z.infer<typeof barberShopSchema>
