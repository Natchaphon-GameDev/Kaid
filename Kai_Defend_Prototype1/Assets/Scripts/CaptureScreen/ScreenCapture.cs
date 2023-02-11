using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScreenCapture : MonoBehaviour
{
    private int CaptureWidth = Screen.width; 
    private int CaptureHeight = Screen.height;

    public enum Format
    {
        RAW,
        JPG,
        PNG,
        PPM
    }
    

    private Format format = Format.JPG;

    private string outputFolder;

    private Rect rect;
    private RenderTexture renderTexture;
    private Texture2D screenShort;

    public bool isProcessing;

    private byte[] currentTextuer;
    public Image ShowImage;

    public UnityEvent OnShowImage;

    public string CurrentFirePath;
    private void Start()
    {
        outputFolder = Application.persistentDataPath + "/Screenshots/";
        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
            Debug.Log("Save Path will be" + outputFolder);
        }
        // DontDestroyOnLoad(this);
    }

    private string CreateFileName(int width, int height)
    {
        var timestamp = DateTime.Now.ToString("yyyyMMddTHHmmss");
        var filename = string.Format("{0}/screen_{1}x{2}_{3}.{4}", outputFolder, width, height, timestamp, format.ToString().ToLower());

        return filename;
    }

    private void CaptureScreenshot()
    {
        isProcessing = true;

        if (renderTexture != null) return;
        rect = new Rect(0, 0, CaptureWidth, CaptureHeight);
        renderTexture = new RenderTexture(CaptureWidth, CaptureHeight, 24);
        screenShort = new Texture2D(CaptureWidth, CaptureHeight, TextureFormat.RGB24, false);

        var camera = Camera.main;
        camera.targetTexture = renderTexture;
        camera.Render();
        RenderTexture.active = renderTexture;
        screenShort.ReadPixels(rect,0,0);

        camera.targetTexture = null;
        RenderTexture.active = null;

        var fileName = CreateFileName((int)rect.width,(int)rect.height);

        byte[] fileHeader = null;
        byte[] fileData = null;

        switch (format)
        {
            case Format.RAW:
                fileData = screenShort.GetRawTextureData();
                break;
            case Format.JPG:
                fileData = screenShort.EncodeToJPG();
                break;
            case Format.PNG:
                fileData = screenShort.EncodeToPNG();
                break;
            default:
            {
                var headerStr = string.Format("P6\n{0} {1}\n255\n",rect.width, rect.height);
                fileHeader = System.Text.Encoding.ASCII.GetBytes(headerStr);
                fileData = screenShort.GetRawTextureData();
                break;
            }
        }

        currentTextuer = fileData;
        new System.Threading.Thread(() =>
            {
                var file = System.IO.File.Create(fileName);

                if (fileHeader != null)
                {
                    file.Write(fileHeader,0,fileHeader.Length);
                }
                file.Write(fileData,0,fileData.Length);
                file.Close();
                Debug.Log($"Screenshot saved {fileName}, size {fileData.Length}");
                isProcessing = false;
            }
            ).Start();
        CurrentFirePath = fileName;

        StartCoroutine(ImageShowCase());
        Destroy(renderTexture);
        renderTexture = null;
        screenShort = null;
    }

    public void TakeScreenShot()
    {
        if (!isProcessing)
        {
            CaptureScreenshot();
        }
        else
        {
            Debug.Log("Processing");
        }
    }

    public IEnumerator ImageShowCase()
    {
        yield return new WaitForEndOfFrame();

        ShowImage.material.mainTexture = null;
        var texture = new Texture2D(CaptureWidth, CaptureHeight, TextureFormat.RGB24, false);
        texture.LoadImage(currentTextuer);
        ShowImage.material.mainTexture = texture;
        OnShowImage?.Invoke();
    }

    public void ShareImage()
    {
        new NativeShare()
            .AddFile(CurrentFirePath)
            .SetSubject("This is my Screen Shot in Kaid")
            .SetText("if you can beat me Let's Share")
            .Share();
    }
}
