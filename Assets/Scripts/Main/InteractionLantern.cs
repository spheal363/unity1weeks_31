using System;
using UnityEngine;
using static MapGenerator;

public class InteractionLantern : MonoBehaviour {
    [SerializeField] private GameObject lanternPrefab;
    [SerializeField] private float lanternCount = 2;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private AudioSource settingLanternSE;

    private Player player;

    // ランタン破壊要求を通知するイベント
    public static event Action<Vector2Int> OnLanternDestroyRequested;

    /// <summary>
    /// 初期化処理
    /// 必要なコンポーネントを取得
    /// </summary>
    private void Start() {
        player = GetComponent<Player>();
    }

    /// <summary>
    /// ランタンを設置または破壊
    /// </summary>
    public void PlaceLantern() {
        if (player == null || mapGenerator == null || settingLanternSE == null) {
            Debug.LogWarning("必要なコンポーネントが揃っていないため、処理を中断します。");
            return;
        }

        Vector2Int lanternPosition = player.currentPos;

        if (CanPlaceLantern(lanternPosition)) {
            PlaceNewLantern(lanternPosition);
        } else if (CanDestroyLantern(lanternPosition)) {
            DestroyExistingLantern(lanternPosition);
        }
    }

    /// <summary>
    /// ランタンを設置可能か判定
    /// </summary>
    private bool CanPlaceLantern(Vector2Int position) {
        return lanternCount > 0 && mapGenerator.GetNextMapType(position) == MAP_TYPE.GROUND;
    }

    /// <summary>
    /// ランタンを破壊可能か判定
    /// </summary>
    private bool CanDestroyLantern(Vector2Int position) {
        return mapGenerator.GetNextMapType(position) == MAP_TYPE.LANTERN;
    }

    /// <summary>
    /// 新しいランタンを設置
    /// </summary>
    private void PlaceNewLantern(Vector2Int position) {
        GameObject lanternObject = Instantiate(
            lanternPrefab,
            mapGenerator.ConvertToScreenPosition(position),
            Quaternion.identity,
            mapGenerator.transform
        );

        Lantern lantern = lanternObject.GetComponent<Lantern>();
        if (lantern != null) {
            lantern.currentPos = position;
        }

        mapGenerator.SetMapType(position, MAP_TYPE.LANTERN);
        lanternCount--;
        Goal.AddLanternCount();
        settingLanternSE.Play();
    }

    /// <summary>
    /// 既存のランタンを破壊
    /// </summary>
    private void DestroyExistingLantern(Vector2Int position) {
        mapGenerator.SetMapType(position, MAP_TYPE.GROUND);
        lanternCount++;

        OnLanternDestroyRequested?.Invoke(position);
        settingLanternSE.Play();
    }
}