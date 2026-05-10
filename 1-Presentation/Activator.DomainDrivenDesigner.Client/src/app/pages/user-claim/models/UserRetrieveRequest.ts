import { UserClaim } from "./UserClaim";

export interface UserRetrieveRequest {
  DisplayName: string,
  Claims: UserClaim[]
}
