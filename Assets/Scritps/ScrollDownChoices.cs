using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Buttons
{
    public GameObject buttonOne;
    public GameObject buttonOnePanel;

    public GameObject buttonTwo;
    public GameObject buttonTwoPanel;

    public GameObject buttonThree;
    public GameObject buttonThreePanel;

    public GameObject buttonFour;
    public GameObject buttonFourPanel;

    public GameObject buttonFive;
    public GameObject buttonFivePanel;

    public GameObject buttonSix;
    public GameObject buttonSixPanel;

    public CanvasGroup notAssignedPanel;
}//Class Buttons

public class ScrollDownChoices : MonoBehaviour
{
    //Buttons is used for ease of access.
	public Buttons buttons;

    //the playerController in the scene
    public GameObject player;

    //the transform component of the scrollbar. Its scroll value is used.
    public Transform scrollbar;

    //which button and panel are currently active?
    public GameObject currentActiveButton;
    public GameObject currentActivePanel;

    //spawnpoints used for ensuring the first wave of enemies spwan when the player starts.
    public GameObject spawnPoints;

    //firstPass is used to indicate if this is the first time executing the code or not.
    bool firstPass;

    //the first time this is executed
    void Start()
    {
        firstPass = true;
    }//Start

    //update function used to determine what panel the player is on. When the player moves the slider the panels change according to its position.
    //the scrolValue has to be constantly checked because the player can move it at any time. This value is passed to multiple funtions,
    //if the value passed meets the requirements it proceeds to switch to that screen.
    void Update()
    {
        float scrollValue = scrollbar.GetComponent<Scrollbar>().value;

        OnValueOne(scrollValue);
        OnValueZero(scrollValue);
        OnValueZeroPointOne(scrollValue);
        OnValueZeroPointThree(scrollValue);
        OnValueZeroPointFive(scrollValue);
        OnValueZeroPointSeven(scrollValue);
    }//Update

    /// <summary>
    /// when the player is scrolling trough the choises the scrollbar outpust a value from 0 to 1, where 0 and 1 are normalized. I set the scrollbar to a snapping motion to make if more representative.
    /// as the player is scrolling it checks the output value of the scrollbar. When the bar changes it first checks the value,
    /// and then if the current active button != the same as the newly selected one. If they are not the same it runs the toggle button function and passes the new button and panel to the function.
    /// for all of the values that are not 0 or 1, there is an inbetween value set.
    /// </summary>
    /// <param name="ValueChanges"></param>
    void OnValueZero(float scrollbarValue)
    {
        if (scrollbarValue == 0 && currentActiveButton != buttons.buttonOne)
        {
            Debug.Log("toggling button");
            ToggleButton(buttons.buttonOne, buttons.buttonOnePanel);
        }
    }//OnValueZero

    void OnValueZeroPointOne(float scrollbarValue)
    {
        if (scrollbarValue > 0.1f && scrollbarValue < 0.3f && currentActiveButton != buttons.buttonTwo)
        {
            Debug.Log("toggling button 2");
            ToggleButton(buttons.buttonTwo, buttons.buttonTwoPanel);
        }
    }//OnValueZeroPointOne

    void OnValueZeroPointThree(float scrollbarValue)
    {
        if (scrollbarValue > 0.3f && scrollbarValue < 0.5f && currentActiveButton != buttons.buttonThree)
        {
            Debug.Log("toggling button 3");
            ToggleButton(buttons.buttonThree, buttons.buttonThreePanel);
        }
    }//OnValueZeroPointThree

    void OnValueZeroPointFive(float scrollbarValue)
    {
        if (scrollbarValue > 0.5f && scrollbarValue < 0.7f && currentActiveButton != buttons.buttonFour)
        {
            Debug.Log("toggling button 4");
            ToggleButton(buttons.buttonFour, buttons.buttonFourPanel);
        }
    }//OnValueZeroPointFive

    void OnValueZeroPointSeven(float scrollbarValue)
    {
        if (scrollbarValue > 0.7f && scrollbarValue < 0.9f && currentActiveButton != buttons.buttonFive)
        {
            Debug.Log("toggling button 5");
            ToggleButton(buttons.buttonFive, buttons.buttonFivePanel);
        }
    }//OnValueZeroPointSeven

    void OnValueOne(float scrollbarValue)
    {
        if (scrollbarValue == 1 && currentActiveButton != buttons.buttonSix)
        {
            Debug.Log("toggling button 6");
            ToggleButton(buttons.buttonSix, buttons.buttonSixPanel);
        }
    }//OnValueOne

    /// <summary>
    /// ButtonPressed is used to assign the choice the player made and then proceeds to toggle the playercontroller on.
    /// </summary>
    public void ButtonOnePressed()
    {
        player.GetComponent<CrossHair>().bullets.activeWeaponType = player.GetComponent<CrossHair>().bullets.m4a1;
        TogglePlayer();
    }//ButtonOnePressed

    public void ButtonTwoPressed()
    {
        player.GetComponent<CrossHair>().bullets.activeWeaponType = player.GetComponent<CrossHair>().bullets.ak47;
        TogglePlayer();
    }//ButtonTwoPressed

    public void ButtonThreePressed()
    {
        player.GetComponent<CrossHair>().bullets.activeWeaponType = player.GetComponent<CrossHair>().bullets.pistol;
        TogglePlayer();
    }//ButtonThreePressed

    public void ButtonNotAssigned()
    {
        StartCoroutine(blinkPanel(buttons.notAssignedPanel));
    }//ButtonNotAssigned

    //function that is used by all of the panels and buttons. This keeps getting reused as the player switches between panels.
    void ToggleButton(GameObject button, GameObject panel)
    {
        //if the button it switches to is not active.
        if (!button.activeSelf)
        {
            //it sets the current panel and button to false to remvoe them from view,
            //sets the passed button and panel to true, and reasignes these to the current.
            //then it runs a coroutine on them newly activated button and panel.
            currentActivePanel.gameObject.SetActive(false);
            currentActiveButton.SetActive(false);
            panel.gameObject.SetActive(true);
            button.SetActive(true);
            currentActiveButton = button;
            currentActivePanel = panel;
            StartCoroutine(animateButtonSwap(currentActiveButton));
            StartCoroutine(animateWeaponInfoPanel(currentActivePanel));
        }
    }//ToggleButton

    //this is called when the player has made thier selection of weapon. First it sets the player in the arena to true, then sets the active weapon type, and then sets the spawnpoints active.
    void TogglePlayer()
    {
        player.SetActive(true);
        player.GetComponent<CrossHair>().bullets.activeWeaponType.SetActive(true);
        spawnPoints.SetActive(true);

        //if this is the first time launching the arena this calls the spawning coroutine in SpawnBehaviours to start the spwaning of enemies.
        if (firstPass)
        {
            StartCoroutine(spawnPoints.GetComponent<SpawnBehaviours>().spawning());
            firstPass = false;
        }
    }//TogglePlayer

    //small coroutine that animates the buttons as the player swtitches between the screens.
    IEnumerator animateButtonSwap(GameObject current)
    {
        for (int i = 0; i < 30; i++)
        {
            current.transform.Rotate(12.0f, 0.0f, 0.0f);
            yield return new WaitForEndOfFrame();
        }
    }//animateButtonSwap

    //small coroutine that animates the panels as the player swtitches between the screens.
    IEnumerator animateWeaponInfoPanel(GameObject panel)
    {
        for (int i = 0; i < 15; i++)
        {
            panel.gameObject.transform.Translate(0.0f, 0.0f, 0.01f * i);
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < 15; i++)
        {
            panel.gameObject.transform.Translate(0.0f, 0.0f, -0.01f * i);
            yield return new WaitForEndOfFrame();
        }
    }//animateWeaponInfoPanel

    //Enumerator that causes a small not assigned panel to show to the player.
    IEnumerator blinkPanel(CanvasGroup group)
    {
        for (float i = 0; i < 29; i++)
        {
            //I tried to use 1 / 29 * i but it would not compute it correctly. So i used a calculator to make the first computation. That's how I got 0.034f.
            //I also tried writing it like so: ( 1 / 29 ) * i but that didn't work either. In all cases it would only return 1 as a whole number
            float alphaValue = 0.034f * i;
            group.alpha = alphaValue;
            yield return new WaitForEndOfFrame();
        }

        group.alpha = 1;

        yield return new WaitForSeconds(1);

        for (int i = 1; i < 29; i++)
        {
            float alphaValue = 1 - 0.034f * i;
            group.alpha = alphaValue;
            yield return new WaitForEndOfFrame();
        }

        group.alpha = 0;

        yield return null;
    }//blinkPanel
}//Class
