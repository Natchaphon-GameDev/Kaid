using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using Manager;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;


public class RankManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser user;
    public GameObject RankDataPrefap;
    public Transform RankPanel;
    public Transform LeaderBoardPanel;
    private DatabaseReference dataBaseReference;

    private bool leaderboardToggle;
    [Header("Player Data")] 
    [SerializeField] private TextMeshProUGUI waveSurvived;
    [SerializeField] private TextMeshProUGUI rankPlayer;
    [SerializeField] private RawImage playerImage;
    
    private void Start()
    {
        GameManager.Instance.GameOver += UpdateToDB;
        dataBaseReference = FirebaseDatabase.DefaultInstance.RootReference;
#if UNITY_ANDROID || UNITY_EDITOR
        InitializeFirebase();
#endif
    }

    private void UpdateToDB()
    {
        StartCoroutine(CheckDamageOverCurrentScore((int)UiManager.Instance.DamageDoneGameOver));
        StartCoroutine(CheckTimeSurvivedOverCurrentScore(Manager.WaveManager.Instance.TimerGameover));
    }
    
    private IEnumerator CheckDamageOverCurrentScore(int damage)
    {
        var DBTask = dataBaseReference.Child("Users").Child(user.UserId).GetValueAsync();
        
        yield return new  WaitUntil(predicate: () => DBTask.IsCompleted);
    
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            var snapshot = DBTask.Result;
            if (damage > int.Parse(snapshot.Child("Damage_Done").Value.ToString()))
            {
                StartCoroutine(UpdateDamageDone(damage));
            }
        }
    }
    
    private IEnumerator CheckTimeSurvivedOverCurrentScore(float time)
    {
        var DBTask = dataBaseReference.Child("Users").Child(user.UserId).GetValueAsync();
        
        yield return new  WaitUntil(predicate: () => DBTask.IsCompleted);
    
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            var snapshot = DBTask.Result;
            if (time > float.Parse(snapshot.Child("Time_Survived").Value.ToString()))
            {
                StartCoroutine(UpdateTimeSurvived(time));
                StartCoroutine(UpdateWaveSurvived((int)Manager.WaveManager.Instance.WaveIndex - 1));
            }
        }
    }

    private IEnumerator UpdateDamageDone(int damage)
    {
        var DBTask = dataBaseReference.Child("Users").Child(user.UserId).Child("Damage_Done").SetValueAsync(damage);
        
        yield return new  WaitUntil(predicate: () => DBTask.IsCompleted);
    
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            Debug.Log("Damage was update");
        }
    }
    
    private IEnumerator UpdateWaveSurvived(int wave)
    {
        var DBTask = dataBaseReference.Child("Users").Child(user.UserId).Child("Wave_Survived").SetValueAsync(wave);
        
        yield return new  WaitUntil(predicate: () => DBTask.IsCompleted);
    
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            Debug.Log("wave was update");
        }
    }
    
    private IEnumerator UpdateTimeSurvived(float time)
    {
        var DBTask = dataBaseReference.Child("Users").Child(user.UserId).Child("Time_Survived").SetValueAsync(time);
        
        yield return new  WaitUntil(predicate: () => DBTask.IsCompleted);
    
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            Debug.Log("time was update");
        }
    }

    private IEnumerator LoadUserScoreFromDB()
    {
        var DBTask = dataBaseReference.Child("Users").Child(user.UserId).GetValueAsync();
        
        yield return new  WaitUntil(predicate: () => DBTask.IsCompleted);
    
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Child("Rank").Value == null)
        {
            StartCoroutine(LoadLeaderBoardData());
            waveSurvived.text = "0";
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;
            waveSurvived.text = snapshot.Child("Wave_Survived").Value.ToString();
            rankPlayer.text = snapshot.Child("Rank").Value.ToString();
            StartCoroutine(DownloadImage(snapshot.Child("PhotoURL").Value.ToString()));
        }
    }

    private IEnumerator LoadLeaderBoardData()
    {
        var DBTask = dataBaseReference.Child("Users").OrderByChild("Time_Survived").GetValueAsync();
        
        yield return new  WaitUntil(predicate: () => DBTask.IsCompleted);
    
        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            DataSnapshot snapshot = DBTask.Result;

            foreach (Transform child in RankPanel.transform)
            {
                Destroy(child.gameObject);
            }
            
            var rank = 0;
            foreach (var childSnapshot in snapshot.Children.Reverse())
            {
                rank++;
                
                if (childSnapshot.Key == user.UserId)
                {
                    dataBaseReference.Child("Users").Child(user.UserId).Child("Rank").SetValueAsync(rank);
                    rankPlayer.text = rank.ToString();
                }
                
                if (rank <= 20)
                {
                    var username = childSnapshot.Child("Username").Value.ToString();
                    var waveSurvived = int.Parse(childSnapshot.Child("Wave_Survived").Value.ToString());
                    var image = childSnapshot.Child("PhotoURL").Value.ToString();
                    
                    var createData = Instantiate(RankDataPrefap, RankPanel);
                    createData.GetComponent<RankData>().NewRankData(rank, username,waveSurvived,image);
                }
            }
        }
    }

    IEnumerator DownloadImage(string MediaUrl)
    {   
        var request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError || request.result == UnityWebRequest.Result.ProtocolError) 
            Debug.Log(request.error);
        else
            playerImage.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
    }

    public void OpenLeaderBoard()
    {
        leaderboardToggle = !leaderboardToggle;
        if (leaderboardToggle)
        {
            TimeManager.Instance.DisableControler = true;
            StartCoroutine(LoadLeaderBoardData());
            StartCoroutine(LoadUserScoreFromDB());
            LeaderBoardPanel.gameObject.SetActive(true);
        }
        else
        {
            TimeManager.Instance.DisableControler = false;
            LeaderBoardPanel.gameObject.SetActive(!true);
        }
    }
    
    void InitializeFirebase() {
        Debug.Log("Setting up Firebase Auth");
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

// Track state changes of the auth object.
    void AuthStateChanged(object sender, System.EventArgs eventArgs) {
        if (auth.CurrentUser != user) {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null) {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn) {
                Debug.Log("Signed in " + user.UserId);
            }
        }
    }

    void OnDestroy() {
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

}
