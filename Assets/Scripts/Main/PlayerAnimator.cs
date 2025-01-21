using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <see cref="https://dlod.hatenablog.com/entry/2018/07/28/225247">Unity2D　見下ろし型画面で4方向に歩かせる</see>

public class PlayerAnimator : MonoBehaviour {
    private Animator animator;
    private Player player;
    private Vector2 moveDirection;

    private const string IS_WALKING = "IS_WALKING";
    private const string X_PARAM = "X";
    private const string Y_PARAM = "Y";

    /// <summary>
    /// 初期化処理
    /// 必要なコンポーネントを取得
    /// </summary>
    private void Awake() {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();

        if (animator == null) {
            Debug.LogError("Animatorが見つかりません。");
        }

        if (player == null) {
            Debug.LogError("Playerコンポーネントが見つかりません。");
        }

        moveDirection = Vector2.right; // デフォルトの方向を設定
    }

    /// <summary>
    /// フレームごとの更新処理
    /// プレイヤーの移動方向とアニメーションの状態を更新
    /// </summary>
    private void Update() {
        UpdateMoveDirection();
        UpdateAnimatorParameters();
    }

    /// <summary>
    /// プレイヤーの移動方向を更新
    /// </summary>
    private void UpdateMoveDirection() {
        if (player == null) return;
        moveDirection = player.GetMoveDir();
    }

    /// <summary>
    /// アニメーターのパラメータを更新
    /// </summary>
    private void UpdateAnimatorParameters() {
        if (animator == null || player == null) return;

        animator.SetBool(IS_WALKING, player.GetIsWalking());

        if (moveDirection != Vector2.zero) {
            animator.SetFloat(X_PARAM, moveDirection.x);
            animator.SetFloat(Y_PARAM, moveDirection.y);
        }
    }
}