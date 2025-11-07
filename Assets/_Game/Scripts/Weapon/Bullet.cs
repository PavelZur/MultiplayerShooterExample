using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private List<DecalOfType> _decalOfTypesList = new();
    [SerializeField] private GameObject _viewMesh;
    public async UniTask BulletFlight(Vector3 start, Vector3 target, float speed, DecalBulletType decalBulletType ,Vector3 normal)
    {
        transform.position = start;

        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            await UniTask.Yield();
        }

        HideBullet(decalBulletType, normal);
    }

    private void HideBullet(DecalBulletType decalBulletType, Vector3 normal)
    {
        _viewMesh.SetActive(false);
        EffectDecalOfDestoy(decalBulletType, normal);
    }

    private void EffectDecalOfDestoy(DecalBulletType decalBulletType, Vector3 normal)
    {
        if (decalBulletType == DecalBulletType.None) return;

        foreach (var key in _decalOfTypesList)
        {
            if (key.decalBulletType == decalBulletType)
            {
                key.particleObj.transform.rotation = Quaternion.LookRotation(normal);
                key.particleObj.SetActive(true);
            }
        }
    }

    private void OnEnable()
    {
        foreach (var key in _decalOfTypesList)
        {
            key.particleObj.SetActive(false);
        }
    }

    private void OnDisable()
    {
        foreach (var key in _decalOfTypesList)
        {
            key.particleObj.SetActive(false);
        }
    }
}

[System.Serializable]
public struct DecalOfType
{
    public DecalBulletType decalBulletType;
    public GameObject particleObj;
}
