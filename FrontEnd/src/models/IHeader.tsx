import { ReactNode } from "react";

export interface IHeader {
  addNewPath: string,
  service: { delete: Function, getAll: Function },
  selectedRowKeys: number[],
  hasSelected: ReactNode,
  handleSearch: ReactNode
}
