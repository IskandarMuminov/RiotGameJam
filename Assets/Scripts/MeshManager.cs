using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshManager : MonoBehaviour
{
    void OnEnable()
    {
        ResourceManager.StateChanged += UpdateMesh;
    }

    void OnDisable()
    {
        ResourceManager.StateChanged = UpdateMesh;
    }

    private void UpdateMesh(Player_State player_State)
    {
        transform.localScale += Vector3.one / 8;
        // try adding different plant / tree objects to the characters head
        // switch (player_State)
        // {
        //     case Player_State.Sapling:
        //         // increase player scale or change mesh 
        //         break;
        //     default:
        //         break;

        // }
    }

}
