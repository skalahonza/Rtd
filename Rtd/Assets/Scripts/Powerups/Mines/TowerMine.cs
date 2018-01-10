using Assets.Scripts.Constants;
using UnityEngine;

/// <summary>
/// Mine spawned by the MineTower
/// </summary>
public class TowerMine : MineBase {
    public override GameObject GetPrefab()
    {
        return Resources.Load<GameObject>("tower_mine");
    }

    public override void Start()
    {
        base.Start();
        Destroy(gameObject,NumberConstants.TowerMineVanishTime);
    }
}