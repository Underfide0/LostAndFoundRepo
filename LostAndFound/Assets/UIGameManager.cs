using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameManager : MonoBehaviour
{
    [SerializeField] private Image Stamina;
    
    void Start()
    {
        UpdateStamina(100);
    }

    // Update is called once per frame
    private void OnEnable()
    {
        PlayerMovement.OnStaminaChange += UpdateStamina;
    }

    private void OnDisable()
    {
        PlayerMovement.OnStaminaChange -= UpdateStamina;
    }
    private void UpdateStamina(float currentStamina)
    {
        Stamina.fillAmount = currentStamina/100;
    }
}
