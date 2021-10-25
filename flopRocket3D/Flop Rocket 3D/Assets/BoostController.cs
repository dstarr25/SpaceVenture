using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostController : MonoBehaviour
{

    [SerializeField] GameObject smoke;
    private ParticleSystem smokePS;
    [SerializeField] Transform boostSpawnpoint;
    [SerializeField] GameObject BoostLightPrefab;
    [SerializeField] Movement rocketScript;
    private Light boostLight;
    private bool trynaBoost;

    //turns off particle system and light under the rocket and sets up variables
    private void Awake() {
        smokePS = smoke.GetComponent<ParticleSystem>();
        smokePS.Stop();
        boostLight = BoostLightPrefab.GetComponent<Light>();
        boostLight.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        // plays the particle system under the rocket (rocket trail) and lights up the light under 
        // the rocketwhen jump button pressed down and stops playing it and turns off the light when released, or when fuel runs out
        if (Input.GetButtonDown("Jump")) {
            smokePS.Play();
            boostLight.enabled = true;
        }
        if (Input.GetButtonUp("Jump") || rocketScript.currentBoost < 0) {
            smokePS.Stop();
            boostLight.enabled = false;
        }
        
    }
}
