using UnityEngine;

[System.Serializable]
public class GameStatus
{
    [Header("Player")]
    public PlayerController player;

    [Header("Inputs")]
    public Vector2 moveDirection = Vector2.zero;
}
