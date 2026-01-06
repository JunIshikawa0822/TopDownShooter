using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(menuName = "MyGame/Container Data", fileName = "NewContainerData")]
public class ContainerData : ItemData
{
    [Header("容量情報")]
    [SerializeField]private string _containerName;
    //conatinerがいくつのGridBlockから構成されるのか、それぞれGridBlockがどのようなwidth, heightから構成されるのか定義
    [SerializeField]private GridBlockData[] _containerBuild;
    [SerializeField]private VisualTreeAsset _containerAsset;

    public GridBlockData[] ContainerBuild => _containerBuild;
    public VisualTreeAsset ContainerAsset => _containerAsset;
    public string ContainerName => _containerName;
}

[System.Serializable]
public struct GridBlockData
{
    public int width;
    public int height;
}
