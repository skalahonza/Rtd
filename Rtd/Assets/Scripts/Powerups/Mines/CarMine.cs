using UnityEngine;

/// <summary>
/// Mine spawned from the car when using Mine powerup
/// </summary>
public class CarMine : MineBase
{
    public override GameObject GetPrefab()
    {
        return Resources.Load<GameObject>("car_mine");
    }
}