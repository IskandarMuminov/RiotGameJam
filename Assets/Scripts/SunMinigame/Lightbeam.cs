using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightbeam : MonoBehaviour
{
    [SerializeField] bool isCaught;
    private CapsuleCollider capsuleCollider;

    private void Start()
    {
        isCaught = false;
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isCaught = true;
        }
    }
}
