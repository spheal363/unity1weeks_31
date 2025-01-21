using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour {
    /// <summary>
    /// 指定されたシーンに切り替える
    /// </summary>
    /// <param name="sceneName">切り替えるシーンの名前</param>
    public static void ChangeScene(string sceneName) {
        if (string.IsNullOrEmpty(sceneName)) {
            Debug.LogError("シーン名が空またはnullです。正しいシーン名を指定してください。");
            return;
        }

        try {
            SceneManager.LoadScene(sceneName);
        } catch (System.Exception ex) {
            Debug.LogError($"シーンの読み込みに失敗しました: {ex.Message}");
        }
    }
}