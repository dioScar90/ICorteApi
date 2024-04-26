import { isOnlyZeros } from '../is/onlyZeros'

function getOnlyNumbers(str: string | number, maxLength: number) {
  return String(str)
    .replace(/\D/g, '')
    .replace(/^0+/g, '')
    .padStart(maxLength, '0')
    .substring(0, maxLength)
}

// 123.456.789-09
function cpf(value: string | number) {
  const numbers = getOnlyNumbers(value, 11)

  if (isOnlyZeros(numbers)) {
    return ''
  }

  return numbers.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4')
}

// 12.345.678/0001-09
function cnpj(value: string | number) {
  const numbers = getOnlyNumbers(value, 14)

  if (isOnlyZeros(numbers)) {
    return ''
  }

  return numbers.replace(/(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, '$1.$2.$3/$4-$5')
}

// 123.45678.90-9
function pis(value: string | number) {
  const numbers = getOnlyNumbers(value, 11)

  if (isOnlyZeros(numbers)) {
    return ''
  }

  return numbers.replace(/(\d{3})(\d{5})(\d{2})(\d{1})/, '$1.$2.$3-$4')
}

// 19100-000
function cep(value: string | number) {
  const numbers = getOnlyNumbers(value, 11)

  if (isOnlyZeros(numbers)) {
    return ''
  }

  return numbers.replace(/(\d{5})(\d{3})/, '$1-$2')
}

// (18) 91234-5678
function phone(value: string | number) {
  return String(value)
  .replace(/\D/g, '')
  .replace(/(\d{2})(\d)/, '($1) $2')
  .replace(/(\d{5}|\d{4})(\d{4})/, '$1-$2')
  .replace(/(-\d{4})(\d+?)/, '$1')
}

enum Type {
  PHONE,
  CPF,
  CNPJ,
  PIS,
  CEP,
}

function format(type: Type, value: unknown) {
  if (!value || (typeof value !== 'string' && typeof value !== 'number')) {
    return ''
  }
  
  switch (type) {
    case Type.PHONE:
      return phone(value)
    case Type.CPF:
      return cpf(value)
    case Type.CNPJ:
      return cnpj(value)
    case Type.PIS:
      return pis(value)
    case Type.CEP:
      return cep(value)
    default:
      return ''
  }
}

export const formatCpf       = (value: unknown) => format(Type.PHONE, value)
export const formatCnpj      = (value: unknown) => format(Type.CPF, value)
export const formatPis       = (value: unknown) => format(Type.CNPJ, value)
export const formatCellPhone = (value: unknown) => format(Type.PIS, value)
export const formatCep       = (value: unknown) => format(Type.CEP, value)
