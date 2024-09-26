import { z } from 'zod'

type RefineFuncProps = {
  openTime?: string,
  closeTime?: string,
  isClosed: boolean,
}

function closeTimeMustBeGreaterThenOpenTimeIfClosed({ openTime, closeTime, isClosed }: RefineFuncProps) {
  if (isClosed) {
    return true
  }

  if (!openTime && !closeTime) {
    return false
  }

  if (!closeTime) {
    return true
  }

  if (openTime && openTime > closeTime) {
    return true
  }

  return false
}

export const specialScheduleSchema = z.object({
  date: z.string({ required_error: 'Dia obrigatório' })
    .date('Dia inválido'),
  
  openTime: z.string({ required_error: 'Horário de abertura obrigatório' })
    .time('Horário de abertura inválido')
    .optional(),

  closeTime: z.string({ required_error: 'Horário de encerramento obrigatório' })
    .time('Horário de encerramento inválido')
    .optional(),

  isClosed: z.coerce.boolean(),
})
  .refine(closeTimeMustBeGreaterThenOpenTimeIfClosed, {
    message: 'Horário de encerramento precisa ser superior ao horário de abertura',
    path: ['closeTime']
  })

export type SpecialScheduleType = z.infer<typeof specialScheduleSchema>
