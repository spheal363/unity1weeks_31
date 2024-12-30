using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearManager : MonoBehaviour {
    private const string TITLE_SCENE = "Title";

    void Update() {
        // �X�y�[�X�L�[�������ꂽ�Ƃ��̏���
        if (Input.GetKeyDown(KeyCode.Space)) {
            SceneManager.LoadScene(TITLE_SCENE);
        }
    }
}