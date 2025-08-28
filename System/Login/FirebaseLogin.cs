using UnityEngine;
using Firebase;
using Firebase.Auth;
using System.Threading.Tasks;

public class FirebaseLogin : MonoBehaviour
{
    protected FirebaseAuth auth;
    protected FirebaseUser user;
    public bool allowAutoLogin = true;
    ForceResendingToken forceResendingToken;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual async void Start()
    {
        
        var status = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (status == DependencyStatus.Available)
        {
            auth = FirebaseAuth.DefaultInstance;
        }
        else
        {
            Debug.LogError("Firebase dependencies not available: " + status);
        }
        if (allowAutoLogin && IsLoggedIn())
        {
            user = auth.CurrentUser;
            OnLoginSuccess(user);
        }
    }


    // --- PHONE AUTH ---
    public virtual void StartPhoneVerification(string e164PhoneNumber, uint timeoutMs = 60000)
    {
        PhoneAuthProvider.GetInstance(auth).VerifyPhoneNumber(
            new PhoneAuthOptions
            {
                PhoneNumber = e164PhoneNumber,
                TimeoutInMilliseconds = timeoutMs,
                ForceResendingToken = forceResendingToken,
            },
            verificationCompleted: (Firebase.Auth.PhoneAuthCredential credential) => {
                OnPhoneVerificationCompleted(credential);
            },
            verificationFailed: (error) => {
                OnPhoneVerificationFailed(error);
            },
            codeSent: (verificationId, token) => {
                OnPhoneCodeSent(verificationId, token);
            },
            codeAutoRetrievalTimeOut: (verificationId) => {
                OnPhoneCodeAutoRetrievalTimeout(verificationId);
            });
    }

    public virtual async Task<FirebaseUser> SignInWithCredential(Credential credential)
    {
        var result = await auth.SignInWithCredentialAsync(credential);
        user = result;
        OnLoginSuccess(user);
        return result;
    }

    public virtual async Task<FirebaseUser> ConfirmPhoneCode(string verificationId, string smsCode)
    {
        var provider = PhoneAuthProvider.GetInstance(auth);
        var credential = provider.GetCredential(verificationId, smsCode);
        return await SignInWithCredential(credential);
    }

    // --- GOOGLE AUTH ---
    public virtual async Task<FirebaseUser> SignInWithGoogleIdToken(string googleIdToken, string googleAccessToken = null)
    {
        var credential = GoogleAuthProvider.GetCredential(googleIdToken, googleAccessToken);
        return await SignInWithCredential(credential);
    }

    // --- FACEBOOK AUTH ---
    public virtual async Task<FirebaseUser> SignInWithFacebookToken(string facebookAccessToken)
    {
        var credential = FacebookAuthProvider.GetCredential(facebookAccessToken);
        return await SignInWithCredential(credential);
    }

    // --- SESSION MANAGEMENT ---
    public bool IsLoggedIn()
    {
        return auth != null && auth.CurrentUser != null;
    }

    public virtual void Logout()
    {
        if (auth != null)
        {
            auth.SignOut();
            user = null;
        }
    }

    // --- LISTENER METHODS ---
    protected virtual async void OnLoginSuccess(FirebaseUser user) {
        string token = await user.TokenAsync(false);
        if (!ConfirmToken(token))
        {
          OnTokenCheckFailed();
          return;
        }
            
    }
    protected virtual bool ConfirmToken(string token)
    {
        // Note: Firebase does not provide a direct method to confirm token validity on the client side.
        // Token validation is typically done on the server side using Firebase Admin SDK.
        // Here, we can only check if the user is logged in and return true if so.
        return IsLoggedIn();
    }
    protected virtual void OnTokenCheckFailed() { }
    protected virtual void OnLoginFailed(string error) { }
    protected virtual void OnPhoneVerificationCompleted(Firebase.Auth.PhoneAuthCredential credential) { }
    protected virtual void OnPhoneVerificationFailed(string error) { }
    protected virtual void OnPhoneCodeSent(string verificationId, ForceResendingToken token) { }
    protected virtual void OnPhoneCodeAutoRetrievalTimeout(string verificationId) { }
}
