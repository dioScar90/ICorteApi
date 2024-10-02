export type IndexesOf<T extends readonly any[]> = Exclude<Partial<T>["length"], T["length"]>
