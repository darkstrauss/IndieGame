using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class PlayerPanels
{
    //Red screen overlay
    public GameObject playerHitPanel;

    //panel that indicates to the player what they had in the previous round
    public GameObject lastRoundScorePanel;
    public Text lastRoundScoreText;

    //player health and score text
    public Text playerHealthText;
    public Text playerScoreText;
}//Class PlayerPanels

public class PlayerStats : MonoBehaviour
{
    //playerPanels class contains multiple items. Used for ease of access.
    public PlayerPanels playerPanels;

    //arenaStartup is the canvas that the player sees before the arena starts.
    public GameObject arenaStartup;
    
    //the camera that is facing the arenaStartup canvas.
    public Camera arenaCam;

    //the spawnPoints that contains the SpawnBehaviours script
    public GameObject spawnPoints;

    //crossHairObject contains the CrossHair component
    public GameObject crossHairObject;

    //player
    GameObject player;

    //player score, health, and previous round
    int score;
    int playerHealth;
    int lastRoundScore;

    //when the player first starts the game it sets several values that the player needs, and sets the player health and score texts to what they need to start with.
    void Start ()
    {
        lastRoundScore = 0;
        playerHealth = 3;
        playerPanels.playerHealthText.text = "HP: " + playerHealth;
        score = 0;
        playerPanels.playerScoreText.text = "Score: " + score;
        player = gameObject;
    }//Start

    // Update is called once per frame
    void Update ()
    {
        //this resets everything when the player health reaches 0. So the player can then play the game again.
        if (playerHealth <= 0)
        {
            //This resets the player back to the middle of the arena
            //player.transform.position.Set(50.0f, 3.0f, 50.0f);
            
            //turns the player off to disable any input and also disables the player camera.
            player.SetActive(false);

            //enables the areana startup canvas so the player can start the arena again.
            arenaStartup.SetActive(true);

            //enables the camera that is looking at the startup panel.
            arenaCam.gameObject.SetActive(true);

            //calls the EmptyList function in SpawnBehaviours to remove all the enemies and clear the <List> that contains them.
            SpawnBehaviours spawnBehaviours = spawnPoints.GetComponent<SpawnBehaviours>();
            spawnBehaviours.EmptyList();

            //turns the weapon that the player had selected the first time around to a disabled state and then sets it to null. When the player then choses a different weapon on the next play,
            //it gets reasigned.
            CrossHair crossHair = crossHairObject.GetComponent<CrossHair>();
            crossHair.bullets.activeWeaponType.SetActive(false);
            crossHair.bullets.activeWeaponType = null;

            //turns the spawnPoints to inactive to stop the update function from spawning new enemies.
            spawnPoints.SetActive(false);

            //the first time the player plays the game this panel will not be shown. When the player has died and started again, they see their previous round's score, as well as their current score.
            if (!playerPanels.lastRoundScorePanel.activeInHierarchy)
            {
                playerPanels.lastRoundScorePanel.SetActive(true);
            }
            lastRoundScore = score;
            playerPanels.lastRoundScoreText.text = "Previous: " + lastRoundScore;

            //resets the score back to 0 for the next play through
            score = 0;
            playerPanels.playerScoreText.text = "Score: " + score;

            //resets the playerhealth panel back to read HP: 3 and then sets the actual playerhealth back to 3. If this was set the same way as score the function would break due to health not being 0.
            playerPanels.playerHealthText.text = "HP: 3";
            playerHealth = 3;
        }
    }//Update

    //Function used to decrease the player health.
    public void Hit()
    {
        playerHealth--;
        playerPanels.playerHealthText.text = "HP: " + playerHealth;
        //when the player is hit a red screenoverlay is shown to the player to indicate that they have been hit.
        StartCoroutine(hitCam());
    }//Hit

    //Function used to add score.
    public void AddScore()
    {
        score++;
        playerPanels.playerScoreText.text = "Score: " + score;
    }//AddScore

    //this IEnumerator runs over 30 frames. Roughly half a second. The first for loop causes the alpha chanel to increase. 0 = 0, 1 = 255 color value.
    //the second for loop decreases the alpha chanel value making is invisible again. The Color.clear ensures that the panel is completely invisible after completion.
    IEnumerator hitCam()
    {
        Image playerHitImage = playerPanels.playerHitPanel.GetComponent<Image>();

        for (int i = 0; i < 15; i++)
        {
            playerHitImage.color = new Color(1.0f, 0.0f, 0.0f, 0.03f * i);
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < 15; i++)
        {
            playerHitImage.color = new Color(1.0f, 0.0f, 0.0f, 0.45f - (i * 0.03f));
            yield return new WaitForEndOfFrame();
        }

        playerHitImage.color = Color.clear;

        yield return null;
    }//hitCam
}//Class
