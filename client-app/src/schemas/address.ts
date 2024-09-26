import { z } from 'zod'

const StateEnum = [
  'AC', 'AL', 'AP', 'AM', 'BA', 'CE', 'DF', 'ES', 'GO', 'MA', 'MT', 'MS', 'MG', 'PA',
  'PB', 'PR', 'PE', 'PI', 'RJ', 'RN', 'RS', 'RO', 'RR', 'SC', 'SP', 'SE', 'TO',
] as const

export const addressSchema = z.object({
  street: z.string({ required_error: 'Logradouro obrigatório' })
    .min(3, { message: 'Logradouro precisa ter pelo menos 3 caracteres' }),

  number: z.string({ required_error: 'Número obrigatório' }),

  complement: z.string()
    .min(3, { message: 'Complemento precisa ter pelo menos 3 caracteres' })
    .optional(),

  neighborhood: z.string({ required_error: 'Bairro obrigatório' })
    .min(3, { message: 'Bairro precisa ter pelo menos 3 caracteres' }),

  city: z.string({ required_error: 'Cidade obrigatória' })
    .min(3, { message: 'Cidade precisa ter pelo menos 3 caracteres' }),

  state: z.enum(StateEnum, {
    required_error: 'Estado obrigatório',
    message: 'Estado inválido',
  }),

  postalCode: z.string({ required_error: 'CEP obrigatório' })
    .length(8, { message: 'CEP precisa ter 8 dígitos' }),

  country: z.string({ required_error: 'País obrigatório' })
    .min(3, { message: 'País precisa ter 8 caracteres' }),
})

export type AddressType = z.infer<typeof addressSchema>
