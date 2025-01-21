using UnityEngine;
using TMPro;
using System.Collections;

public class TextBlinking : MonoBehaviour {
    // 点滅させたいTextMeshProのオブジェクト
    private TMP_Text tmpText;
    // 点滅の周期（秒）
    [SerializeField] private float blinkDuration = 0.5f;
    private Coroutine blinkCoroutine;

    private void Start() {
        InitializeTextComponent();
        StartBlinking();
    }

    /// <summary>
    /// TMP_Textコンポーネントを初期化
    /// </summary>
    private void InitializeTextComponent() {
        tmpText = GetComponent<TMP_Text>();
        if (tmpText == null) {
            Debug.LogError("TMP_Text コンポーネントがアタッチされていません。");
        }
    }

    /// <summary>
    /// 点滅を開始
    /// </summary>
    public void StartBlinking() {
        if (blinkCoroutine != null) {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(BlinkGradientCoroutine());
    }

    /// <summary>
    /// 点滅処理のコルーチン
    /// </summary>
    private IEnumerator BlinkGradientCoroutine() {
        float elapsedTime = 0f;

        while (true) {
            elapsedTime += Time.deltaTime;
            float alpha = (Mathf.Sin((elapsedTime / blinkDuration) * Mathf.PI * 2) + 1) / 2; // sin波を0～1の範囲に正規化

            if (tmpText != null) {
                Color color = tmpText.color;
                color.a = alpha; // アルファ値を設定
                tmpText.color = color;
            } else {
                Debug.LogWarning("TMP_Text が null のため、点滅処理を停止します。");
                yield break;
            }

            yield return null; // フレームごとに更新
        }
    }
}