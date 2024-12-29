using UnityEngine;

public class TitleManager : MonoBehaviour {
    public GameObject titleCanvas; // タイトル画面のCanvas
    public GameObject instructionCanvas; // 操作説明画面のCanvas
    private bool isInstructionVisible = false; // 現在操作説明が表示されているかどうか
    private TextBlinking textBlinking;
    private TitlePlayer titlePlayer;

    void Start() {
        textBlinking = titleCanvas.GetComponentInChildren<TextBlinking>();
        titlePlayer = FindObjectOfType<TitlePlayer>();
    }

    void Update() {
        // スペースキーが押されたときの処理
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (!isInstructionVisible) {
                // 操作説明画面を表示する
                ShowInstructions();
            } else {
                // 操作説明画面を非表示にする
                HideInstructions();
            }
        }
    }

    void ShowInstructions() {
        // タイトル画面を非表示、操作説明を表示
        titleCanvas.SetActive(false);
        instructionCanvas.SetActive(true);
        isInstructionVisible = true;
    }

    void HideInstructions() {
        // タイトル画面を表示、操作説明を非表示
        titleCanvas.SetActive(true);
        instructionCanvas.SetActive(false);
        isInstructionVisible = false;
        textBlinking.StartBlinking(); // TextBlinkingのコルーチンを再開

        // TitlePlayerのwalkSEの再生状態をリセット
        if (titlePlayer != null) {
            titlePlayer.ResetWalkSE();
        }
    }
}