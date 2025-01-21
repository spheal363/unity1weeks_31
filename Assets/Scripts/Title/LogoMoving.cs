using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoMoving : MonoBehaviour {
    // ロゴのRawImageを格納
    private RawImage rawImage;

    // 振幅
    [SerializeField] private float amplitude = 50f;
    // 周波数
    [SerializeField] private float frequency = 1f;

    // ロゴの初期位置
    private Vector3 startPos;

    private void Start() {
        InitializeComponents();
    }

    private void Update() {
        AnimateLogoPosition();
    }

    /// <summary>
    /// 必要なコンポーネントを初期化
    /// </summary>
    private void InitializeComponents() {
        rawImage = GetComponent<RawImage>();
        startPos = rawImage.transform.localPosition;
    }

    /// <summary>
    /// ロゴの位置を上下に動かすアニメーション
    /// </summary>
    private void AnimateLogoPosition() {
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        rawImage.transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}