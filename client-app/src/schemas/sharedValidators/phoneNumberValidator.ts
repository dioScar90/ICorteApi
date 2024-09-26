import { z } from 'zod'

const isValidPhoneNumber = (value: string) => /^\d{2}9\d{8}$/.test(value)

export function phoneNumberValidator(messageIdentifier?: string) {
  const NumTelefone = messageIdentifier ?? "Número de telefone"

  return z.string()
    .min(1, { message: `${NumTelefone} obrigatório` })
    .refine(isValidPhoneNumber, { message: `${NumTelefone} precisa estar no formato (xx) 9xxxx-xxxx` })
}
