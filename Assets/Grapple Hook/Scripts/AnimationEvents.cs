using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEvents : MonoBehaviour
{
    //script for animation events
    public class StringEvent : UnityEvent<string> { }
    public UnityEvent unityEvent1;
    public UnityEvent unityEvent2;
    public UnityEvent unityEvent3;   
    void AnimationUnityEvent1()
    {
        unityEvent1.Invoke();
    }
    void AnimationUnityEvent2()
    {
        unityEvent2.Invoke();
    }
    void AnimationUnityEvent3()
    {
        unityEvent3.Invoke();
    }
    
}
