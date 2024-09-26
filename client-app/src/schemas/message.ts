import { z } from 'zod'

export const messageSchema = z.object({
  content: z.string()
    .min(1, { message: 'Mensagem não pode estar vazia' })
    .max(255, { message: 'Mensagem não pode ser maior que 255 caracteres' }),
})

export type MessageType = z.infer<typeof messageSchema>
