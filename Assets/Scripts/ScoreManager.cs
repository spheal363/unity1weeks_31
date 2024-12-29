using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour {
    private int lanternCount;
    private float clearTime;

    private int score;
    private TMP_Text scoreText;

    // ランタンスコア関連
    [SerializeField] private int maxLanternScore = 2000;  // ランタン部門の最大スコア
    [SerializeField] private int lanternPenalty = 50;    // ランタン1個あたりのペナルティ

    // タイムスコア関連
    [SerializeField] private int maxTimeScore = 8000;     // タイム部門の最大スコア
    [SerializeField] private float timePenalty = 50f;    // 1秒あたりのペナルティ（大きいほどクリア時間の影響が大きい）

    void Start() {
        // Goalクラスなどからランタン数とクリア時間を取得（例）
        lanternCount = Goal.GetLanternCount();
        clearTime = Goal.GetElapsedTime();

        // 自身にアタッチされたTMP_Textを参照
        scoreText = GetComponent<TMP_Text>();

        // --- 加算方式でスコア計算 ---
        // 1) ランタンスコア
        //    maxLanternScore からランタン数 × ペナルティを引いて算出し、0未満にはしない
        int lanternScore = maxLanternScore - (lanternCount * lanternPenalty);
        lanternScore = Mathf.Max(0, lanternScore);

        // 2) タイムスコア
        //    maxTimeScore から (クリア時間 × ペナルティ) を引いて算出し、0未満にはしない
        float timeScore = maxTimeScore - (clearTime * timePenalty);
        timeScore = Mathf.Max(0f, timeScore);

        // 3) 合計スコアを算出 (intにキャスト)
        score = lanternScore + (int)timeScore;

        // 表示を更新
        UpdateScoreText();
    }

    private void UpdateScoreText() {
        // 3桁区切りで表示（#,0 はカンマ区切り）
        scoreText.text = score.ToString("#,0");
    }
}