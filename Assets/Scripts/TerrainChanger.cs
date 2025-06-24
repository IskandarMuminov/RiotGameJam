using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainChanger : MonoBehaviour
{
    public GameObject[] seedTerrains;
    public GameObject[] sapTerrains;
    public GameObject[] youngTerrains;
    public GameObject[] matureTerrains;
    public static TerrainChanger instance;
    // Start is called before the first frame update
    public void changeTerrain(Player_State state)
    {
        switch (state)
        {
            case Player_State.Seedling:

                break;
            case Player_State.Sapling:
                foreach (GameObject terrain in sapTerrains)
                {
                    terrain.SetActive(true);
                }
                break;
            case Player_State.Young:
                foreach (GameObject terrain in youngTerrains)
                {
                    terrain.SetActive(true);
                }
                break;
            case Player_State.Mature:
                foreach (GameObject terrain in matureTerrains)
                {
                    terrain.SetActive(true);
                }
                break;
        }
    }
}
