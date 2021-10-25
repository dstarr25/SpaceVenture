using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{

    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] float boostStrength = 10f;
    [SerializeField] float thresholdX;
    [SerializeField] float cameraY, cameraZ;
    [SerializeField] GameObject ExplosiveRocketPrefab;
    [SerializeField] Vector3 explosiveRocketOffset;
    [SerializeField] GameObject ExplosionPrefab;
    [SerializeField] float explosionForce,explosionRadius,explosionUpwardModifier;
    [SerializeField] float startingBoost;
    [SerializeField] float minY,maxY;
    [SerializeField] Slider fuelBar;
    [SerializeField] Image tooltips;
    [SerializeField] Image deathtext;
    public float currentBoost;
    private Camera mainCamera;
    private Rigidbody rb;
    private float inputX;
    private bool inputBoost;
    private bool hasDied;

    //sets up rigidbody and camera variables
    private void Awake() {
        rb = GetComponent<Rigidbody>();
        mainCamera = Camera.main;
    }

    // sets up camera starting location based on inspector entered values, fills up boost, hasdied is false
    void Start()
    {
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, cameraY, cameraZ);
        currentBoost = startingBoost;
        hasDied = false;
    }

    void Update()
    {
        //disables the hints when any input used in the game is pressed
        if (Input.GetAxis("Horizontal") != 0 || inputBoost) 
        {
            tooltips.enabled = false;
        }


        //INPUTS
        inputX = Input.GetAxis("Horizontal");
        if (Input.GetButton("Jump") && currentBoost > 0) 
        {
            inputBoost = true;
            currentBoost -= Time.deltaTime;
        } 
        else 
            inputBoost = false;

        fuelBar.value = currentBoost / startingBoost;


        //CAMERA POSITION - moves the camera according to the position of the rocket
        if (transform.position.x >= mainCamera.transform.position.x - thresholdX) 
        {
            mainCamera.transform.position = new Vector3(transform.position.x + thresholdX, cameraY,cameraZ);
        }


        //DIE IF OUT OF BOUNDS!!
        if (transform.position.y > maxY || transform.position.y < minY) {
            Die(transform.position);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move() 
    {
        //rotates the rocket based on the horizontal input axis
        rb.angularVelocity = Vector3.back * inputX * rotateSpeed;
        if (inputBoost) 
        {
            //applies a force in the direction of up for the rocket when the boost input is true
            Vector3 boostVector = transform.up * boostStrength;
            rb.AddForce(boostVector);
        } 

    }

    // if the rocket collides with an asteroid, hasdied goes to true so that you can't die twice 
    // due to multiple collisions in quick succession, calls die function with the location being the location of collision
    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "asteroid" && !hasDied) 
        {
            hasDied = true;
            Vector3 contactPosition = other.contacts[0].point;
            Die(contactPosition);
        }
    }

    // when the rocket enters a trigger collider, if its energy it fills up the boost and destroys 
    // the energy object, if its the asteroid destroyer, kills the rocket
    private void OnTriggerEnter(Collider other) {
        Debug.Log("TriggerEnter with " + other.gameObject.tag);
        if (other.gameObject.tag == "energy") {
            currentBoost = startingBoost;
            Destroy(other.gameObject);
        } else if (other.gameObject.tag == "asteroidDestroyer") {
            Die(transform.position);
        }
    }

    // Die function! shows the death text image, creates the explosive rocket object, creates an explosion 
    // at the location passed into the function, destroys the regular rocket
    public void Die(Vector3 explosionPosition) {
        deathtext.enabled = true;
        GameObject eRocket = Instantiate(ExplosiveRocketPrefab,transform.position - explosiveRocketOffset,Quaternion.identity);
        foreach(Transform t in eRocket.transform) 
            t.gameObject.GetComponent<Rigidbody>().AddExplosionForce(rb.velocity.magnitude * explosionForce,explosionPosition,explosionRadius,explosionUpwardModifier);
        Instantiate(ExplosionPrefab, explosionPosition, Quaternion.identity);
        Destroy(gameObject);

    }
}
