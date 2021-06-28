import { AssetCategory } from "./AssetCategory"
import { Assignment } from "./Assignment"

export type AssetInfo = {

    assetName: string,

    categoryId: number,

    specification: string,

    installedDate: Date,

    state: number,

    location: number

}

export interface Asset {

    id:number,

    assetName: string,

    assetCode:string,

    categoryId: number,
    
    category: AssetCategory,

    specification: string,

    installedDate: Date,

    state: number,

    location: number,
    
    assignments: Assignment[]

}

export type CreateAssetModel = {

    assetName: string,

    categoryId: number,

    specification: string,

    installedDate: Date,

    state: number,

    location: number

}

export type EditAssetModel = {

    assetName: string,

    specification: string,

    installedDate: Date,

    state: number

}


export enum AssetState {

    AVAILABLE,

    NOT_AVAILABLE,
    
    ASSIGNED,

    WAITING_FOR_RECYCLING,

    RECYCLED
    
}

export enum Location {

    HANOI,

    HOCHIMINH

}
