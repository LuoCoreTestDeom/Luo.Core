
/// <summary>
/// 个人信息
/// </summary>
 type JwtProfile=
{
    name:string
    auths:number
    expires:number
}
type JwtResponseDto=
{
    access:string
    Type:string
    profile:JwtProfile
   
}
export default JwtResponseDto