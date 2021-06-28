
export interface IDataTable<T> {
  columns: object[],
  updateEntityPath: string,
  service: {
    delete: Function,
    getAll: Function
  }
}
