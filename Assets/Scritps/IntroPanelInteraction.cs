using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntroPanelInteraction : MonoBehaviour {

    //canvas groups that need to be accessed throughout the script.
    public CanvasGroup mainScreen;
    public CanvasGroup howToPlayScreen;
    public CanvasGroup areYouSureScreen;
    public CanvasGroup switchScreen;

    //When the "Play" button is pressed.
	public void OnClickPlay()
    {
        Debug.Log("Play pressed");
        StartCoroutine(playPressed());
    }//OnClickPlay

    IEnumerator playPressed()
    {
        //causes a black screen fade over 2 seconds by changing the alpha values.
        for (int i = 0; i < 119; i++)
        {
            mainScreen.alpha = mainScreen.alpha - (i * (Time.deltaTime / 4));

            switchScreen.alpha = switchScreen.alpha + (i * (Time.deltaTime / 4));

            yield return new WaitForEndOfFrame();
        }

        mainScreen.alpha = 0;
        switchScreen.alpha = 1;
        Application.LoadLevel("Arena");
    }//playPressed

    //when the "Wot?" button is pressed.
    public void OnClickHowToPlay()
    {
        Debug.Log("Wot? pressed");
        StartCoroutine(ShowScreenInHalfSecond(howToPlayScreen));
    }//OnClickHowToPlay

    //when the "Wot?"'s back button is pressed.
    public void OnClickHowToPlayBack()
    {
        StartCoroutine(RemoveScreenInHalfSecond(howToPlayScreen));
    }//OnClickHowToPlayBack

    //when the "Quit" button is pressed.
    public void OnClickQuit()
    {
        Debug.Log("Quit pressed");
        StartCoroutine(ShowScreenInHalfSecond(areYouSureScreen));
    }//OnClickQuit

    //Quit's NO
    public void OnClickQuitNo()
    {
        StartCoroutine(RemoveScreenInHalfSecond(areYouSureScreen));
    }//OnClickQuitNo

    //Quit's YES
    public void OnClickQuitYes()
    {
        Application.Quit();
    }//OnClickQuitYes

    //IEnumerator used to show a screen in 1 second. It requires a canvas group.
    IEnumerator ShowScreenInOneSecond(CanvasGroup toShow)
    {
        toShow.gameObject.SetActive(true);

        for (int i = 0; i < 59; i++)
        {
            toShow.alpha = toShow.alpha + (i * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        toShow.alpha = 1;
    }//ShowScreenInOneSecond

    //IEnumerator used to remove a screen in 1 second. It requires a canvas group.
    IEnumerator RemoveScreenInOneSecond(CanvasGroup toShow)
    {
        for (int i = 0; i < 59; i++)
        {
            toShow.alpha = toShow.alpha - (i * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        toShow.alpha = 0;

        toShow.gameObject.SetActive(false);
    }//RemoveScreenInOneSecond

    //IEnumerator used to show a screen in a half of a second. It requires a canvas group.
    IEnumerator ShowScreenInHalfSecond(CanvasGroup toShow)
    {
        toShow.gameObject.SetActive(true);

        for (int i = 0; i < 29; i++)
        {
            toShow.alpha = toShow.alpha + (i * Time.deltaTime * 2);
            yield return new WaitForEndOfFrame();
        }

        toShow.alpha = 1;
    }//ShowScreenInHalfSecond

    //IEnumerator used to remove a screen in a half of a second. It requires a canvas group.
    IEnumerator RemoveScreenInHalfSecond(CanvasGroup toShow)
    {
        for (int i = 0; i < 29; i++)
        {
            toShow.alpha = toShow.alpha - (i * Time.deltaTime * 2);
            yield return new WaitForEndOfFrame();
        }

        toShow.alpha = 0;

        toShow.gameObject.SetActive(false);
    }//RemoveScreenInHalfSecond
}//Class
