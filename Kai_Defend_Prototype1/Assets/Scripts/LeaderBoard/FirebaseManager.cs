using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using System.Threading.Tasks;
using Firebase.Extensions;
using Manager;
using TMPro.Examples;
using UnityEngine.Networking;
using UnityEngine.SocialPlatforms.Impl;
using UserProfile = Firebase.Auth.UserProfile;

public class FirebaseManager : MonoBehaviour
{
    FirebaseAuth auth;
    FirebaseUser user;
    private DatabaseReference databaseReference;

    public GameObject LoginPanel;

    public GameObject SignUpPanel;
    
    public GameObject ForgetPasswordPanel;

    public GameObject MenuPanel;

    public Toggle RememberMe;
    // public GameObject ProfilePanel;

    public TMP_InputField LoginEmail;
    public TMP_InputField LoginPassword;
    public TMP_InputField SignUpEmail;
    public TMP_InputField SignUpPassward;
    public TMP_InputField SignUpConfirm;
    public TMP_InputField SignUpUserName;
    public TMP_InputField ForgetPasswordEmail;
    
    [Header("Button")]
    public Button PlayAsGuestBuuton;
    [SerializeField] private Button logoutButton;
    [SerializeField] private Button connectIdButton;
    public Button RegisterButton;
    public Button RegisterTransferButton;
    public Button BackButton;
    public Button BackTransferButton;
    public Button DeleteAccountButton;

    [Header("Player Info")]
    public TextMeshProUGUI PlayerName;
    public TextMeshProUGUI PlayerTopWave;
    public TextMeshProUGUI PlayerTopDamage;
    public TextMeshProUGUI PlayerTopTime;
    public RawImage PlayerImage;

    [Header("Notification Info")]
    public TextMeshProUGUI NotificationTextTitel;
    public TextMeshProUGUI NotificationTextMessage;
    public GameObject NotificationFeild;

    [Header("Upload Photo")] 
    public TMP_InputField Input_Url;
    public string URL_DefaultImange = "https://cdn.icon-icons.com/icons2/2506/PNG/512/user_icon_150670.png";

    private bool isSignIn;
    private bool isSigned;

    private string userNameTemp;


    [Header("Leader Board")] 
    [SerializeField] private TextMeshProUGUI rankPlayer;
    [SerializeField] private RectTransform rankPanel;
    [SerializeField] private GameObject rankDataPrefab;
    [SerializeField] private RawImage photoLeaderBoard;
    [SerializeField] private TextMeshProUGUI waveLeaderBoard;

    // public string uid;

    public void ClearUserData()
    {
        //TODO  LATER
    }
    
    //DataBase

    #region Panel Controller
    
    public void OpenLoginPanel()
    {
        LoginPanel.gameObject.SetActive(true);
        SignUpPanel.gameObject.SetActive(false);
        ForgetPasswordPanel.gameObject.SetActive(false);
        MenuPanel.gameObject.SetActive(false);
        PlayAsGuestBuuton.gameObject.SetActive(true);
    }

    public void OpenSignUpPanel()
    {
        if (user!=null && user.IsAnonymous)
        {
            LoginPanel.gameObject.SetActive(false);
            SignUpPanel.gameObject.SetActive(true);
            ForgetPasswordPanel.gameObject.SetActive(false);
            MenuPanel.gameObject.SetActive(false);
            PlayAsGuestBuuton.gameObject.SetActive(false);

            BackTransferButton.gameObject.SetActive(true);
            RegisterTransferButton.gameObject.SetActive(true);
            BackButton.gameObject.SetActive(false);
            RegisterButton.gameObject.SetActive(false);
            return;
        }
        
        LoginPanel.gameObject.SetActive(false);
        SignUpPanel.gameObject.SetActive(true);
        ForgetPasswordPanel.gameObject.SetActive(false);
        MenuPanel.gameObject.SetActive(false);
        PlayAsGuestBuuton.gameObject.SetActive(false);
        
        BackTransferButton.gameObject.SetActive(false);
        RegisterTransferButton.gameObject.SetActive(false);
        BackButton.gameObject.SetActive(true);
        RegisterButton.gameObject.SetActive(true);
    }
    
    public void OpenMenuPanel()
    {
        if (user is { IsAnonymous: true })
        {
            LoginPanel.gameObject.SetActive(false);
            SignUpPanel.gameObject.SetActive(false);
            ForgetPasswordPanel.gameObject.SetActive(false);
            MenuPanel.gameObject.SetActive(true);
            PlayAsGuestBuuton.gameObject.SetActive(false);
            DeleteAccountButton.gameObject.SetActive(true);
            return;
        }
        
        logoutButton.gameObject.SetActive(true);
        connectIdButton.gameObject.SetActive(false);
        LoginPanel.gameObject.SetActive(false);
        SignUpPanel.gameObject.SetActive(false);
        ForgetPasswordPanel.gameObject.SetActive(false);
        MenuPanel.gameObject.SetActive(true);
        PlayAsGuestBuuton.gameObject.SetActive(false);
        DeleteAccountButton.gameObject.SetActive(false);
        
    }
    
    public void OpenForgetPasswordPanel()
    {
        LoginPanel.gameObject.SetActive(false);
        SignUpPanel.gameObject.SetActive(false);
        ForgetPasswordPanel.gameObject.SetActive(true);
        MenuPanel.gameObject.SetActive(false);
        PlayAsGuestBuuton.gameObject.SetActive(false);
    }

    #endregion

    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;

        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available) {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                InitializeFirebase();

                // Set a flag here to indicate whether Firebase is ready to use by your app.
            } else {
                UnityEngine.Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });

        RememberMe.isOn = PlayerPrefs.GetInt("RememberMe") == 1? true : false;
            
        if (RememberMe.isOn)
        {
            LoginEmail.text = PlayerPrefs.GetString("LoginEmail");
            LoginPassword.text = PlayerPrefs.GetString("LoginPassword");
        }
        else
        {
            LoginEmail.text = "";
            LoginPassword.text = "";
            PlayerPrefs.SetString("LoginEmail", "");
            PlayerPrefs.SetString("LoginPassword", "");
            PlayerPrefs.Save();
        }
        
        OpenLoginPanel();
    }

    public void SaveStateRememberMe()
    {
        PlayerPrefs.SetInt("RememberMe", RememberMe.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
    
    
    
    #region Register

    private IEnumerator CheckAlreadyName(string playerName)
    {
        var DBTask = databaseReference.Child("Users").OrderByChild("Username").GetValueAsync();
        
        yield return new  WaitUntil(predicate: () => DBTask.IsCompleted);
    
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            
            foreach (var childSnapshot in snapshot.Children)
            {
                var username = childSnapshot.Child("Username").Value.ToString();
                if (username == playerName)
                {
                    userNameTemp = username;
                    ShowNotification("Error", "Your Name In Already Use");
                    Debug.Log(userNameTemp);
                    yield break;
                }
            }
            CreateUser(SignUpEmail.text, SignUpPassward.text, SignUpUserName.text);
        }
    }

    public void SignUpUser()
    {
        if (string.IsNullOrEmpty(SignUpEmail.text) && string.IsNullOrEmpty(SignUpPassward.text) &&
            string.IsNullOrEmpty(SignUpConfirm.text) && string.IsNullOrEmpty(SignUpUserName.text))
        {
            ShowNotification("Alert", "Fields Empty! Please Input All Details");
            return;
        }

        if (SignUpPassward.text != SignUpConfirm.text)
        {
            ShowNotification("Alert", "Password And Confirm Password Not Match");
            return;
        }
        //TODO : Try to implement username already in use

        StartCoroutine(CheckAlreadyName(SignUpUserName.text));
    }
    
    private void CreateUser(string email, string password, string userName)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                foreach (var exception in task.Exception.Flatten().InnerExceptions)
                {
                    var firebaseEx = exception as FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        ShowNotification("Error",GetErrorMessage(errorCode)); 
                    }
                }
                return;
            }

            // Firebase user has been created.
            var newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            
            //Update Profile
            UpdateUserProfile(userName,URL_DefaultImange);

            PlayerName.text = userName;

            //Write to Database
            WriteNewUser(newUser.UserId,userName,newUser.Email);
            
        });
    }

    private void UpdateUserProfile(string userName,string url)
    {
        FirebaseUser user = auth.CurrentUser;
        if (user != null) {
            UserProfile profile = new Firebase.Auth.UserProfile {
                DisplayName = userName,
                PhotoUrl = new System.Uri(url),
            };
            user.UpdateUserProfileAsync(profile).ContinueWith(task => {
                if (task.IsCanceled) {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    return;
                }
                if (task.IsFaulted) {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("User profile updated successfully.");
                
                ShowNotification("Alert","Account Successfully Created");
                StartCoroutine(DownloadImage(user.PhotoUrl.ToString()));
            });
        }
    }
    
    #endregion
    
    #region Login

    public void LoginUser()
    {
        if (string.IsNullOrEmpty(LoginEmail.text) && string.IsNullOrEmpty(LoginPassword.text))
        {
            ShowNotification("Alert", "Fields Empty! Please Input All Details");
            return;
        }

        //Login
        SignInUser(LoginEmail.text, LoginPassword.text);
    }

    
    private void SignInUser(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                foreach (var exception in task.Exception.Flatten().InnerExceptions)
                {
                    var firebaseEx = exception as FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        ShowNotification("Error",GetErrorMessage(errorCode)); 
                    }
                }
                return;
            }

            var newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
            
            //Enter the game and show profile setup here 
            PlayerName.text = newUser.DisplayName;
            
            PlayAsGuestBuuton.gameObject.SetActive(false);
            OpenMenuPanel();
            
            PlayerPrefs.SetString("LoginEmail", $"{LoginEmail.text}");
            PlayerPrefs.SetString("LoginPassword", $"{LoginPassword.text}");
            PlayerPrefs.Save();
        });
    }

    //Test Write To Database
    private void WriteNewUser(string userId, string name, string email) 
    {
        databaseReference.Child("Users").Child(userId).Child("Username").SetValueAsync(name);
        databaseReference.Child("Users").Child(userId).Child("Email").SetValueAsync(email);
        databaseReference.Child("Users").Child(userId).Child("Damage_Done").SetValueAsync(0);
        databaseReference.Child("Users").Child(userId).Child("Time_Survived").SetValueAsync(0);
        databaseReference.Child("Users").Child(userId).Child("Wave_Survived").SetValueAsync(0);
    }
    
    private void WriteOldUser(string userId, string name, string email) 
    {
        databaseReference.Child("Users").Child(userId).Child("Username").SetValueAsync(name);
        databaseReference.Child("Users").Child(userId).Child("Email").SetValueAsync(email);
    }

    #endregion

    #region Reset Password

    public void ForgetPass()
    {
        if (string.IsNullOrEmpty(ForgetPasswordEmail.text))
        {
            ShowNotification("Alert", "Fields Empty! Please Input All Details");
            return;
        }
        SendPasswordResetEmail(ForgetPasswordEmail.text);
    }
    
    private void SendPasswordResetEmail(string email)
    {
        if (user == null) {
            auth.SendPasswordResetEmailAsync(email).ContinueWith(task => {
                if (task.IsCanceled) {
                    Debug.LogError("SendPasswordResetEmailAsync was canceled.");
                    return;
                }
                if (task.IsFaulted) {
                    Debug.LogError("SendPasswordResetEmailAsync encountered an error: " + task.Exception);
                    foreach (var exception in task.Exception.Flatten().InnerExceptions)
                    {
                        var firebaseEx = exception as FirebaseException;
                        if (firebaseEx != null)
                        {
                            var errorCode = (AuthError)firebaseEx.ErrorCode;
                            ShowNotification("Error",GetErrorMessage(errorCode)); 
                        }
                    }
                    return;
                }
                ShowNotification("Successful","Please Check Your MailBox");
                Debug.Log("Password reset email sent successfully.");
            });
        }
    }


    #endregion

    #region Error Notification

    private static string GetErrorMessage(AuthError errorCode)
    {
        var message = "";
        switch (errorCode)
        {
            case AuthError.AccountExistsWithDifferentCredentials:
                message = "Account Not Exist";
                break;
            case AuthError.MissingPassword:
                message = "Missing Password";
                break;
            case AuthError.WeakPassword:
                message = "Password Too Weak";
                break;
            case AuthError.WrongPassword:
                message = "Password is Wrong";
                break;
            case AuthError.EmailAlreadyInUse:
                message = "Your Email Already in Use";
                break;
            case AuthError.InvalidEmail:
                message = "Your Email Invalid";
                break;
            case AuthError.MissingEmail:
                message = "Your Email Missing";
                break;
            default:
                message = "Invalid Error";
                break;
        }
        return message;
    }
    
    private void ShowNotification(string title, string message)
    {
        NotificationTextTitel.text = title;
        NotificationTextMessage.text = message;
        NotificationFeild.gameObject.SetActive(true);
        StartCoroutine(CloseNotification());
    }

    private IEnumerator CloseNotification()
    {
        yield return new WaitForSeconds(2);
        NotificationFeild.gameObject.SetActive(false);
        NotificationTextTitel.text = "";
        NotificationTextMessage.text = "";
    }
    
    #endregion

    #region Get Current Sign-In User

    private void InitializeFirebase() 
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    private void AuthStateChanged(object sender, System.EventArgs eventArgs) 
    {
        if (auth.CurrentUser != user) {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null) {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn) {
                Debug.Log("Signed in " + user.UserId);
                isSignIn = true;
            }
        }
    }
    
    private void OnDestroy() 
    {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    #endregion
    public void Logout()
    {
        auth.SignOut();
        isSignIn = false;
        isSigned = false;
        ShowNotification("Successful","Logout Account");
    }
    
    public void DeleteUser()
    {
        var user = auth.CurrentUser;
        databaseReference.Child("Users").Child(user.UserId).RemoveValueAsync();
        if (user != null) {
            user.DeleteAsync().ContinueWith(task => {
                if (task.IsCanceled) {
                    Debug.LogError("DeleteAsync was canceled.");
                    return;
                }
                if (task.IsFaulted) {
                    Debug.LogError("DeleteAsync encountered an error: " + task.Exception);
                    return;
                }
                isSignIn = false;
                isSigned = false;
                Debug.Log("User deleted successfully.");
                ShowNotification("Successful","Account Deleted");
            });
        }
    }
    
    private void CheckID()
    {
        if (auth.CurrentUser.IsAnonymous)
        {
            logoutButton.gameObject.SetActive(false);
            connectIdButton.gameObject.SetActive(true);
            DeleteAccountButton.gameObject.SetActive(true);
            PlayerName.text = "Guest";
            ShowNotification("Welcome Back","You're Login as Guest");
        }
        else
        {
            logoutButton.gameObject.SetActive(true);
            connectIdButton.gameObject.SetActive(false);
            DeleteAccountButton.gameObject.SetActive(false);
            PlayerName.text = user.DisplayName;
            ShowNotification("Welcome Back",$"You're Login as {user.DisplayName}");
        }
    }

    private void LateUpdate()
    {
        if (isSignIn)
        {
            if (!isSigned)
            {
                isSigned = true;
                // Profile Player Here
                StartCoroutine(LoadUserScoreFromDB());
                if (user.PhotoUrl != null)
                {
                    StartCoroutine(DownloadImage(user.PhotoUrl.ToString()));
                }
                else
                {
                    StartCoroutine(DownloadImage(URL_DefaultImange));
                }
                CheckID();
                OpenMenuPanel();
            }
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PlayAsGuest()
    {
        auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError("SignInAnonymouslyAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SignInAnonymouslyAsync encountered an error: " + task.Exception);
                return;
            }

            var newUser = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            UpdateUserProfile("Guest",URL_DefaultImange);

            //Write to Database
            WriteNewUser(user.UserId,"Guest","Null");
        });
    }

    #region Transfer ID

    public void TransferID()
    {
        if (string.IsNullOrEmpty(SignUpEmail.text) && string.IsNullOrEmpty(SignUpPassward.text) &&
            string.IsNullOrEmpty(SignUpConfirm.text) && string.IsNullOrEmpty(SignUpUserName.text))
        {
            ShowNotification("Alert", "Fields Empty! Please Input All Details");
            return;
        }
        if (SignUpPassward.text != SignUpConfirm.text)
        {
            ShowNotification("Alert", "Password And Confirm Password Not Match");
            return;
        }
        StartCoroutine(CheckAlreadyNameIdTransfer(SignUpUserName.text));
    }

    private void TransferData(string email, string password, string username)
    {
        var credential = EmailAuthProvider.GetCredential(email, password);
        // (Anonymous user is signed in at that point.)
        auth.CurrentUser.LinkWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("TransferData was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("TransferData encountered an error: " + task.Exception);
                foreach (var exception in task.Exception.Flatten().InnerExceptions)
                {
                    var firebaseEx = exception as FirebaseException;
                    if (firebaseEx != null)
                    {
                        var errorCode = (AuthError)firebaseEx.ErrorCode;
                        ShowNotification("Error",GetErrorMessage(errorCode)); 
                    }
                }
                return;
            }
            //Transfer Finish Here
            UpdateUserProfile(username,user.PhotoUrl.ToString());
            //Write to Database
            WriteOldUser(user.UserId,username,email);
            SignInUser(email,password);
            ShowNotification("Successful","Transfer To Permanent ID");
        });
    }
    
    private IEnumerator CheckAlreadyNameIdTransfer(string playerName)
    {
        var DBTask = databaseReference.Child("Users").OrderByChild("Username").GetValueAsync();
        
        yield return new  WaitUntil(predicate: () => DBTask.IsCompleted);
    
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            
            foreach (var childSnapshot in snapshot.Children)
            {
                var username = childSnapshot.Child("Username").Value.ToString();
                if (username == playerName)
                {
                    userNameTemp = username;
                    ShowNotification("Error", "Your Name In Already Use");
                    Debug.Log(userNameTemp);
                    yield break;
                }
            }
            //Register
            TransferData(SignUpEmail.text, SignUpPassward.text,SignUpUserName.text);
        }
    }

    #endregion
    
    //LeaderBoard

    #region Load Data
    
    private IEnumerator LoadUserScoreFromDB()
    {
        var DBTask = databaseReference.Child("Users").Child(user.UserId).GetValueAsync();

        yield return new  WaitUntil(predicate: () => DBTask.IsCompleted);
        
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Child("Rank").Value == null)
        {
            //Show score to zero
            PlayerTopWave.text = "0";
            PlayerTopTime.text = "0.00 s";
            PlayerTopDamage.text = "0";
        }
        else
        {
            Debug.Log("Load DATA");
            DataSnapshot snapshot = DBTask.Result;
            PlayerTopWave.text = snapshot.Child("Wave_Survived").Value.ToString();
            PlayerTopDamage.text = snapshot.Child("Damage_Done").Value.ToString();
            PlayerTopTime.text = $"{float.Parse(snapshot.Child("Time_Survived").Value.ToString()) :F} s";
        }
    }
    
    #endregion
    
    private IEnumerator DownloadImage(string MediaUrl)
    {   
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError || request.result == UnityWebRequest.Result.ProtocolError) 
        {
            Debug.Log(request.error);
            ShowNotification("Error","Upload Photo Failure");
        }
        else
        {
            PlayerImage.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
            databaseReference.Child("Users").Child(user.UserId).Child("PhotoURL").SetValueAsync(MediaUrl);
        }
        
    }

    public void UploadPhotoUrl()
    {
        if (string.IsNullOrEmpty(Input_Url.text))
        {
            ShowNotification("Alert", "Fields Empty! Please Input Your Url");
            return;
        }
        UpdateUserProfile(user.DisplayName,Input_Url.text);
        ShowNotification("Successful","Upload Photo Successful");
    }
    
    public void RefreshPhoto()
    {
        StartCoroutine(DownloadImage(user.PhotoUrl.ToString()));
    }

    #region LeaderBoard

    public void OpenLeaderBoard()
    {
        StartCoroutine(LoadLeaderBoardData());
    }
    
    private IEnumerator LoadLeaderBoardData()
    {
        var DBTask = databaseReference.Child("Users").OrderByChild("Time_Survived").GetValueAsync();
        
        yield return new  WaitUntil(predicate: () => DBTask.IsCompleted);
    
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            foreach (Transform child in rankPanel.transform)
            {
                Destroy(child.gameObject);
            }
            
            var rank = 0;
            foreach (var childSnapshot in snapshot.Children.Reverse())
            {
                rank++;
                
                if (childSnapshot.Key == user.UserId)
                {
                    databaseReference.Child("Users").Child(user.UserId).Child("Rank").SetValueAsync(rank);
                    rankPlayer.text = rank.ToString();
                    photoLeaderBoard.texture = PlayerImage.texture;
                    waveLeaderBoard.text = PlayerTopWave.text;
                }
                
                if (rank <= 20)
                {
                    var username = childSnapshot.Child("Username").Value.ToString();
                    var waveSurvived = int.Parse(childSnapshot.Child("Wave_Survived").Value.ToString());
                    var image = childSnapshot.Child("PhotoURL").Value.ToString();
                    
                    var createData = Instantiate(rankDataPrefab, rankPanel);
                    createData.GetComponent<RankData>().NewRankData(rank, username,waveSurvived,image);
                }
            }
        }
    }

    #endregion
    
}