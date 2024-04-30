const getKebabCase = (str) => str.split(/(?=[A-Z])/).map(txt => txt.toLowerCase()).join('-')

export const stringToDatasetFormat = (obj) => {
  return Object.entries(obj)
    .map(([key, value]) => 'data-' + getKebabCase(key) + '="' + value + '"')
    .join(' ')
}
