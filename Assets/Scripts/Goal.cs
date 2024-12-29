using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    private static int lanternCount = 0;
    private static float startTime;

    private void Start() {
        startTime = Time.time;
    }

    public static void AddLanternCount() {
        lanternCount++;
    }

    public static int GetLanternCount()
    {
        return lanternCount;
    }

    public static float GetElapsedTime() {
        return Time.time - startTime;
    }
}