
import { Asset, AssetState, CreateAssetModel, EditAssetModel } from "../models/Asset";
import { AssetsPagedListResponse } from "../models/PagedListResponse";
import { PaginationParameters } from "../models/Pagination";
import { HttpClient } from "./HttpClient";
export class AssetService extends HttpClient {

  private static instance?: AssetService;

  private constructor() {
    super("https://localhost:5001");
  }

  public static getInstance = (): AssetService => {
    if (AssetService.instance === undefined) {
        AssetService.instance = new AssetService();
    }

    return AssetService.instance;
  }


  public getAllAssets = () => this.instance.get(`/api/Assets/getallasset/${JSON.parse(sessionStorage.getItem("id")!)}`);

  public getAssetsBySearch = (searchText : string) => this.instance.get(`/api/Assets/search/${JSON.parse(sessionStorage.getItem("id")!)}/${searchText}`);

  public getAsset = (id : number) => this.instance.get<Asset>(`/api/Assets/${id}`);
  public getAssets = (parameters?: PaginationParameters) => this.instance.get<AssetsPagedListResponse>(
    "/api/Assets",
    {
      params: {
        PageNumber: parameters?.PageNumber ?? 1,
        PageSize: parameters?.PageSize ?? 10
      }
    });

    public filterByCategory = (categoryId: number, parameters?: PaginationParameters) => {
      return this.instance.get<AssetsPagedListResponse>(`/api/Assets/category/${categoryId}`,
      {
        params: {
          PageNumber: parameters?.PageNumber ?? 1,
          PageSize: parameters?.PageSize ?? 10
        }
      })
    }
  
    public filterByState = (state: AssetState, parameters?: PaginationParameters) => {
      return this.instance.get<AssetsPagedListResponse>(`/api/Assets/state/${state.valueOf()}`,
      {
        params: {
          PageNumber: parameters?.PageNumber ?? 1,
          PageSize: parameters?.PageSize ?? 10
        }
      })
    }
  
    public searchAssets = (searchText: string, parameters?: PaginationParameters) => this.instance.get<AssetsPagedListResponse>(
      `/api/Assets/search`,
      {
        params: {
          query: searchText,
          PageNumber: parameters?.PageNumber ?? 1,
          PageSize: parameters?.PageSize ?? 10
        }
      })
  
  public create = (asset: CreateAssetModel) => this.instance.post<Asset>("/api/assets", asset);

  public update = (asset: EditAssetModel, id: number) => this.instance.put(`/api/assets/${id}`, asset);

  public delete = (id: number) => this.instance.delete(`/api/assets/${id}`);

}
