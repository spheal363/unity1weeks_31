using UnityEngine;
using TMPro;
using System.Collections;

public class TextBlinking : MonoBehaviour {
    private TMP_Text tmpText;        // 点滅させたいTextMeshProのオブジェクト
    public float blinkDuration = 0.5f;  // 点滅の周期（秒）
    private Coroutine blinkCoroutine;

    void Start() {
        tmpText = GetComponent<TMP_Text>();
        StartBlinking();
    }

    public void StartBlinking() {
        if (blinkCoroutine != null) {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(BlinkGradientCoroutine());
    }

    IEnumerator BlinkGradientCoroutine() {
        float elapsedTime = 0f;

        while (true) {
            elapsedTime += Time.deltaTime;
            float alpha = (Mathf.Sin((elapsedTime / blinkDuration) * Mathf.PI * 2) + 1) / 2; // サイン波を0～1の範囲に正規化
            Color color = tmpText.color;
            color.a = alpha;  // アルファ値を設定
            tmpText.color = color;

            yield return null; // フレームごとに更新
        }
    }
}