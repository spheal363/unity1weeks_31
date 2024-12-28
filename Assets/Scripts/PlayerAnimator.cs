using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private Player player;
    private float speed = 5.0f;

    private const string IS_WALKING = "IsWalking";

    private void Awake() {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    private void Update()
    {
        Vector2 moveDir = player.getMoveDir();
        animator.SetBool(IS_WALKING, player.getIsWalking());

        if (moveDir != Vector2.zero) {
            animator.SetFloat("X", moveDir.x);
            animator.SetFloat("Y", moveDir.y);
        }

        Debug.Log(moveDir);
    }
}
