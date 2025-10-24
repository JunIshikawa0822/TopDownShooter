using UnityEngine;

public class PlayerSystem : ASystem, IOnUpdate, IOnFixedUpdate
{
    public override void OnSetUp()
    {

    }

    public void OnUpdate()
    {

    }

    public void OnFixedUpdate()
    {
        gameStat.player.Move(gameStat.moveDirection);
    }
}
