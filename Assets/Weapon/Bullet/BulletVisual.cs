using UnityEngine;
public class BulletVisual : APooledObject
{
    [SerializeField]private TrailRenderer _trailRenderer;

    public virtual void Active()
    {
        _trailRenderer.emitting = true;
    }

    public virtual void Deactive()
    {
        _trailRenderer.emitting = false;
        _trailRenderer.Clear();
    }
}
