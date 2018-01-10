using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Car
{
    /// <summary>
    /// Car starts blinking and it is incollidable for a limited time
    /// </summary>
    public class CarGhostRespawn : MonoBehaviour
    {
        private readonly Renderer[] _renderers = new Renderer[5];
        private List<Collider> colliders = new List<Collider>();
        bool notend = false;

        public void Start()
        {
            for (var i = 0; i < 5; i++)
            {
                _renderers[i] = gameObject.transform.GetChild(i).GetComponent<Renderer>();
            }
            Destroy(this,5);

            //find other cars
            var cars = FindObjectsOfType<CarSpirit>().Select(x => x.gameObject.transform);
            //add their colliders
            foreach (var car in cars)
                colliders.AddRange(GetCarColliders(car));
            
            foreach (var col in colliders)
            {
                var meCols = GetCarColliders(transform);
                foreach (var me in meCols)
                    Physics.IgnoreCollision(me, col);
            }
            StartCoroutine(Blink());
        }

        private IEnumerator Blink()
        {
            while (true)
            {
                foreach (var carRenderer in _renderers)
                    carRenderer.enabled = !carRenderer.enabled;
                yield return new WaitForSeconds(0.2f);
            }
        }

        public void OnDestroy()
        {
            //if car is destroyed becouse of end of game
            if(!notend){
                return;
            }
            //restore collisions 
            foreach (var carRenderer in _renderers)
                carRenderer.enabled = true;

            foreach (var col in colliders)
            {
                var meCols = GetCarColliders(transform);
                foreach (var me in meCols)
                    Physics.IgnoreCollision(me, col,false);
            }
        }

        private IEnumerable<Collider> GetCarColliders(Transform carTransform)
        {
            yield return carTransform.GetComponent<BoxCollider>();
            foreach (var wheelCollider in carTransform.GetComponentsInChildren<WheelCollider>())
                yield return wheelCollider;
        }
    }
}