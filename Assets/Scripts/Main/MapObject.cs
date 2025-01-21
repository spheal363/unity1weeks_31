using UnityEngine;

/// <summary>
/// マップ上に存在するオブジェクトの基底クラス
/// </summary>
public abstract class MapObject : MonoBehaviour {
    // PlayerとLanternで共通して使いたいMapGeneratorを保持
    [SerializeField] protected MapGenerator mapGenerator;

    // 現在の座標を保持
    public Vector2Int currentPos { get; set; }

    /// <summary>
    /// オブジェクトの初期化処理を行う
    /// 指定した座標にオブジェクトを配置
    /// </summary>
    /// <param name="startPos">初期位置の座標</param>
    public virtual void Initialize(Vector2Int startPos) {
        currentPos = startPos;

        if (mapGenerator == null) {
            Debug.LogError("MapGenerator が設定されていません。");
            return;
        }

        transform.localPosition = mapGenerator.ConvertToScreenPosition(startPos);
    }
}