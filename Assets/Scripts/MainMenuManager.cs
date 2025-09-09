using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public void OnQuitClicked()
    {
        Application.Quit();

        // If in editor, stop play mode
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}