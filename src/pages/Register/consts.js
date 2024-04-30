import { z } from 'zod';

const usualLogin = z.object({
  type: z.literal('usual'),
  email: z.string().email(),
  password: z.string().min(8),
})

const googleLogin = z.object({
  type: z.literal('google'),
  email: z.undefined(),
  password: z.undefined(),
})

export const schema = usualLogin.or(googleLogin)
