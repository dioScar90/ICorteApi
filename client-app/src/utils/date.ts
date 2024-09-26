import { DateStringType } from "@/types/date"

type GetTodayProps = {
  dateStr?: DateStringType,
  isDateIso?: boolean,
  isTimeIso?: boolean,
  isFullIso?: boolean,
  isString?: boolean,
  locale?: string,
}

function getNewDateObject(date?: DateStringType) {
  if (!date) {
    return new Date(new Date().setHours(12))
  }

  if (date.includes('T')) {
    return new Date(date)
  }

  return new Date(date + 'T12:00')
}

export function getToday({
    dateStr = undefined,
    isDateIso = undefined,
    isTimeIso = undefined,
    isFullIso = undefined,
    isString = undefined,
    locale = undefined
  }: GetTodayProps) {
  const date = getNewDateObject(dateStr)

  if (isTimeIso) {
    return date.toISOString().split('T')[1]
  }

  if (isDateIso) {
    return date.toISOString().split('T')[0]
  }

  if (isFullIso) {
    return date.toISOString()
  }

  if (isString) {
    return date.toLocaleDateString(locale ?? 'pt-BR')
  }

  return date
}