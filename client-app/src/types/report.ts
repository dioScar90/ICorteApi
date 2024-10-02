
export type Rating = 1 | 2 | 3 | 4 | 5

export type Report = {
  id: number
  barberShopId: number
  title?: string,
  content?: string,
  rating: Rating
}
