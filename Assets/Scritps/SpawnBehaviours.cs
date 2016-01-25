using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SpawnBehaviours : MonoBehaviour
{
    //the arenaStartup canvas and the camera that looks at it.
    public GameObject arenaCanvas;
    public Camera arenaCam;

    public Text countDown;

    //array of spawnpoints for the enemies to spawn at
    public GameObject[] spawnPoints;

    //array of doors that block the arena spawnpoints.
    public GameObject[] doors;

    //array of availible enemies to spawn from.
    public GameObject[] boys;

    //<List> of active spawned enemies.
    public List<GameObject> activeSoldiers;

    //bools used to control the spawning
    public bool startSpawning;
    public bool emptyList;
    public bool isSpawning;

    void Start()
    {
        //the first time the game starts this needs to be set to start spawning.
        emptyList = true;
        isSpawning = false;
    }//Start

    void Update()
    {
        //checks to see if the list of active soldier is empty, and if its not already spawning. If all of these are met it starts to spawn a new wave.
        if (activeSoldiers.Count == 0 && emptyList && !isSpawning)
        {
            emptyList = false;
            StartCoroutine(spawning());
        }

        //when the game switches to the arena view this.gameObect becomes acitve. While this gameObject is active the arenaCanvas and cam should not be active.
        if (arenaCanvas.activeInHierarchy)
        {
            arenaCanvas.SetActive(false);
            arenaCam.gameObject.SetActive(false);
        }

        //when the count of the activeSoldiers <List> is 0 the list is empty again.
        if (activeSoldiers.Count == 0)
        {
            emptyList = true;
        }
    }//Update

    public IEnumerator spawning()
    {
        //when this IEnumerator is called isSpawning is set to true. This is used because the process of spawning takes time,
        //and this prevents the IEnumerator from being called more then once during spawning.
        isSpawning = true;

        countDown.color = Color.red;

        for (int i = 3; i > 0; i--)
        {
            countDown.text = "" + i;
            yield return new WaitForSeconds(1);
        }

        countDown.color = Color.clear;

        //over about 2 seconds the spawndoors move downwards.
        for (int i = 0; i < 119; i++)
        {
            //this for loop causes all the doors in the array to be moved down
            for (int d = 0; d < doors.Length; d++)
            {
                doors[d].transform.Translate(new Vector3(0.0f, 0.0f, 0.085f));
            }
            yield return new WaitForEndOfFrame();
        }

        //after the doors are done moving it continues to start spawning enemies at the spawnpoints.
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            //random range is used to determine the type of enemy to spawn.
            int spawnChoice = Mathf.FloorToInt(Random.Range(0, 6));

            GameObject enemy = (GameObject)Instantiate(boys[spawnChoice], spawnPoints[i].gameObject.transform.position, spawnPoints[i].gameObject.transform.rotation);
            //this adds the newly spawned enemies to the <List> of active soldiers.
            activeSoldiers.Add(enemy);
        }

        //the whole process waits 1 second here before closing the doors back up.
        yield return new WaitForSeconds(1);

        //over about 2 seconds the doors close back up.
        for (int i = 0; i < 119; i++)
        {
            //this for loop causes all the doors in the array to be moved up
            for (int d = 0; d < doors.Length; d++)
            {
                doors[d].transform.Translate(new Vector3(0.0f, 0.0f, -0.085f));
            }
            yield return new WaitForEndOfFrame();
        }

        //sets the emptyList bool to false because the list is no longer empty, and it has also finished spawning.
        emptyList = true;
        isSpawning = false;
    }//spawning

    //function used to clear the <List> of active soldiers. Mainly used to reset the list when the player dies.
    public void EmptyList()
    {
        foreach (GameObject soldier in activeSoldiers)
        {
            Destroy(soldier);
        }

        activeSoldiers.Clear();

        emptyList = true;
    }//EmptyList
}//Class
