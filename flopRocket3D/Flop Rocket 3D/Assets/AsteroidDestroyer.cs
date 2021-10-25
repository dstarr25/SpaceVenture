using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidDestroyer : MonoBehaviour
{
    //destroys asteroids when they hit it so the scene doesn't have way too many asteroids in it
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "asteroid") {
            Destroy(other.gameObject);
        }
    }
}
