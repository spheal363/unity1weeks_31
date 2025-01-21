using UnityEngine;

public class TitleManager : MonoBehaviour {
    // タイトル画面のCanvasを格納
    [SerializeField] private GameObject titleCanvas;
    // 操作説明画面のCanvasを格納
    [SerializeField] private GameObject instructionCanvas;

    // 現在操作説明画面が表示されているかどうか
    private bool isInstructionVisible = false;
    // テキストの点滅を制御するクラス
    private TextBlinking textBlinking;
    // タイトル画面のプレイヤーを制御するクラス
    private TitlePlayer titlePlayer;

    private void Start() {
        InitializeComponents();
    }

    private void Update() {
        HandleInput();
    }

    /// <summary>
    /// 必要なコンポーネントを初期化
    /// </summary>
    private void InitializeComponents() {
        textBlinking = titleCanvas.GetComponentInChildren<TextBlinking>();
        titlePlayer = FindObjectOfType<TitlePlayer>();
    }

    /// <summary>
    /// 入力を処理
    /// </summary>
    private void HandleInput() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            ToggleInstructions();
        }
    }

    /// <summary>
    /// 操作説明の表示状態を切り替える
    /// </summary>
    private void ToggleInstructions() {
        if (isInstructionVisible) {
            HideInstructions();
        } else {
            ShowInstructions();
        }
    }

    /// <summary>
    /// 操作説明画面を表示する
    /// </summary>
    private void ShowInstructions() {
        SetCanvasState(false, true);
        isInstructionVisible = true;
    }

    /// <summary>
    /// 操作説明画面を非表示にする
    /// </summary>
    private void HideInstructions() {
        SetCanvasState(true, false);
        isInstructionVisible = false;
        RestartTextBlinking();
        ResetTitlePlayerState();
    }

    /// <summary>
    /// 指定された状態に基づいてCanvasを切り替える。
    /// </summary>
    /// <param name="titleActive">タイトル画面の表示状態。</param>
    /// <param name="instructionActive">操作説明画面の表示状態。</param>
    private void SetCanvasState(bool titleActive, bool instructionActive) {
        titleCanvas.SetActive(titleActive);
        instructionCanvas.SetActive(instructionActive);
    }

    /// <summary>
    /// TextBlinkingの動作を再開。
    /// </summary>
    private void RestartTextBlinking() {
        if (textBlinking != null) {
            textBlinking.StartBlinking();
        } else {
            Debug.LogWarning("TextBlinking が初期化されていないため、動作を再開できません。");
        }
    }

    /// <summary>
    /// TitlePlayerの状態をリセット。
    /// </summary>
    private void ResetTitlePlayerState() {
        if (titlePlayer != null) {
            titlePlayer.ResetWalkSE();
        } else {
            Debug.LogWarning("TitlePlayer が初期化されていないため、状態をリセットできません。");
        }
    }
}
