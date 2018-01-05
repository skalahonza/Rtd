using UnityEngine;

public class CarMine : MineBase
{
    public override GameObject GetPrefab()
    {
        return Resources.Load<GameObject>("car_mine");
    }
}