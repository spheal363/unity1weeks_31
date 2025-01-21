using System;
using TMPro;
using UnityEngine;
using unityroom.Api;

/// <see cref="https://blog.unityroom.com/entry/2024/01/28/003740">スコアランキング機能の実装方法</see>
///

public class ScoreManager : MonoBehaviour {
    private int lanternCount; // 使用されたランタンの数
    private float clearTime; // クリアまでの経過時間
    private int score; // 最終スコア
    private TMP_Text scoreText; // スコアを表示するテキストコンポーネント

    // ランタンスコア関連の設定
    [SerializeField] private int maxLanternScore = 2000; // ランタン部門の最大スコア
    [SerializeField] private int lanternPenalty = 50; // ランタン1個あたりのペナルティ

    // タイムスコア関連の設定
    [SerializeField] private int maxTimeScore = 8000; // タイム部門の最大スコア
    [SerializeField] private float timePenalty = 50f; // 1秒あたりのペナルティ

    void Start() {
        InitializeValues();
        CalculateScore();
        UpdateScoreText();
        SendScoreToServer();
    }

    /// <summary>
    /// 初期値を設定
    /// </summary>
    private void InitializeValues() {
        // ランタン数とクリア時間をGoalクラスから取得
        lanternCount = Goal.GetLanternCount();
        clearTime = Goal.GetElapsedTime();

        // 自身にアタッチされたTMP_Textを取得
        scoreText = GetComponent<TMP_Text>();

        if (scoreText == null) {
            Debug.LogError("ScoreManager: TMP_Textが見つかりません。正しいコンポーネントをアタッチしてください。");
        }
    }

    /// <summary>
    /// スコアを計算
    /// </summary>
    private void CalculateScore() {
        int lanternScore = CalculateLanternScore();
        int timeScore = CalculateTimeScore();
        score = lanternScore + timeScore;
    }

    /// <summary>
    /// ランタンスコアを計算
    /// maxLanternScore からランタン数 × ペナルティを引いて算出し、0未満にはしない
    /// </summary>
    /// <returns>計算されたランタンスコア</returns>
    private int CalculateLanternScore() {
        int lanternScore = maxLanternScore - (lanternCount * lanternPenalty);
        return Mathf.Max(0, lanternScore);
    }

    /// <summary>
    /// タイムスコアを計算
    /// maxTimeScore から (クリア時間 × ペナルティ) を引いて算出し、0未満にはしない
    /// </summary>
    /// <returns>計算されたタイムスコア</returns>
    private int CalculateTimeScore() {
        float timeScore = maxTimeScore - (clearTime * timePenalty);
        return Mathf.Max(0, (int)timeScore);
    }

    /// <summary>
    /// スコアをサーバーに送信
    /// </summary>
    private void SendScoreToServer() {
        if (UnityroomApiClient.Instance == null) {
            Debug.LogError("UnityroomApiClientが初期化されていません。スコア送信に失敗しました。");
            return;
        }

        UnityroomApiClient.Instance.SendScore(1, score, ScoreboardWriteMode.HighScoreDesc);
    }

    /// <summary>
    /// スコア表示を更新
    /// </summary>
    private void UpdateScoreText() {
        if (scoreText != null) {
            scoreText.text = score.ToString("#,0");
        }
    }

    /// <summary>
    /// 現在のスコアを取得
    /// </summary>
    /// <returns>計算されたスコア</returns>
    public int GetScore() {
        return score;
    }
}
