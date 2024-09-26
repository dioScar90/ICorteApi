import { z } from 'zod'

const MIN_RATING = 1
const MAX_RATING = 5

function isInRange(value: number) {
  return value >= MIN_RATING && value <= MAX_RATING
}

export const reportSchema = z.object({
  title: z.string({ required_error: 'Título obrigatório' })
    .min(3, { message: 'Título precisa ter pelo menos 3 caracteres' }),

  content: z.string()
    .min(3, { message: 'Comentário precisa ter pelo menos 3 caracteres' })
    .optional(),

  rating: z.coerce.number({ required_error: 'Nota obrigatória' })
    .int('Nota inválida')
    .refine(isInRange, { message: `Nota precisa estar entre ${MIN_RATING} e ${MAX_RATING}`})
})

export type ReportType = z.infer<typeof reportSchema>
