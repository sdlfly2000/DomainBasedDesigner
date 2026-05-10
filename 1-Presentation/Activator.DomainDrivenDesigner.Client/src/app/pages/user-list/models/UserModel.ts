export interface UserModel {
  id: UserReference,  
  userName: string,
  displayName: string,
  claims: []
}
export interface UserReference {
  code: string,
  cacheFieldName: string,
  cacheCode: string
}
