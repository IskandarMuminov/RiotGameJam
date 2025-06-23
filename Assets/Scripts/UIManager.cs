using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI sun_amount_text;
    [SerializeField] private TextMeshProUGUI water_amount_text;
    [SerializeField] private TextMeshProUGUI mineral_amount_text;

    void OnEnable()
    {
        ResourceManager.StatUpdated += UpdateUI;
    }


    private void UpdateUI(string type, int amount)
    {
        TextMeshProUGUI text;
        switch (type)
        {
            case "Sun":
                text = sun_amount_text;
                break;
            case "Water":
                text = water_amount_text;
                break;
            case "Mineral":
                text = mineral_amount_text;
                break;
            default:
                //  invalid type
                return;
        }
        text.SetText(amount.ToString());
        
    }
}
