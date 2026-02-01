using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    Image im;
    private static LoadingScreen main;
    private static bool isLoading;
    public static int LastScene = -1;

    private static List<string> scenesInBuild;

    // Start is called before the first frame update
    void Awake()
    {
        if (main == null)
        {
            main = this;
            DontDestroyOnLoad(transform.parent.gameObject);
            im = GetComponent<Image>();
            isLoading = false;

            scenesInBuild = new List<string>();
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                int lastSlash = scenePath.LastIndexOf("/");
                scenesInBuild.Add(scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
            }

            StartCoroutine(S());
        }
        else
        {
            Destroy(transform.parent.gameObject);
        }
    }

    void Update()
    {

    }

    public static bool Load(string _scene)
    {
        if (isLoading)
        {
            return false;
        }
        if (!scenesInBuild.Contains(_scene))
        {
            return false;
        }
        main.StopAllCoroutines();
        main.StartCoroutine(main.E(_scene));
        return true;
    }

    IEnumerator E(string _scene)
    {
        LastScene = SceneManager.GetActiveScene().buildIndex;

        isLoading = true;
        im.raycastTarget = true;
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(_scene);
        loadingOperation.allowSceneActivation = false;
        float T = 0;
        while (T < 1)
        {
            im.color = new Color(0, 0, 0, T);
            T += Time.deltaTime * 3;
            yield return null;
        }
        im.color = Color.black;

        while (loadingOperation.progress < 0.89f)
        {
            Debug.Log(loadingOperation.progress);
            yield return null;
        }
        loadingOperation.allowSceneActivation = true;

        StartCoroutine(S());
    }

    IEnumerator S()
    {
        im.raycastTarget = false;
        float T = 1;
        while (T > 0)
        {
            im.color = new Color(0, 0, 0, T);
            T -= Time.deltaTime * 3;
            yield return null;
        }
        im.color = new Color(0, 0, 0, 0);
        isLoading = false;
    }
}
