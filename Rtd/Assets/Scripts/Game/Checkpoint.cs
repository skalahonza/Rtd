using Assets.Mechanics;
using UnityEngine;

/// <summary>
/// Checkpoint handler
/// </summary>
public class Checkpoint : MonoBehaviour {
    public GameObject[] positions;
    public int offset;

    /// <summary>
    /// Player reached checkpoint
    /// </summary>
    /// <param name="other">
    /// player Collider
    /// </param>
    void OnTriggerEnter (Collider other) {
        if (other.gameObject.GetComponent<Player> () == null)
            return;
        other.gameObject.GetComponent<Player> ().latest = this;
        other.gameObject.GetComponent<Player> ().checkpointOffest = offset;

        //heal the car
        var car = other.gameObject.GetComponent<CarSpirit> ();
        car.Hp = car.MaxHp;
        AnimationMechanics.SpawnParticle ("heal_wave", car.gameObject.transform);
    }
}