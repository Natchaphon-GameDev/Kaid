using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class RankData : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI NumberText;
    public RawImage ProfileImage;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI ScoreText;

    public void NewRankData(int rank, string playerName, int waveSur,string url)
    {
        NumberText.text = rank.ToString();
        NameText.text = playerName;
        ScoreText.text = waveSur.ToString();
        StartCoroutine(DownloadImage(url));
    }

    private IEnumerator DownloadImage(string MediaUrl)
    {   
        var request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if(request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.DataProcessingError || request.result == UnityWebRequest.Result.ProtocolError) 
            Debug.Log(request.error);
        else
            ProfileImage.texture = ((DownloadHandlerTexture) request.downloadHandler).texture;
    }
    
}
