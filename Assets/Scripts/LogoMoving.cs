using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoMoving : MonoBehaviour {
    private RawImage rawImage;
    [SerializeField] private float amplitude = 50f; // �U��
    [SerializeField] private float frequency = 1f;  // ���g��

    private Vector3 startPos;

    void Start() {
        rawImage = GetComponent<RawImage>();
        startPos = rawImage.transform.localPosition;
    }

    void Update() {
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        rawImage.transform.localPosition = new Vector3(startPos.x, newY, startPos.z);
    }
}