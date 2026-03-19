using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
        // Unityエディタでテスト中は止める
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ビルド後のゲームではアプリを終了
        Application.Quit();
#endif
    }
}