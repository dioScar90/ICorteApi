import { z } from 'zod'

const cotainsUpperCase = (value: string) => /[A-Z]/.test(value)
const cotainsLowerCase = (value: string) => /[a-z]/.test(value)
const cotainsDigit = (value: string) => /[0-9]/.test(value)
const cotainsNonAlphanumeric = (value: string) => /[!@#$%^&*(),.?":{}|<>]/.test(value)

export function passwordValidator(messageIdentifier?: string) {
  const Senha = messageIdentifier ?? 'Senha'

  return z.string()
    .min(1, { message: `${Senha} obrigatória` })
    .min(8, { message: `${Senha} deve conter pelo menos 8 caracteres` })
    .refine(cotainsUpperCase, { message: `${Senha} deve conter pelo menos uma letra maiúscula` })
    .refine(cotainsLowerCase, { message: `${Senha} deve conter pelo menos uma letra minúscula` })
    .refine(cotainsDigit, { message: `${Senha} deve conter pelo menos um dígito` })
    .refine(cotainsNonAlphanumeric, { message: `${Senha} deve conter pelo menos um caractere especial` })
}
