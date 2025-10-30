using UnityEngine;

public class PlayerSystem : ASystem, IOnUpdate, IOnFixedUpdate
{
    public override void OnSetUp()
    {
        IAnimationHandler playerAnimHandler = gameStat.player.GetComponent<PlayerAnimationHandler>();

        gameStat.player.OnSetUp(playerAnimHandler);
    }

    public void OnUpdate()
    {

    }

    public void OnFixedUpdate()
    {
        gameStat.player.Move(gameStat.moveDirection);
        gameStat.player.Rotate(gameStat.moveDirection);
    }
}
