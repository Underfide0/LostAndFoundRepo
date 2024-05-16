using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PhotoManager : MonoBehaviour
{
    public Material[] materialToApply;

    public RawImage[] images;

    public GameObject[] marks;

    public GameObject[] hangedPhotos;   

    public int numberPhoto;

    public int photosTaken;

    [SerializeField] private GameObject comeBackUI;

    [SerializeField] private GameObject cabinaBien;

    [SerializeField] private GameObject cabinaMal;


    public void Screenshot()
    {
        
            Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            texture.ReadPixels(new Rect(2, 2, Screen.width, Screen.height), 2, 2);
            texture.Apply();

            photosTaken++;

            byte[] bytes = texture.EncodeToPNG();
            string filePath = Path.Combine(Application.persistentDataPath, "screenshot_" + numberPhoto + ".png");
            File.WriteAllBytes(filePath, bytes);

            Debug.Log("Screenshot " + numberPhoto + " saved on " + filePath);

            if (numberPhoto < materialToApply.Length)
            {
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(bytes);
                materialToApply[numberPhoto].mainTexture = tex;
                images[numberPhoto].texture = tex;
                marks[numberPhoto].SetActive(true);
                hangedPhotos[numberPhoto].SetActive(true);
            }
            else
            {
                Debug.LogWarning("Algo has hecho mal");
                
            }
        

        if (photosTaken == materialToApply.Length)
        { 
            comeBackUI.SetActive(true);

            cabinaBien.SetActive(false);
            cabinaMal.SetActive(true);
        }

        Debug.Log("phototaken");
    }
}
