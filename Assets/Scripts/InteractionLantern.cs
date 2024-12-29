using System;
using UnityEngine;
using static MapGenerator;

public class InteractionLantern : MonoBehaviour {
    [SerializeField] private GameObject lanternPrefab;
    [SerializeField] private float lanternCount = 2;
    [SerializeField] private MapGenerator mapGenerator;

    private Player player;

    // ランタン破壊要求を通知するイベント
    public static event Action<Vector2Int> OnLanternDestroyRequested;

    private void Start() {
        player = GetComponent<Player>();
    }

    public void PlaceLantern() {
        // 現在のプレイヤー位置を基準にランタン設置/破壊
        Vector2Int lanternPos = player.currentPos;

        // 新規ランタンの設置
        if (lanternCount > 0 && (mapGenerator.GetNextMapType(lanternPos) == MAP_TYPE.GROUND)) {

            GameObject lanternObj = Instantiate(
                lanternPrefab,
                mapGenerator.ScreenPos(lanternPos),
                Quaternion.identity,
                mapGenerator.transform
            );
            lanternObj.GetComponent<Lantern>().currentPos = lanternPos;
            mapGenerator.SetMapType(lanternPos, MAP_TYPE.LANTERN);
            lanternCount--;
        }
        // 既にランタンがあるなら回収 (破壊リクエストを投げる)
        else if(mapGenerator.GetNextMapType(lanternPos) == MAP_TYPE.LANTERN) {
            mapGenerator.SetMapType(lanternPos, MAP_TYPE.GROUND);
            lanternCount++;

            OnLanternDestroyRequested?.Invoke(lanternPos);
        }
    }
}