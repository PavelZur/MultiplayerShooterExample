using Cysharp.Threading.Tasks;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public async UniTask BulletFlight(Vector3 start, Vector3 target, float speed)
    {
        transform.position = start;

        while (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            await UniTask.Yield();
        }

        HideBullet();
    }

    private void HideBullet()
    {
        gameObject.SetActive(false);
    }
}
