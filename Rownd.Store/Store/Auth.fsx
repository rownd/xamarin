namespace Rownd.Store

#r "nuget: JWT, 10.0.2"
open JWT
open JWT.Builder


type AuthState =
    {
        AccessToken: string
        RefreshToken: string
    }
    member this.IsAuthenticated = this.AccessToken <> null
    member this.IsAccessTokenValid =
        let valParams = ValidationParameters.Default
        valParams.ValidateSignature <- false
        valParams.TimeMargin <- 60
        let builder: JwtBuilder = JwtBuilder.Create()
        let builder = builder.WithValidationParameters(valParams)
        
        builder.Decode this.AccessToken

        
        
