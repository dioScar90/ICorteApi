export function getStorage<TValue>(key: string) {
  const value = localStorage.getItem(key)

  if (!value) {
    return null
  }
  
  try {
    return JSON.parse(value) as TValue
  } catch(err) {
    return null
  }
}

export function removeStorage(...keys: string[]) {
  keys.forEach(key => localStorage.removeItem(key))
}

export function setStorage<TValue>(key: string, value: TValue) {
  localStorage.setItem(key, JSON.stringify(value))
}
