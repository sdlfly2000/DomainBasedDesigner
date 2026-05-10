import { UserClaim } from "./UserClaim";

export interface UpdateUserClaimRequest {
  preClaim: UserClaim,
  newClaim: UserClaim,
  userId: string
}
