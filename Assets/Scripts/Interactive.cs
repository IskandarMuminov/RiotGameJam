using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Interactive : MonoBehaviour
{
    // This component allows an item to be seleccted and interacted with // 
    // When hovered over it should be highlighted 
    // Set the event in the editor 
    [SerializeField] private Outline outline; 
    [Serializable]
    public class InteractEvent : UnityEvent { }

    // Event delegates triggered on interact
    [FormerlySerializedAs("onInteract")]
    [SerializeField]
    private InteractEvent m_OnInteract = new InteractEvent();

    bool allowInteraction = false;

    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (allowInteraction)
            {
                Interact(); 
            }
        }
    }
    // This function will run whatever function is set in the editor
    public void Interact()
    {
        m_OnInteract.Invoke();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision Start");
        Debug.Log(other.gameObject.tag);
        if (other.gameObject.tag == "Player")
        {
            // enable outline 
            // allow player to interact
            allowInteraction = true;
            outline.enabled = true;
        }

    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // disable outline 
            // prevent interaction 
            allowInteraction = false;
            outline.enabled = false;
        }
    }

}
