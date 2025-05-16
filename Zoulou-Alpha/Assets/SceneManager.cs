using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManager : MonoBehaviour
{   
    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is quitting...");
    }    
}
