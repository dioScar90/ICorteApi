export const throttle = (func, delay) => {
  let lastExecutionTime = 0
  
  return function(...args) {
    const currentTime = Date.now()
    
    if (currentTime - lastExecutionTime >= delay) {
      func.apply(this, args)
      lastExecutionTime = currentTime
    }
  }
}