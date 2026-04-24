using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("SampleScene"); // 🔥 ใส่ชื่อฉากเกม
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");

        Application.Quit();

        // 🔥 เผื่อใช้ใน Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}