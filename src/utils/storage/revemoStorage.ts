export const removeStorage = (...keys) => {
  keys.forEach(key => localStorage.removeItem(key))
}
