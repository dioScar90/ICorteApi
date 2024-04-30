export const formatNumber = (value, locale = 'pt-BR') => {
  return new Intl.NumberFormat(locale).format(value)
}
