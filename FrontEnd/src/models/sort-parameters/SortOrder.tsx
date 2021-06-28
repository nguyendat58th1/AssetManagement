export enum SortOrder {
  Ascend,
  Descend,
}

export function ConvertToSortOrderEnum(input?: string): SortOrder {
  if (input === undefined) {
    return SortOrder.Ascend
  }

  switch (input.toLowerCase()) {
    case 'ascend':
      return SortOrder.Ascend
    case 'descend':
      return SortOrder.Descend
    default:
      throw new Error(`No enum of equivalent to ${input} is found`)
  }
}
