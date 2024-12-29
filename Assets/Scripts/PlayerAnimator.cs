using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private Player player;
    private Vector2 moveDir;

    private const string IS_WALKING = "IsWalking";

    private void Awake() {
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();

        moveDir = new Vector2(1, 0);
    }

    private void Update()
    {
        moveDir = player.getMoveDir();
        animator.SetBool(IS_WALKING, player.getIsWalking());

        if (moveDir != Vector2.zero) {
            animator.SetFloat("X", moveDir.x);
            animator.SetFloat("Y", moveDir.y);
        }
    }
}
