using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class OnlineMode : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool globalOfflineMode;
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        globalOfflineMode = false;
    }

    public void toggleOfflineMode()
    {
        globalOfflineMode = !globalOfflineMode;
    }
    public void loadGame()
    {
        SceneManager.LoadScene("Master");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
