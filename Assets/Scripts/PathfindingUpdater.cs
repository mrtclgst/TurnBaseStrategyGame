using System;
using System.IO;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{
    
    private void Start()
    {
        DestructibleCrate.OnAnyCrateDestroyed += DestructibleCrate_OnOnAnyCrateDestroyed;
    }

    private void DestructibleCrate_OnOnAnyCrateDestroyed(object sender, EventArgs e)
    {
        DestructibleCrate destructibleCrate = sender as DestructibleCrate;
        Pathfinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GetGridPosition(), true);
    }
}