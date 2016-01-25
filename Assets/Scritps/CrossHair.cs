using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Bullets
{
    //public GameObject activeBulletType.
    public GameObject activeWeaponType;

    //public GameObject weaponType1.
    public GameObject m4a1;

    //public GameObject weaponType2.
    public GameObject ak47;

    //public GameObject weaponType3;
    public GameObject pistol;

    //public ParticleSystem for bullet shots.
    public ParticleSystem gunShot;

    //public ParticleSystem for the bullets hitting.
    public ParticleSystem gunShotHit;

    //public GameObject used to spawn the bullet particles.
    public GameObject bulletSpawn;

    //public AudioClip for gun fire sound
    public AudioClip gunFire;

}//Class Bullets

public class CrossHair : MonoBehaviour
{
    //bullets containts several gameObejcts and particle systems. this is used for ease of access.
    public Bullets bullets;

    //the texture to be used for the player crosshair.
    public Texture2D crosshairTexture;
    //crosshair scale
    public float crosshairScale = 1;

    //camera that is used to do the raycast from
    public Transform cam;
    
    public GameObject spawnBehaviourObject;

    PlayerStats playerStats;
    float previousShot;

    //gets the playerstats component that is on this gameObject on awake. PlayerStats is used to store score.
    void Awake()
    {
        playerStats = gameObject.GetComponent<PlayerStats>();
    }//Awake

    void OnGUI()
    {
        //draws a crosshair in the middle of the screen to indicate to the player where they are aiming with their weapon.
        if (crosshairTexture != null)
        {
            GUI.DrawTexture(new Rect((Screen.width - crosshairTexture.width * crosshairScale) / 2, (Screen.height - crosshairTexture.height * crosshairScale) / 2, crosshairTexture.width * crosshairScale, crosshairTexture.height * crosshairScale), crosshairTexture);
        }
    }//OnGUI

    void Update()
    {
        //used to give a rate of fire for the player.
        previousShot = previousShot + Time.deltaTime;

        //a raycast is sent when the player presses down on the first mouse button in the game world, and if the previous shot was longer than 0.15 seconds ago.
        if (Input.GetMouseButton(0) && previousShot > 0.15f)
        {
            //creates a new audiosource to play a gunfire sound. I create a new one because the footsteps use one as well.
            //the volume is set to half so the player doesn't recieve hearing damage. Adds the gunfire audio clip to the component and then plays.
            //when the sound is done playing it removes the newly created audio source. This also allows multiple fire sounds to run at the same time. 
            AudioSource gunFireAudioSource = gameObject.AddComponent<AudioSource>();
            gunFireAudioSource.volume = 0.3f;
            gunFireAudioSource.clip = bullets.gunFire;
            gunFireAudioSource.Play();
            Destroy(gunFireAudioSource, bullets.gunFire.length);

            //instantiates a particle effect to show the player that he is shooting.
            ParticleSystem shot = (ParticleSystem)Instantiate(bullets.gunShot, bullets.bulletSpawn.transform.position, bullets.bulletSpawn.transform.rotation);
            //removes the instantiated particle after it has run trough its lifetime
            Destroy(shot.gameObject, shot.startLifetime);

            //need to check the tag of what i hit. if the tag is 'enemy' it needs to add force.

            //refered to object that the raycast hits.
            RaycastHit hit;

            //how the ray casts. In this instance its from the middle of the main camera.
            Ray ray = new Ray(cam.position, cam.forward);

            //if the ray hits something
            if (Physics.Raycast(ray, out hit))
            {
                //creates a small particle effect where the player shoots, it destroys itself after its lifetime.
                Quaternion hitRotation = gameObject.transform.rotation;
                ParticleSystem shotHit = (ParticleSystem)Instantiate(bullets.gunShotHit, hit.point, hitRotation);
                Destroy(shotHit.gameObject, shotHit.startLifetime);

                //if the hit object has the tag 'enemy' it adds velocity up to the object it hit. It sets the gameObject to be destroyed 1.5 seconds after it has been hit.
                if (hit.collider.tag == "Enemy")
                {
                    //if the hit collider has a rigidbody it adds velocity upwards to move the gameobject out of the player's view.
                    if (hit.collider.attachedRigidbody)
                    {
                        hit.collider.attachedRigidbody.AddRelativeForce(new Vector3(0.0f, 20.0f, 0.0f), ForceMode.VelocityChange);
                    }
                    //disable the collider to prevent dubble scoring on a sigle target
                    hit.collider.enabled = false;
                    
                    //destroys the hit gameobject 1.5 seconds after it was hit.
                    Destroy(hit.collider.gameObject, 1.5f);

                    //adds score for the player to keep track of the kills they have made
                    playerStats.AddScore();

                    //this trims the empty gameObjects from the List<GameObject> that contains the enemy soldiers. When the list is empty the next wave can spawn.
                    SpawnBehaviours spawnBehaviours = spawnBehaviourObject.GetComponent<SpawnBehaviours>();
                    spawnBehaviours.activeSoldiers.Remove(hit.collider.gameObject);
                    spawnBehaviours.activeSoldiers.TrimExcess();
                }
            }

            //sets the previous shot to zero so the player can shoot again
            previousShot = 0;

        }
    }//Update
}//Class
