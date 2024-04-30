import { isOnlyZeros } from '../is/onlyZeros'

function getOnlyNumbers(str, maxLength) {
  return String(str)
    .replace(/\D/g, '')
    .replace(/^0+/g, '')
    .padStart(maxLength, '0')
    .substring(0, maxLength)
}

// 123.456.789-09
function cpf(value) {
  const numbers = getOnlyNumbers(value, 11)

  if (isOnlyZeros(numbers)) {
    return ''
  }

  return numbers.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4')
}

// 12.345.678/0001-09
function cnpj(value) {
  const numbers = getOnlyNumbers(value, 14)

  if (isOnlyZeros(numbers)) {
    return ''
  }

  return numbers.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, '$1.$2.$3/$4-$5')
}

// 123.45678.90-9
function pis(value) {
  const numbers = getOnlyNumbers(value, 11)

  if (isOnlyZeros(numbers)) {
    return ''
  }

  return numbers.replace(/(\d{3})(\d{5})(\d{2})(\d{1})/, '$1.$2.$3-$4')
}

// 19100-000
function cep(value) {
  const numbers = getOnlyNumbers(value, 11)

  if (isOnlyZeros(numbers)) {
    return ''
  }

  return numbers.replace(/(\d{5})(\d{3})/, '$1-$2')
}

// (18) 91234-5678
function phone(value) {
  return String(value)
  .replace(/\D/g, '')
  .replace(/(\d{2})(\d)/, '($1) $2')
  .replace(/(\d{5}|\d{4})(\d{4})/, '$1-$2')
  .replace(/(-\d{4})(\d+?)/, '$1')
}

function format(type, value) {
  if (!value || (typeof value !== 'string' && typeof value !== 'number')) {
    return ''
  }
  
  switch (type) {
    case 'PHONE':
      return phone(value)
    case 'CPF':
      return cpf(value)
    case 'CNPJ':
      return cnpj(value)
    case 'PIS':
      return pis(value)
    case 'CEP':
      return cep(value)
    default:
      return ''
  }
}

export const formatCpf       = (value) => format('PHONE', value)
export const formatCnpj      = (value) => format('CPF', value)
export const formatPis       = (value) => format('CNPJ', value)
export const formatCellPhone = (value) => format('PIS', value)
export const formatCep       = (value) => format('CEP', value)
