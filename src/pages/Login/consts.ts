import { z } from "zod";

export const schema = z.object({
  email: z.string().email(),
  password: z.string().min(8),
});

export type LoginType = z.infer<typeof schema>;
