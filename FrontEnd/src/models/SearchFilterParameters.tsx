import { UserType } from "./UserModel";

interface GenericSearchFilterParameters {
    searchQuery: string | "";
}

export interface UserSearchFilterParameters extends GenericSearchFilterParameters {
    type?: UserType;
}