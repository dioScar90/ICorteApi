const stateEnum = [
  'AC', 'AL', 'AP', 'AM', 'BA', 'CE', 'DF', 'ES', 'GO', 'MA', 'MT', 'MS', 'MG', 'PA',
  'PB', 'PR', 'PE', 'PI', 'RJ', 'RN', 'RS', 'RO', 'RR', 'SC', 'SP', 'SE', 'TO',
] as const

type State = typeof stateEnum[number]

export type Address = {
  id: number
  barberShopId: number
  street: string
  number: string
  complement?: string
  neighborhood: string
  city: string
  state: State
  postalCode: string
  country: string
}
