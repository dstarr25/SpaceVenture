using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AsteroidController : MonoBehaviour
{

    [SerializeField] float offsetX = 40;
    [SerializeField] float spacing;
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] float lowerYBound, upperYBound;
    [SerializeField] Transform AsteroidDestroyer;
    [SerializeField] float radiusMin, radiusMax;
    [SerializeField] float spacingMax;
    [SerializeField] Material[] materials;
    [SerializeField] int energyOccurence;
    [SerializeField] GameObject energyPrefab;
    [SerializeField] float energySpacing;
    [SerializeField] TextMeshProUGUI ScoreText;

    private float permaStartX, totalDistance;
    private int numAsteroids;
    private float startX;
    private Camera mainCamera;
    private float nextRadius;

    private void Awake() {
        mainCamera = Camera.main;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        startX = transform.position.x;
        permaStartX = transform.position.x;
        nextRadius = Random.Range(radiusMin, radiusMax);
        numAsteroids = 0;

    }

    // Update is called once per frame
    void Update()
    {
        totalDistance = transform.position.x - permaStartX;
        ScoreText.text = Mathf.Floor(totalDistance) + " m";
        float deltaX = transform.position.x - startX;
        if (deltaX >= spacing) 
        {

            if (numAsteroids < energyOccurence - 1) {
                //creates the asteroid
                GameObject newAsteroid = Instantiate(asteroidPrefab, new Vector3(transform.position.x,Random.Range(lowerYBound,upperYBound), transform.position.z), Quaternion.identity);
                newAsteroid.GetComponent<Renderer>().material = materials[Random.Range(0,materials.Length)];
                newAsteroid.transform.localScale = Vector3.one * nextRadius;
            } else {
                //creates energy object
                GameObject newEnergy = Instantiate(energyPrefab, transform.position,Quaternion.Euler(energyPrefab.transform.rotation.eulerAngles.x,energyPrefab.transform.rotation.eulerAngles.y ,energyPrefab.transform.rotation.eulerAngles.z));
                energyOccurence++;
                numAsteroids = -2;
                spacing = energySpacing;
            }
            
            numAsteroids++;

            //start of spacing stuff - spacing meaning the spacing between asteroids
            // when you go a certain distance deltaX after the last asteroid was spawned, spawns a new one. 
            // already stores the next asteroid radius so that the spacing for the next asteroid accounts for 
            // what its radius is going to be - dont want asteroids touching each other ever
            // also - energy object has its own spacing to make it stand out. this spacing occurs
            // before and after the energy object.
            deltaX = 0;
            startX = transform.position.x;
            float nextSpacingMin = nextRadius;
            nextRadius = Random.Range(radiusMin,radiusMax);
            nextSpacingMin += nextRadius;
            if (numAsteroids == energyOccurence - 1) 
                spacing = energySpacing;
            else if (numAsteroids == -1) {
                spacing = energySpacing;
                numAsteroids = 0;
            }
            else 
                spacing = Random.Range(nextSpacingMin,spacingMax);
            //end of spacing stuff
            
        }
        transform.position = new Vector3(mainCamera.transform.position.x + offsetX, mainCamera.transform.position.y, 0);
        AsteroidDestroyer.position = new Vector3(transform.position.x - 2*offsetX, AsteroidDestroyer.position.y, AsteroidDestroyer.position.z);
    }


}
