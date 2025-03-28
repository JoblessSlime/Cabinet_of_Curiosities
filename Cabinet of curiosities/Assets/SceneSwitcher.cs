using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.GraphicsBuffer;

public class SceneSwitcher : MonoBehaviour
{
    public string sceneToLoad;
    public GameObject book;

    public void LoadScene()
    {
        DontDestroyOnLoad(book);
        SceneManager.LoadScene(sceneToLoad);
    }
}