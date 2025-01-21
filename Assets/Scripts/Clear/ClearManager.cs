using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearManager : MonoBehaviour {
    private const string TITLE_SCENE = "Title";

    void Update() {
        HandleSpaceKeyPress();
    }

    /// <summary>
    /// スペースキーが押されたときの処理
    /// </summary>
    private void HandleSpaceKeyPress() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            LoadTitleScene();
        }
    }

    /// <summary>
    /// タイトルシーンを読み込む
    /// </summary>
    private void LoadTitleScene() {
        SceneManager.LoadScene(TITLE_SCENE);
    }
}