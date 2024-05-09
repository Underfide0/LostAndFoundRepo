using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PhotoManager : MonoBehaviour
{
    public Material[] materialToApply;

    public int numberPhoto;

    public int photosTaken;

    [SerializeField] private GameObject comeBackUI;
    void Start()
    {
     
        


    }

    // Update is called once per frame
    

    public void ScreenshotOld()
    {

        int numPhotos = materialToApply.Length;

        for (int i = 0; i < numPhotos; i++)
        {
            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(2, 2, Screen.width, Screen.height), 2, 2);
            texture.Apply();

            photosTaken++;

            byte[] bytes = texture.EncodeToPNG();
            string filePath = Path.Combine(Application.persistentDataPath, "screenshot_" + i + ".png");
            File.WriteAllBytes(filePath, bytes);

            Destroy(texture);

            Debug.Log("Screenshot " + i + " saved on " + filePath);

            if (i < materialToApply.Length)
            {
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(bytes);
                materialToApply[numberPhoto].mainTexture = tex;
            }
            else
            {
                Debug.LogWarning("Algo has hecho mal");
                break;
            }
        }

        if (photosTaken == 10)
        {
            comeBackUI.SetActive(true);
        }

        Debug.Log("phototaken");
    }

    public void Screenshot()
    {
        
            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(2, 2, Screen.width, Screen.height), 2, 2);
            texture.Apply();

            photosTaken++;

            byte[] bytes = texture.EncodeToPNG();
            string filePath = Path.Combine(Application.persistentDataPath, "screenshot_" + numberPhoto + ".png");
            File.WriteAllBytes(filePath, bytes);

            Destroy(texture);

            Debug.Log("Screenshot " + numberPhoto + " saved on " + filePath);

            if (numberPhoto < materialToApply.Length)
            {
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(bytes);
                materialToApply[numberPhoto].mainTexture = tex;
            }
            else
            {
                Debug.LogWarning("Algo has hecho mal");
                
            }
        

        if (photosTaken == materialToApply.Length)
        { 
            comeBackUI.SetActive(true);
        }

        Debug.Log("phototaken");
    }
}
