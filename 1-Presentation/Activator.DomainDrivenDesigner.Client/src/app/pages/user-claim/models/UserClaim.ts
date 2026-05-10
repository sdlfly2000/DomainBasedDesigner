import { ClaimTypeValues } from "./ClaimTypeValues"

export interface UserClaim {
  claimId: string  
  claimType: ClaimTypeValues
  value: string
  isFixed: boolean
}
