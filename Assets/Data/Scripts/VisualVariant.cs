using UnityEngine;

[System.Serializable]
public class VisualVariant
{
    public string Id;                    // "Default", "Camo1", "Camo2" など
    public Material OverrideMaterial;    // 任意（nullなら無変更）
    public Texture OverrideTexture;      // 任意
}