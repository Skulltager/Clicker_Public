using UnityEngine;

public class CursorController : MonoBehaviour
{
    public static CursorController instance { private set; get; }

    public readonly EventVariable<CursorController, int> cursorVisibleCount;
    private readonly EventVariable<CursorController, bool> editorCursorVisible;

    private CursorController()
    {
        cursorVisibleCount = new EventVariable<CursorController, int>(this, 0);
        editorCursorVisible = new EventVariable<CursorController, bool>(this, false);
    }

    private void Awake()
    {
        instance = this;
        cursorVisibleCount.onValueChangeImmediate += OnValueChanged_CursorVisibleCount;
        editorCursorVisible.onValueChange += OnValueChanged_EditorCursorVisible;
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            editorCursorVisible.value = !editorCursorVisible.value;
    }

    private void OnValueChanged_EditorCursorVisible(bool oldValue, bool newValue)
    {
        if (newValue)
            cursorVisibleCount.value++;
        else
            cursorVisibleCount.value--;
    }

    private void OnValueChanged_CursorVisibleCount(int oldValue, int newValue)
    {
        if (newValue > 0)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void OnDestroy()
    {
        cursorVisibleCount.onValueChange -= OnValueChanged_CursorVisibleCount;
        editorCursorVisible.onValueChange -= OnValueChanged_EditorCursorVisible;
    }
}