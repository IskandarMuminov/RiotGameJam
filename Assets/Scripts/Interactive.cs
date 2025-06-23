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
    [Serializable]
    public class InteractEvent : UnityEvent {}

    // Event delegates triggered on interact
    [FormerlySerializedAs("onInteract")]
    [SerializeField]
        private InteractEvent m_OnInteract = new InteractEvent();

    // This function will run whatever function is set in the editor
    void Interact()
    {
        m_OnInteract.Invoke(); 
    }
    
}
