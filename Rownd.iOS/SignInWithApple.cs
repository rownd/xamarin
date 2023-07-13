using System;
using System.Text;
using AuthenticationServices;
using Foundation;
using Newtonsoft.Json;
using Rownd.Xamarin.Utils;
using UIKit;

namespace Rownd.Xamarin.iOS
{
    public class AppleSignInData
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
    }

    public partial class AppleSignUpCoordinator : NSObject, IASAuthorizationControllerDelegate, IASAuthorizationControllerPresentationContextProviding
    {
        private SignInIntent intent;

        public void SignIn(SignInIntent intent)
        {
            this.intent = intent;

            // Create an object of the ASAuthorizationAppleIDProvider
            var appleIDProvider = new ASAuthorizationAppleIdProvider();

            // Create a request
            var request = appleIDProvider.CreateRequest();

            // Define the scope of the request
            request.RequestedScopes = new ASAuthorizationScope[] { ASAuthorizationScope.FullName, ASAuthorizationScope.Email };

            // Make the request
            var authorizationController = new ASAuthorizationController(new ASAuthorizationRequest[] { request })
            {
                // Assigning the delegates
                PresentationContextProvider = this,
                Delegate = this
            };

            authorizationController.PerformRequests();
        }

        public UIWindow GetPresentationAnchor(ASAuthorizationController controller)
        {
            var vc = UIApplication.SharedApplication.Windows[^1].RootViewController;
            return vc?.View.Window!;
        }

        private string GetFullName(string firstName, string lastName)
        {
            return $"{firstName ?? ""} {lastName ?? ""}";
        }

        #region IASAuthorizationController Delegate

        // If authorization is successful then this method will get triggered
        [Export("authorizationController:didCompleteWithAuthorization:")]
        public void DidComplete(ASAuthorizationController controller, ASAuthorization authorization)
        {
            // TODO: Send "completing" notification to Rownd shared lib
            // Rownd.requestSignIn(jsFnOptions: RowndSignInJsOptions(
            //        loginStep: .completing
            // ))

            if (authorization.GetCredential<ASAuthorizationAppleIdCredential>() is ASAuthorizationAppleIdCredential appleIdCredential)
            {

                // Create an account in your system.
                // let userIdentifier = appleIDCredential.user
                var fullName = appleIdCredential.FullName;
                var email = appleIdCredential.Email;
                var identityToken = appleIdCredential.IdentityToken;

                if (email != null)
                {
                    // Store email and fullName in AppleSignInData struct if available
                    var userAppleSignInData = new AppleSignInData
                    {
                        Email = email,
                        FirstName = fullName?.GivenName,
                        LastName = fullName?.FamilyName,
                        FullName = GetFullName(fullName?.GivenName, fullName?.FamilyName)
                    };

                    JsonConvert.SerializeObject(userAppleSignInData);

                    // TODO: Temporarily persist for later access
                    // var defaults = UserDefaults.standard;
                    // defaults.set(encoded, forKey: appleSignInDataKey)
                }

                if (identityToken != null && new NSString(identityToken, NSStringEncoding.ASCIIStringEncoding) is NSString urlContent)
                {
                    var idToken = urlContent.ToString();

                    // TODO: Pass to shared code for handling

                    //Task {
                    //    do
                    //    {
                    //        let tokenResponse = try await Auth.fetchToken(idToken: idToken, intent: intent);





                    //        Task {
                    //            @MainActor in
                    //    Rownd.requestSignIn(jsFnOptions: RowndSignInJsOptions(
                    //        loginStep: RowndSignInLoginStep.success,
                    //        intent: self.intent,
                    //        userType: tokenResponse?.userType
                    //    ));
                    //        }

                    //        // Prevent fast auth handshakes from feeling jarring to the user
                    //        try await Task.sleep(nanoseconds: UInt64(2 * Double(NSEC_PER_SEC)))





                    //DispatchQueue.main.async {
                    //            store.dispatch(store.state.auth.onReceiveAuthTokens(
                    //                AuthState(
                    //                    accessToken: tokenResponse?.accessToken,
                    //                    refreshToken: tokenResponse?.refreshToken
                    //                )
                    //            ))


                    //    store.dispatch(SetLastSignInMethod(payload: SignInMethodTypes.apple))


                    //    store.dispatch(Thunk<RowndState> {
                    //                dispatch, getState in
                    //        guard let state = getState() else { return }

                    //                var userData = state.user.data


                    //        let defaults = UserDefaults.standard
                    //        //use UserDefault values for Email and fullName if available
                    //        if let userAppleSignInData = defaults.object(forKey: appleSignInDataKey) as? Data {
                    //                    let decoder = JSONDecoder()
                    //            if let loadedAppleSignInData = try? decoder.decode(AppleSignInData.self, from: userAppleSignInData) {
                    //                        userData["email"] = AnyCodable.init(loadedAppleSignInData.email)
                    //                userData["first_name"] = AnyCodable.init(loadedAppleSignInData.firstName)
                    //                userData["last_name"] = AnyCodable.init(loadedAppleSignInData.lastName)
                    //                userData["full_name"] = AnyCodable.init(loadedAppleSignInData.fullName)
                    //            }
                    //                    } else
                    //                    {
                    //                        if let email = email {
                    //                            userData["email"] = AnyCodable.init(email)
                    //                                userData["first_name"] = AnyCodable.init(fullName?.givenName)
                    //                                userData["last_name"] = AnyCodable.init(fullName?.familyName)
                    //                                userData["full_name"] = AnyCodable.init(String("\(fullName?.givenName) \(fullName?.familyName)"))
                    //                            }
                    //                    }

                    //                    if (!userData.isEmpty)
                    //                    {
                    //                        DispatchQueue.main.async {
                    //                            dispatch(UserData.save(userData))
                    //                            }
                    //                    }
                    //                })
                    //}
                    //        } catch ApiError.generic(let errorInfo) {
                    //            if errorInfo.code == "E_SIGN_IN_USER_NOT_FOUND" {
                    //                Rownd.requestSignIn(jsFnOptions: RowndSignInJsOptions(
                    //                    token: idToken,
                    //                    loginStep: .noAccount,
                    //                    intent: .signIn
                    //                ))
                    //                }
                    //            else
                    //            {
                    //                DispatchQueue.main.async {
                    //                    Rownd.requestSignIn(jsFnOptions: RowndSignInJsOptions(
                    //                        loginStep: .error,
                    //                        signInType: .apple
                    //                    ))
                    //                    }
                    //            }
                    //        } catch
                    //        {
                    //            DispatchQueue.main.async {
                    //                Rownd.requestSignIn(jsFnOptions: RowndSignInJsOptions(
                    //                    loginStep: .error,
                    //                    signInType: .apple
                    //                ))
                    //                }
                    //        }
                    //        }
                }
                else
                {
                    // TODO: Show error hub state
                    //                logger.error("Missing data from Apple sign-in response: \(String(describing: appleIDCredential))")
                    //Rownd.requestSignIn(jsFnOptions: RowndSignInJsOptions(
                    //    loginStep: .error,
                    //    signInType: .apple
                    //))
                }
            }
            else if (authorization.GetCredential<ASAuthorizationPlatformPublicKeyCredentialRegistration>() is ASAuthorizationPlatformPublicKeyCredentialRegistration credentialRegistration)
            {
                var attestationObject = credentialRegistration.RawAttestationObject;
                var clientDataJSON = credentialRegistration.RawClientDataJson;
                var credentialID = credentialRegistration.CredentialId; // TODO: need this as base64url encoded

                // TODO: Handle registration API stuff in shared code
            }
            else if (authorization.GetCredential<ASAuthorizationPlatformPublicKeyCredentialAssertion>() is ASAuthorizationPlatformPublicKeyCredentialAssertion credentialAssertion)
            {
                var signature = credentialAssertion.Signature;
                var clientDataJSON = credentialAssertion.RawClientDataJson;
                var userId = credentialAssertion.UserId;
                var credentialID = credentialAssertion.CredentialId; // TODO: need this as base64url encoded
                var authenticatorData = credentialAssertion.RawAuthenticatorData;

                // TODO: Handle passkey authorization/authentication API stuff in shared code
            }
            else
            {
                // TODO: Show error. This is an unknown credential type
                // logger.error("Unknown credential type returned from Apple ID sign-in: \(String(describing: authorization.credential))")
                //        Rownd.requestSignIn(jsFnOptions: RowndSignInJsOptions(
                //    loginStep: .error,
                //    signInType: .apple
                // ))
            }
        }

        // Handle authorization failures
        [Export("authorizationController:didCompleteWithError:")]
        public void DidComplete(ASAuthorizationController controller, NSError error)
        {
            // If there is any error, we'll get it here
            //logger.error("An error occurred while signing in with Apple. Error: \(String(describing: error))")

            if (error == null)
            {
                return;
            }
            //    var authorizationError = error as ASAuthorizationError;
            //if (authorizationError != null)
            //    {
            //        Rownd.requestSignIn(jsFnOptions: RowndSignInJsOptions(
            //            loginStep: .error,
            //            signInType: .apple
            //        ))
            //    return
            //}

            //    switch (authorizationError.) {
            //        case .canceled:
            //            return
            //        default:
            //    Rownd.requestSignIn(jsFnOptions: RowndSignInJsOptions(
            //        loginStep: .error,
            //        signInType: .apple
            //    ))
            //                    }
        }

        #endregion
    }
}