using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameInput gameInput;

    private bool isWalking;

    private void Update() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector2 moveDir = new Vector2(inputVector.x, inputVector.y);
        transform.position += (Vector3)moveDir * moveSpeed * Time.deltaTime;

        isWalking = moveDir != Vector2.zero;
    }

    public bool IsWalking() {
        return isWalking;
    }
}
