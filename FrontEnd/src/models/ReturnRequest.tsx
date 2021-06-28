export interface ReturnRequest {
  id: number
  assetCode: string
  assetName: string
  assignedDate: string
  requestedByUser: string
  acceptedByUser: string | null
  returnedDate: string | null
  state: ReturnRequestState
}

export enum ReturnRequestState {
  WaitingForReturning,
  Completed,
}

export interface ReturnRequestFilterParameters {
    returnedDate?: string;
    state?: ReturnRequestState;
}

export interface CreateReturnRequestModel {
    assignmentId: number;
}