export const getFormValuesAfterSubmit = (form, files = false) => {
  if (!form || !(form instanceof HTMLFormElement)) {
    return null
  }

  const formData = new FormData(form)

  if (!files) {
    return Object.fromEntries(
      [...formData]
      .filter(([_, value]) => typeof value === 'string')
    )
  }
  
  const obj = {}

  for (const [key, value] of formData) {
    if (value instanceof File) {
      obj[key] ??= []
      obj[key].push(value)
    } else {
      obj[key] = value
    }
  }

  return obj
}