using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnESC : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadScene();
        }
    }

    void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}

