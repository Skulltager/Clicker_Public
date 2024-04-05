using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Startup : MonoBehaviour
{
    private const string SCENE_ESSENTIALS = "Essentials";
    private const string SCENE_GAME = "Game";
    private const string SCENE_STARTUP = "Startup";

    private void Awake()
    {
        StartCoroutine(Routine_LoadGameFromStartup());
    }

    private IEnumerator Routine_LoadGameFromStartup()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SCENE_ESSENTIALS, LoadSceneMode.Additive);
        while (!operation.isDone)
            yield return null;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(SCENE_ESSENTIALS));

        operation = SceneManager.LoadSceneAsync(SCENE_GAME, LoadSceneMode.Additive);
        while (!operation.isDone)
            yield return null;

        operation = SceneManager.UnloadSceneAsync(SCENE_STARTUP);
        while (!operation.isDone)
            yield return null;
    }
}
