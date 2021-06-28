
import { PaginationParameters, UsersPagedListResponse } from "../models/Pagination";
import { UserSearchFilterParameters } from "../models/SearchFilterParameters";
import { UserSortParameters } from "../models/sort-parameters/UserSortParameters";
import { User, UserInfo, EditUserModel, CreateUserModel } from "../models/UserModel";
import { HttpClient } from "./HttpClient";
export class UserService extends HttpClient {

  private static instance?: UserService;

  private constructor() {
    super("https://localhost:5001");
  }

  public static getInstance = (): UserService => {
    if (UserService.instance === undefined) {
      UserService.instance = new UserService();
    }

    return UserService.instance;
  }

  public create = (user: CreateUserModel) => this.instance.post<User>("/api/users", user);

  public getAllUsers = () => this.instance.get(`/api/Users/getalluser/${JSON.parse(sessionStorage.getItem("id")!)}`);

  public getAllNoCondition = () => this.instance.get("/api/Users/getAllNoCondition");

  public getUsersBySearch = (searchText: string) => this.instance.get(`/api/Users/search/${JSON.parse(sessionStorage.getItem("id")!)}/${searchText}`);

  public getUsers = (parameters?: PaginationParameters) => this.instance.get<UsersPagedListResponse>(
    "/api/Users",
    {
      params: {
        PageNumber: parameters?.PageNumber ?? 1,
        PageSize: parameters?.PageSize ?? 10
      }
    });

  public getUser = (id: number) => this.instance.get<UserInfo>(`/api/Users/${id}`);

  public searchAndFilter = (
    searchFilterParameters: UserSearchFilterParameters,
    paginationParameters?: PaginationParameters,
    sortParameters?: UserSortParameters,
  ) => this.instance.get<UsersPagedListResponse>(
    `api/Users/params`,
    {
      params: {
        SearchQuery: searchFilterParameters.searchQuery,
        Type: searchFilterParameters.type,
        SortCol: sortParameters?.column,
        Order: sortParameters?.order,
        PageNumber: paginationParameters?.PageNumber ?? 1,
        PageSize: paginationParameters?.PageSize ?? 10
      }
    }
  )

  public updateUser = (user: EditUserModel, id: number) => this.instance.put<EditUserModel>(`/api/Users/${id}`, user);

  public disableUser = (id: number) => this.instance.put(`/api/Users/${JSON.parse(sessionStorage.getItem("id")!)}/disable/${id}`);
}
