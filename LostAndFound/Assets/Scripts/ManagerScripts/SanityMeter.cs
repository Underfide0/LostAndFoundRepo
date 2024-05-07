using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanityMeter : MonoBehaviour
{

    public Image SanityImage;
    [SerializeField] private GameObject player;
    private PlayerController playerController;
    public float timer;
    [SerializeField] private float maxTime;

    // Start is called before the first frame update
    void Start()
    {
        timer = maxTime;
        
    }

    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!playerController.isDriving || !playerController.isCopiling)
        {
            timer -= Time.deltaTime;
            SanityImage.fillAmount = timer / maxTime;

        }
        else
        {
            //timer = timer;
        }

        RecoverSanity();

       
    }

    public void RecoverSanity()
    {
        timer += 20;   
    }
}
