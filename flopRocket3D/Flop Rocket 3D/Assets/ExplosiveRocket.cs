using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExplosiveRocket : MonoBehaviour
{

    [SerializeField] KeyCode keyReset;

    // Update is called once per frame
    void Update()
    {
        // reloads scene if resetkey is pressed
        if (Input.GetKeyDown(keyReset)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
