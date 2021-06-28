import { ReturnRequest } from './ReturnRequest'
import { Assignment } from './Assignment'
import { User } from './UserModel'

export type PaginationParameters = {
  PageNumber: number;
  PageSize: number;
}

interface PagedListResponse<TModel> {
  totalCount: number
  pageSize: number
  currentPage: number
  totalPages: number
  hasNext: boolean
  hasPrevious: boolean
  items: TModel[]
}

export interface UsersPagedListResponse extends PagedListResponse<User> {}

export interface ReturnRequestPagedListResponse extends PagedListResponse<ReturnRequest> {}
export interface AssignmentPagedListResponse extends PagedListResponse<Assignment> {}
