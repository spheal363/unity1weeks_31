using UnityEngine;

public class Lantern : MapObject {

    private void Start() {
        InteractionLantern.OnLanternDestroyRequested += HandleLanternDestroyRequest;
    }

    private void HandleLanternDestroyRequest(Vector2Int pos)
    {
        if (pos == currentPos) {
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        InteractionLantern.OnLanternDestroyRequested -= HandleLanternDestroyRequest;
    }
}