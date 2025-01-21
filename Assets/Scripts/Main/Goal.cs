using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    // スコア計算に使用
    // ランタンを設置した回数
    private static int lanternCount = 0;
    // ゲーム開始時刻
    private static float startTime;

    /// <summary>
    /// 初期化処理で、開始時刻を設定
    /// </summary>
    private void Start() {
        InitializeStartTime();
    }

    /// <summary>
    /// ランタンの設置カウントを1増やす
    /// </summary>
    public static void AddLanternCount() {
        lanternCount++;
    }

    /// <summary>
    /// 現在のランタンの設置カウントを取得
    /// </summary>
    public static int GetLanternCount() {
        return lanternCount;
    }

    /// <summary>
    /// ゲーム開始時刻からの経過時間を取得
    /// </summary>
    public static float GetElapsedTime() {
        return Time.time - startTime;
    }

    /// <summary>
    /// 開始時刻を初期化
    /// </summary>
    private static void InitializeStartTime() {
        startTime = Time.time;
    }
}