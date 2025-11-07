using UnityEngine;

public class DieEffect : MonoBehaviour
{
    private void OnEnable()
    {
        Destroy(gameObject,3f);
    }
}