using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToDrawScene : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        }
    }
}
