import { SortOrder } from "./SortOrder"

export enum UserSortColumn {
  StaffCode,
  FullName,
  UserName,
  JoinedDate,
  Type,
}

export interface UserSortParameters {
  column? : UserSortColumn;
  order? : SortOrder;
}

export function ConvertToColumnEnum(input?: string) : UserSortColumn  {
  if (input === undefined) {
    throw new Error(`Undefined input`)
  }
  
  switch (input.toLowerCase()) {
    case "staffcode":
      return UserSortColumn.StaffCode
    case "fullname":
      return UserSortColumn.FullName
    case "username":
      return UserSortColumn.UserName
    case "joineddate":
      return UserSortColumn.JoinedDate
    case "type":
      return UserSortColumn.Type
    default:
      throw new Error(`No enum of equivalent to ${input} is found`)
  }
}