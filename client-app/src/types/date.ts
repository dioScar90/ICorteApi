type BASE_RANGE_DAY =
  '01' | '02' | '03' | '04' | '05' | '06' | '07' | '08' | '09' | '10' |
  '11' | '12' | '13' | '14' | '15' | '16' | '17' | '18' | '19' | '20' |
  '21' | '22' | '23' | '24' | '25' | '26' | '27' | '28' | '29' | '30' |
  '31'

type BASE_RANGE_MONTH =
  '01' | '02' | '03' | '04' | '05' | '06' | '07' | '08' | '09' | '10' |
  '11' | '12'

type BASE_RANGE_YEAR = `${'19' | '20' | '21'}${number}${number}`

type TimeType = `${number}${number}:${number}${number}`
type DateType = `${BASE_RANGE_YEAR}-${BASE_RANGE_MONTH}-${BASE_RANGE_DAY}`;

export type DateStringType = DateType | `${DateType}T${TimeType}`;
