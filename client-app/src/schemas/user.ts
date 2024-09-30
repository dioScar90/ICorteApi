import { z } from 'zod'
import { passwordValidator } from './sharedValidators/passwordValidator'
import { emailValidator } from './sharedValidators/emailValidator'
import { phoneNumberValidator } from './sharedValidators/phoneNumberValidator'

export const userEmailUpdate = z.object({
  email: emailValidator(),
})

export type UserEmailUpdateType = z.infer<typeof userEmailUpdate>

export const userPhoneNumberUpdate = z.object({
  phoneNumber: phoneNumberValidator(),
})

export type UserPhoneNumberUpdateType = z.infer<typeof userPhoneNumberUpdate>

export const userPasswordUpdate = z.object({
  currentPassword: z.string(),
  newPassword: passwordValidator(),
})

export type UserPasswordUpdateType = z.infer<typeof userPasswordUpdate>

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
