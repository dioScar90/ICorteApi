export const formatCurrency = (value, locale = 'pt-BR') => {
  return new Intl.NumberFormat(locale, {
      style: 'currency',
      currency: 'BRL'
  }).format(value)
}
