using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour
{

    //varable used to multiply the movement speed =
    public float MOVEMENT_SPEED;

    //the player
    GameObject player;
    //this is a transform that is a child of the player. The player gameObject has a transform that I want the NPC to look at.
    public Transform lookAtThis;

    //enemy anamator and hitAnimation. Anamator used to play animations. hitAnimation is used for its length in time.
    Animator enemyAnimation;
    public AnimationClip hitAnimation;

    //the gameObject spawnPoints contains a component that we need to access for spawn regulation.
    public GameObject spawnPoints;

    //bool used to indicate weather the NPC has a target to look at or not.
    bool lookingIsActive;

    //the NPCs are instantiated. When they are instantiated thay need to retrieve a couple things.
    void Awake()
    {
        spawnPoints = GameObject.Find("SpawnPoints");
        enemyAnimation = gameObject.GetComponent<Animator>();
    }//Awake

    void Update () {

        //checks if the gameObject is enabled in the hierarchy and if its not looking at something.
        if (gameObject.activeInHierarchy && !lookingIsActive)
        {
            //it looks through the scene for a gameObject named LookAtThis. and stores the retrieved gameObject and lets the lookingIsActive bool to true. Then starts a coroutine. 
            Debug.Log("Locating player");
            lookAtThis = GameObject.Find("LookAtThis").transform;
            lookingIsActive = true;
            StartCoroutine(moveTowardsPlayer());
        }
    }//Update

    //when the enemy gets close to the player i want them to do something just before they hit.

    IEnumerator moveTowardsPlayer()
    {
        //this coroutine only works if the lookingIsActive bool is true.
        while (lookingIsActive)
        {
            for (int i = 0; i < 59; i++)
            {
                //the gameObject looks at the lookAtThis transform and starts moving forward. 
                transform.LookAt(lookAtThis.transform);
                gameObject.transform.Translate(Vector3.forward * MOVEMENT_SPEED);
                yield return new WaitForEndOfFrame();
            }
        }
    }//moveTowardsPlayer

    void OnTriggerEnter(Collider other)
    {
        //when the gameObject runs into an object it checks the tag of that hit object. Only the the tag returns "Player" will  is do something.
        if (other.tag == "Player")
        {
            //assigns other as player so we can access the playerStats component on it.
            player = other.gameObject;

            //plays an animation for the enemy to resemble hitting
            enemyAnimation.Play("Hit");

            //does a check on the gameObject to see if it contains the SpawnBehaviours script. Then it removes itself from the <List> of enemies, clears the empty list item,
            //and destroys the gameObject after the hit animation has completed.
            if (spawnPoints.GetComponent<SpawnBehaviours>() != null)
            {
                spawnPoints.GetComponent<SpawnBehaviours>().activeSoldiers.Remove(gameObject);
                spawnPoints.GetComponent<SpawnBehaviours>().activeSoldiers.TrimExcess();
                Destroy(gameObject, hitAnimation.length);
            }

            //does a check for the PlayerStats component on the player. Runs the Hit funtion in the PlayerStats component.
            if (player.GetComponent<PlayerStats>())
            {
                player.GetComponent<PlayerStats>().Hit();
            }
        }
    }//OnTriggerEnter
}//Class
