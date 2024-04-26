const getKebabCase = (str: string) => str.split(/(?=[A-Z])/).map(txt => txt.toLowerCase()).join('-')

export const stringToDatasetFormat = (obj: object) => {
  return Object.entries(obj)
    .map(([key, value]) => 'data-' + getKebabCase(key) + '="' + value + '"')
    .join(' ')
}
