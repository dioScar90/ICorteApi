export const getStorage = (key) => {
  const value = localStorage.getItem(key)
  return value ? JSON.parse(value) : null
}
