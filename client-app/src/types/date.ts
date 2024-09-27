const zeroToThree = [
  '0', '1', '2', '3',
] as const

const zeroToFive = [
  '0', '1', '2', '3', '4', '5',
] as const

const zeroToNine = [
  '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
] as const

const oneToNine = [
  '1', '2', '3', '4', '5', '6', '7', '8', '9',
] as const

type Hour = `0${typeof zeroToNine[number]}` | `1${typeof zeroToNine[number]}` | `2${typeof zeroToThree[number]}`
type MinuteOrSecond = `${typeof zeroToFive[number]}${typeof zeroToNine[number]}`

type Year = `20${typeof zeroToNine[number]}${typeof zeroToNine[number]}`
type Month = `0${typeof oneToNine[number]}` | '10' | '11' | '12'
type Day = `0${typeof oneToNine[number]}` | `1${typeof zeroToNine[number]}` | `2${typeof zeroToNine[number]}` | '30' | '31'

export type TimeOnly = `${Hour}:${MinuteOrSecond}:${MinuteOrSecond}`
export type DateOnly = `${Year}-${Month}-${Day}`
// export type DateTime = `${DateOnly}T${TimeOnly}`
