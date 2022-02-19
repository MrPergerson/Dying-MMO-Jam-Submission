using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HandleAbilityUI : MonoBehaviour
{
    [SerializeField] private GameObject cooldownObject;
    
    [SerializeField] private TextMeshProUGUI cooldownNumberText;
    private float cooldownNumber = 0;

    void Start()
    {
        HideCooldownNumber();
        StartCoroutine(CooldownCountdown());
    }

    IEnumerator CooldownCountdown()
    {
        if (cooldownNumber > 0)
        {
            SetCooldownText(cooldownNumber);
            if(!cooldownObject.activeSelf) 
                RevealCooldownNumber();
            
            cooldownNumber -= Time.deltaTime;
            SetCooldownText(cooldownNumber);
        }
        else
        {
            cooldownNumber = 0;
            SetCooldownText(cooldownNumber);
            if (cooldownObject.activeSelf)
                HideCooldownNumber();
        }

        yield return new WaitForSeconds(1.0f);
    }

    public void SetCooldownNumber(float num)
    {
        cooldownNumber = num;
    }

    private void HideCooldownNumber()
    {
        cooldownObject.SetActive(false);
    }

    private void RevealCooldownNumber()
    {
        cooldownObject.SetActive(true);
    }

    private void SetCooldownText(float num)
    {
        int number = (int)num;
        cooldownNumberText.text = number.ToString();
    }
}
