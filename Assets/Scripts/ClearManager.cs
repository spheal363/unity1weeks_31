using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearManager : MonoBehaviour {
    private const string TITLE_SCENE = "Title";

    void Update() {
        // スペースキーが押されたときの処理
        if (Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene(TITLE_SCENE);
        }
    }
}