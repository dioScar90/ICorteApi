export class Result<T> {
  #isSuccess: boolean
  #value?: T
  #error?: Error

  private constructor(value?: T, error?: Error) {
    this.#error = error
    this.#isSuccess = !this.#error
    this.#value = value
  }
  
  get isSuccess() {
    return this.#isSuccess
  }
  
  get value() {
    if (!this.#isSuccess) {
      throw this.#error!
    }

    return this.#value!
  }
  
  get error() {
    return this.#error
  }

  static Success<T>(value: T) {
    return new Result(value)
  }

  static Failure<T>(error: Error) {
    return new Result<T>(undefined, error)
  }
}
