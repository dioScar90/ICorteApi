// 123.456.789-09
const cpf = (value) => value
  .replace(/\D/g, '')
  .replace(/(\d{3})(\d)/, '$1.$2')
  .replace(/(\d{3})(\d)/, '$1.$2')
  .replace(/(\d{3}\.\d{3}\.\d{3})(\d)/, '$1-$2')
  .replace(/(-\d{2})\d+?$/, '$1')

// 12.345.678/001-09
const cnpj = (value) => value
  .replace(/\D/g, '')
  .replace(/(\d{2})(\d)/, '$1.$2')
  .replace(/(\d{3})(\d)/, '$1.$2')
  .replace(/(\d{3})(\d)/, '$1/$2')
  .replace(/(\d{2}\.\d{3}\.\d{3}\/\d{4})(\d)/, '$1-$2')
  .replace(/(-\d{2})\d+?$/, '$1')

// 123.45678.90-9
const pis = (value) => value
  .replace(/\D/g, '')
  .replace(/(\d{3})(\d)/, '$1.$2')
  .replace(/(\d{5})(\d)/, '$1.$2')
  .replace(/(\d{3}\.\d{5}\.\d{2})(\d)/, '$1-$2')
  .replace(/(-\d{1})\d+?$/, '$1')

// 19100-000
const cep = (value) => value
  .replace(/\D/g, '')
  .replace(/^(\d{5})(\d{3})+?$/, '$1-$2')
  .replace(/(-\d{3})(\d+?)/, '$1')

// (18) 91234-5678
const phone = (value) => value
  .replace(/[\D]/g, '')
  .replace(/(\d{2})(\d)/, '($1) $2')
  .replace(/(\d{5}|\d{4})(\d{4})/, '$1-$2')
  .replace(/(-\d{4})(\d+?)/, '$1')

const format = (type, value) => {
  if (!value || typeof value === 'object') {
    return ''
  }

  switch (type) {
    case 'phone':
      return phone(value)
    case 'cpf':
      return cpf(value)
    case 'cnpj':
      return cnpj(value)
    case 'pis':
      return pis(value)
    case 'cep':
      return cep(value)
    default:
      return ''
  }
}

const formatCpf       = (value) => format('cpf', value)
const formatCnpj      = (value) => format('cnpj', value)
const formatPis       = (value) => format('pis', value)
const formatCellPhone = (value) => format('phone', value)
const formatCep       = (value) => format('cep', value)

export {
  formatCpf,
  formatCnpj,
  formatPis,
  formatCellPhone,
  formatCep,
}
