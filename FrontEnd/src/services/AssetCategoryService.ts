
import { AssetCategory, CreateCategoryModel } from "../models/AssetCategory";
import { HttpClient } from "./HttpClient";
export class AssetCategoryService extends HttpClient {

  private static instance?: AssetCategoryService;

  private constructor() {
    super("https://localhost:5001");
  }

  public static getInstance = (): AssetCategoryService => {
    if (AssetCategoryService.instance === undefined) {
        AssetCategoryService.instance = new AssetCategoryService();
    }

    return AssetCategoryService.instance;
  }

  public getAll = () => this.instance.get<AssetCategory[]>("/api/AssetCategories");
  
  public create = (category: CreateCategoryModel) => this.instance.post<AssetCategory>("/api/AssetCategories", category);

  public checkNameExist = (categoryName: string) => this.instance.get<boolean>(`/api/AssetCategories/checkName/${categoryName}`);

  public checkPrefixExist = (categoryCode: string) => this.instance.get<boolean>(`/api/AssetCategories/checkCode/${categoryCode}`);

}
