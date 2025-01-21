using UnityEngine;

public class Lantern : MapObject {
    private void Start() {
        SubscribeToLanternDestroyEvent();
    }

    /// <summary>
    /// ランタンの破壊要求イベントを購読
    /// </summary>
    private void SubscribeToLanternDestroyEvent() {
        InteractionLantern.OnLanternDestroyRequested += HandleLanternDestroyRequest;
    }

    /// <summary>
    /// ランタンの破壊要求を処理
    /// </summary>
    /// <param name="pos">破壊対象の座標</param>
    private void HandleLanternDestroyRequest(Vector2Int pos) {
        if (pos == currentPos) {
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        UnsubscribeFromLanternDestroyEvent();
    }

    /// <summary>
    /// ランタンの破壊要求イベントの購読を解除
    /// </summary>
    private void UnsubscribeFromLanternDestroyEvent() {
        InteractionLantern.OnLanternDestroyRequested -= HandleLanternDestroyRequest;
    }
}