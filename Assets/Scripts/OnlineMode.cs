using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class OnlineMode : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool globalOfflineMode;
    
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("GameController");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    
    void Start()
    {
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
    public void loadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
