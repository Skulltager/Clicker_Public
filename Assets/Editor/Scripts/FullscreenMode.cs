using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class PlayModeFullscreen
{
    private static GameObject playModeGameObject;

    static PlayModeFullscreen()
    {
        EditorApplication.playModeStateChanged += PlayModeStateChanged;
    }

    private static void PlayModeStateChanged(PlayModeStateChange playModeStateChange)
    {
        switch (playModeStateChange)
        {
            case PlayModeStateChange.EnteredPlayMode:
                playModeGameObject = new GameObject("PlayModeFullscreen", typeof(PlayModeFullscreenMonoBehaviour));
                break;
            case PlayModeStateChange.ExitingPlayMode:
                GameObject.Destroy(playModeGameObject);
                break;
        }
    }
}

public class PlayModeFullscreenMonoBehaviour : MonoBehaviour
{
    private const float LONG_PRESS_TIME = 1.0f;
    private static readonly bool START_FULLSCREEN = false;

    private Type gameViewType;
    private PropertyInfo gameView_ShowToolbarProperty;
    private Type dockAreaType;
    private MethodInfo dockArea_AddTabMethod;
    private MethodInfo dockArea_FindPaneMethod;

    private List<EditorWindow> dummyViews;
    private EditorWindow fullscreenGameView;

    private float keyPressTime;
    private bool isFullscreen;

    private void Awake()
    {
        dummyViews = new List<EditorWindow>();

        // Hide this game object
        gameObject.hideFlags = HideFlags.HideInHierarchy;
        DontDestroyOnLoad(gameObject);

        // Cache reflection
        gameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");
        gameView_ShowToolbarProperty = gameViewType.GetProperty("showToolbar", BindingFlags.NonPublic | BindingFlags.Instance);

        dockAreaType = Type.GetType("UnityEditor.DockArea,UnityEditor");
        dockArea_AddTabMethod = dockAreaType.GetMethod("AddTab", new Type[] { typeof(EditorWindow), typeof(bool) });
        dockArea_FindPaneMethod = dockAreaType.GetMethod("FindPaneIndex", BindingFlags.Instance | BindingFlags.NonPublic, null, new Type[] { typeof(EditorWindow) }, null);
    }

    private IEnumerator Start()
    {
        if (!START_FULLSCREEN)
            yield break;

        // Wait until at least third frame for unity editor init reasons
        while (Time.frameCount < 3)
            yield return null;

        CreateFullscreen();
    }

    private void OnDestroy()
    {
        DestroyFullscreen();
    }

    private void Update()
    {
        UpdateInput();

        if (keyPressTime >= LONG_PRESS_TIME)
        {
            if (!isFullscreen)
                CreateFullscreen();
            else
                DestroyFullscreen();

            // Switching to min-value ensures player has to basically release the key first to trigger again
            keyPressTime = float.MinValue;
        }
    }

    private void CreateFullscreen()
    {
        if (isFullscreen)
            return;
        isFullscreen = true;

        // Create dummy views
        dummyViews = new List<EditorWindow>();

        // Add dummy views to dock areas with a game view inside
        UnityEngine.Object[] dockAreas = Resources.FindObjectsOfTypeAll(dockAreaType);
        foreach (UnityEngine.Object dockArea in dockAreas)
        {
            UnityEngine.Object[] gameViews = Resources.FindObjectsOfTypeAll(gameViewType);

            foreach (var item in gameViews)
            {
                int result = (int)dockArea_FindPaneMethod.Invoke(dockArea, new object[] { gameViews[0] });
                if (result == -1)
                    continue;

                // This is a dock area with a game view!
                EditorWindow dummyView = ScriptableObject.CreateInstance<EditorWindow>();
                dummyView.titleContent = new GUIContent("Dummy");
                dummyViews.Add(dummyView);

                dockArea_AddTabMethod.Invoke(dockArea, new object[] { dummyView, true });
            }
        }

        // Create fullscreen game view
        fullscreenGameView = (EditorWindow)ScriptableObject.CreateInstance(gameViewType);

        gameView_ShowToolbarProperty.SetValue(fullscreenGameView, false);

        fullscreenGameView.ShowPopup();
        fullscreenGameView.position = new Rect(new Vector2(0, 0), new Vector2(2560, 1440));
        fullscreenGameView.Focus();
    }

    private void DestroyFullscreen()
    {
        if (!isFullscreen)
            return;
        isFullscreen = false;

        // Destroy fullscreen game view
        fullscreenGameView.Close();
        fullscreenGameView = null;

        // Destroy dummy views
        foreach (EditorWindow dummyView in dummyViews)
            dummyView.Close();

        dummyViews.Clear();
    }

    private void UpdateInput()
    {
        bool isKeyPressed;
#if ENABLE_INPUT_SYSTEM
        if (UnityEngine.InputSystem.Keyboard.current == null)
            return;
        isKeyPressed = UnityEngine.InputSystem.Keyboard.current.escapeKey.isPressed;
#else
        isKeyPressed = Input.GetKey(KeyCode.Escape);
#endif

        if (isKeyPressed)
            keyPressTime += Time.unscaledDeltaTime;
        else
            keyPressTime = 0;
    }
}
