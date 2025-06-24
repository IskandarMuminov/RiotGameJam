using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Seedling : Only walking
// Sapling: Enable Jump ability
// Young: Enable vine grasp ability 
// Mature: Enable bash ability 
public enum Player_State
{
    Seedling = 0, Sapling = 1, Young = 2, Mature = 3
}

public struct Stats
{
    public Stats(int req)
    {
        current_amount = 0;
        required_amount = req;
    }

    public int current_amount { get; set; }
    public int required_amount { get; }

    public void Inc()
    {
        if (current_amount < required_amount)
        {
            current_amount++; 
        }
    }

    public bool IsReqReached()
    {
        return current_amount == required_amount; 
    }
}

public class ResourceManager : MonoBehaviour
{
    // Player Resource Manager
    public static ResourceManager Instance;
    // Track amount of sunlight, water and minerals collected 
    private Stats sun_stats = new(3);
    private Stats water_stats = new(3);
    private Stats mineral_stats = new(3);
    // Trigger events to update UI, Abilities and Player Mesh
    public static UnityAction<string, int> StatUpdated;
    public static UnityAction<Player_State> StateChanged;

    // Track Player State
    public Player_State player_State = Player_State.Seedling;

    void Start()
    {
        Instance = this;
    }

    public void IncreaseSun()
    {
        sun_stats = IncreaseStats(sun_stats, "Sun", Player_State.Sapling);
        Debug.Log("Current Sun: " + sun_stats.current_amount); 
    }

    public void IncreaseWater()
    {
        water_stats = IncreaseStats(water_stats, "Water", Player_State.Young); 
    }

    public void IncreaseMineral()
    {
        mineral_stats = IncreaseStats(mineral_stats, "Mineral", Player_State.Mature); 
    }

    public int GetCurrentSun()
    {
        return sun_stats.current_amount;
    }
    
    private Stats IncreaseStats(Stats stats, string type, Player_State next_state)
    {
        stats.Inc();
        // trigger event for UI update
        //TerrainChanger.instance.changeTerrain(next_state);
        StatUpdated?.Invoke(type, stats.current_amount);
        if (stats.IsReqReached())
        {
            
            // move to sapling stage
            if (player_State != next_state) {
                player_State = next_state;
                StateChanged?.Invoke(player_State);
            }
        }
        return stats; 
    }
    
}
