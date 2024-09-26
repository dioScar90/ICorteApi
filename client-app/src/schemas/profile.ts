import { z } from 'zod'
import { phoneNumberValidator } from './sharedValidators/phoneNumberValidator'

export const profileSchema = z.object({
  firstName: z.string({ required_error: 'Nome obrigatório' })
    .min(3, { message: 'Nome precisa ter pelo menos 3 caracteres' }),

  lastName: z.string({ required_error: 'Sobrenome obrigatório' })
    .min(3, { message: 'Sobrenome precisa ter pelo menos 3 caracteres' }),

  gender: z.enum(['Feminino', 'Masculino'], { message: 'Gênero inválido' }),

  phoneNumber: phoneNumberValidator(),
})

export type ProfileType = z.infer<typeof profileSchema>
