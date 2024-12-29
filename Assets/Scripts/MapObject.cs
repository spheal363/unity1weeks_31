using UnityEngine;

/// <summary>
/// マップ上に存在するオブジェクトの基底クラス
/// </summary>
public abstract class MapObject : MonoBehaviour {
    // Player と Lantern で共通して使いたい MapGenerator を保持させる
    [SerializeField] protected MapGenerator mapGenerator;
    public Vector2Int currentPos { get; set; }

    // 共通の初期化処理:
    //   例) 指定した座標にオブジェクトを置くときに使う
    public virtual void Initialize(Vector2Int startPos) {
        currentPos = startPos;
        if (mapGenerator != null) {
            transform.localPosition = mapGenerator.ScreenPos(startPos);
        }
    }
}