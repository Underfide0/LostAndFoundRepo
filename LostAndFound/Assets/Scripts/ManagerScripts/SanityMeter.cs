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
    public bool crazyTime;

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
        
        if (playerController.isDriving || playerController.isCopiling)
        {
            timer = timer;

        }
        else
        {
           
            timer -= Time.deltaTime;
            SanityImage.fillAmount = timer / maxTime;
        }

        if (timer < maxTime && playerController.inCabin ){
            StartCoroutine(RegenerateSanity());
        }

        crazyMoment();
    }

    public void RecoverSanity()
    {
        timer += 50;   
    }

    public IEnumerator RegenerateSanity()
    {
        float incrementAmount = 0.5f;
        while (timer < maxTime)
        {
            timer += incrementAmount * Time.deltaTime;
            yield return null;
        }
    }

    public void crazyMoment()
    {
        if (timer <= 0)
        {
            crazyTime = true;
            Debug.Log("locoooo");
        }
    }
}
