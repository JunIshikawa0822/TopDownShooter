using UnityEngine;

//アタッチメントの3Dオブジェクトを管理する側
public class AttachmentSocket
{
    [SerializeField] private AttachmentType _type;  // Scope, Barrel, Grip など
    [SerializeField] private Transform _mountPoint; // 実際にPrefabをつける位置
    public AttachmentType Type => _type;

    private GameObject _currentAttachmentInstance;

    public void Attach(GameObject attachmentPrefab)
    {
        //3Dオブジェクトをもらってセットする処理

        // if (_currentAttachmentInstance != null)
        //     GameObject.Destroy(_currentAttachmentInstance);

        // _currentAttachmentInstance = GameObject.Instantiate(attachmentPrefab, _mountPoint);
    }

    public void Detach()
    {
        //3Dオブジェクトを外す処理

        if (_currentAttachmentInstance != null)
            GameObject.Destroy(_currentAttachmentInstance);
    }
}
