using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour {
    private PlayerInputActions playerInputActions;

    private void Awake() {
        InitializePlayerInputActions();
    }

    /// <summary>
    /// PlayerInputActionsを初期化して有効化
    /// </summary>
    private void InitializePlayerInputActions() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    /// <summary>
    /// 正規化された移動ベクトルを取得
    /// </summary>
    /// <returns>正規化されたVector2の移動入力</returns>
    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        return inputVector.normalized;
    }

    /// <summary>
    /// ランタンを設置する入力がトリガーされたかを確認
    /// </summary>
    /// <returns>入力がトリガーされた場合はtrue</returns>
    public bool IsPlaceLanternPressed() {
        return playerInputActions.Player.Interaction.triggered;
    }
}