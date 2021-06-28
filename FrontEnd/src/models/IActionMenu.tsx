import { ReactNode } from "react";


export interface IActionMenu {
  selectedRow: {
    id: number
  },
  updateEntityPath: ReactNode,
  service: { delete: Function, getAll: Function }
}
