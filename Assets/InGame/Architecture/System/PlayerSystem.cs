using UnityEngine;

public class PlayerSystem : ASystem, IOnUpdate, IOnFixedUpdate
{
    public bool IsActiveForUpdate => true;
    public override void OnSetUp()
    {
        IAnimationHandler playerAnimHandler = gameStat.player.GetComponent<PlayerAnimationHandler>();

        gameStat.player.OnSetUp(playerAnimHandler);
        gameStat.player.Equip(gameStat.playerEquipWeapon);
    }

    public void OnUpdate()
    {
        gameStat.player.Rotate(gameStat.worldPosition);
    }

    public void OnFixedUpdate()
    {
        gameStat.player.Move(gameStat.moveDirection);
        //gameStat.player.Rotate(gameStat.moveDirection);
    }
}
