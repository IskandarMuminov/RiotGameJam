using System.Collections;
using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.Events;

public class InputListener : MonoBehaviour
{
    UnityEvent m_sunLightPopup = new UnityEvent();
    private bool rounting = false;
    bool rt = false;
    bool plant = false;
    void Start()
    {
        m_sunLightPopup.AddListener(OnJumpPressed);
    }

    void Update()
    {
        int currentSun = ResourceManager.Instance.GetCurrentSun();
        int currentWater = ResourceManager.Instance.GetCurrentWater();
        int currentMineral = ResourceManager.Instance.GetCurrentMineral();
        // Press Jump to trigger the Listener
        if (Input.GetKeyDown(KeyCode.Space) && m_sunLightPopup != null && currentSun <= 3)
        {
            m_sunLightPopup.Invoke();
        }
        else if (currentSun == 3 && currentWater < 1 && !plant)
        {
            if (m_sunLightPopup != null) m_sunLightPopup.RemoveListener(OnJumpPressed);
            //Register the next area's popup
            StartCoroutine(WaterPopup());
        }
        else if (currentSun == 3 && currentWater == 1 && currentMineral < 3 && !rounting)
        {
            StartCoroutine(MineralPopup());
        }
        else if (currentSun == 3 && currentWater == 1 && currentMineral == 3 && !rt)
        {
            StartCoroutine(PlantPopup());
        }
    }

    void OnJumpPressed()
    {
        Debug.Log("Not Enough Sun! Current amount: " + ResourceManager.Instance.GetCurrentSun());
        //DialogueManager.Instance.Bark("Jump Popup", this.transform);
        DialogueManager.Instance.StartConversation("Jump Popup", this.transform);
    }
    
    IEnumerator WaterPopup()
    {
        plant = true;
        yield return new WaitForSecondsRealtime(10f);
        DialogueManager.Instance.StartConversation("Water need", this.transform);
        plant = false;
    }
    
    IEnumerator MineralPopup()
    {
        rounting = true;
        yield return new WaitForSecondsRealtime(10f);
        DialogueManager.Instance.StartConversation("Mineral Popup", this.transform);
        rounting = false;
    }
    
    IEnumerator PlantPopup()
    {
        rt = true;
        yield return new WaitForSecondsRealtime(10f);
        DialogueManager.Instance.StartConversation("Planting", this.transform);
        rt = false;
    }
    
}