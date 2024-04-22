const PREPOSITIONS =
  ['di', 'da', 'das', 'do', 'dos', 'de', 'von', 'van', 'le', 'la', 'du', 'des', 'del', 'della', 'der', 'al']

const isPrepositionAndNotFirst = (word, i) => i > 0 && (word.length === 1 || PREPOSITIONS.includes(word))

export const formatName = (str) => {
  if (!str || typeof str !== 'string') {
    return ''
  }
  
	return str.toLowerCase()
    .replace(/\s+/g, ' ')
    .trim()
    .split(' ')
    .map((word, i) => isPrepositionAndNotFirst(word, i) ? word : word[0].toUpperCase() + word.slice(1))
    .join(' ')
}
