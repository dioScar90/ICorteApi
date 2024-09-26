import { z } from 'zod'
import { passwordValidator } from './sharedValidators/passwordValidator'
import { emailValidator } from './sharedValidators/emailValidator'
import { phoneNumberValidator } from './sharedValidators/phoneNumberValidator'

export const userRegisterSchema = z.object({
  email: emailValidator(),
  password: passwordValidator(),
  confirmPassword: z.string().min(1, { message: 'Confirmação de senha obrigatória' }),
})
  .refine(({ password, confirmPassword }) => password === confirmPassword, {
    message: 'Senhas devem ser iguais',
    path: ['confirmPassword']
  })

export type UserRegisterType = z.infer<typeof userRegisterSchema>

export const userLoginSchema = z.object({
  email: emailValidator(),
  password: z.string().min(1, { message: 'Senha obrigatória' }),
})

export type UserLoginType = z.infer<typeof userLoginSchema>

export const userPhoneNumberSchema = z.object({
  phoneNumber: phoneNumberValidator()
})

export type UserPhoneNumberType = z.infer<typeof userPhoneNumberSchema>
