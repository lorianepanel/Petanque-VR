using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public void StartTheGame()
    {
        StartCoroutine(LoadGameScene());
    }

    private IEnumerator LoadGameScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("InGame");

        while (!asyncLoad.isDone)
        {
            // Ajouter une petite pause pour optimiser les performances
            yield return new WaitForSeconds(1f);
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
