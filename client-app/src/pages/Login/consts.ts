import { z } from 'zod'

const cotainsUpperCase = (value: string) => /[A-Z]/.test(value)
const cotainsLowerCase = (value: string) => /[a-z]/.test(value)
const cotainsDigit = (value: string) => /[0-9]/.test(value)
const cotainsNonAlphanumeric = (value: string) => /[!@#$%^&*(),.?":{}|<>]/.test(value)

export const schema = z.object({
  email: z.string()
    .min(1, { message: "Email obrigatório" })
    .email("Formato de email inválido"),

  password: z.string({ message: 'Senha obrigatória' })
    .min(8, { message: 'Senha deve conter pelo menos 8 caracteres' })
    .refine(cotainsUpperCase, { message: 'Senha deve conter pelo menos uma letra maiúscula' })
    .refine(cotainsLowerCase, { message: 'Senha deve conter pelo menos uma letra minúscula' })
    .refine(cotainsDigit, { message: 'Senha deve conter pelo menos um dígito' })
    .refine(cotainsNonAlphanumeric, { message: 'Senha deve conter pelo menos um caractere especial' }),

  confirmPassword: z.string({ message: 'Senha obrigatória' })
    .min(8, { message: 'Senha deve conter pelo menos 8 caracteres' }),
})
.refine(({ password, confirmPassword }) => password === confirmPassword, {
  message: 'Senhas devem ser iguais',
  path: ['confirmPassword']
})
