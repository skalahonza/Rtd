using UnityEngine;

public class TowerMine : MineBase {
    public override GameObject GetPrefab()
    {
        return Resources.Load<GameObject>("tower_mine");
    }
}