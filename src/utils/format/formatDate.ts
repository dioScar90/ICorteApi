const getDateTypeValue = (value) => {
  if (value instanceof Date) {
    return value
  }

  if (typeof value !== 'string') {
    return null
  }

  if (value.includes('T')) {
    return new Date(value)
  }

  const dateStr = value + 'T00:00'
  return new Date(dateStr)
}

const sanitizeOptions = (options) => {
  const validAttributes = [
    'year', 'month', 'day', 'hour', 'minute', 'second', 'hour12', 'timeZone', 'timeZoneName', 'weekday', 'dateStyle', 'timeStyle'
  ]

  const sanitized = {}
  
  for (const key in options) {
    if (!validAttributes.includes(key)) {
      continue
    }

    sanitized[key] = options[key]
  }

  return sanitized
}

export const formatDate = (value, options = {}) => {
  const date = getDateTypeValue(value)

  if (!date) {
    return null
  }

  return new Intl.DateTimeFormat('pt-BR', { ...sanitizeOptions(options) }).format(date)
}

