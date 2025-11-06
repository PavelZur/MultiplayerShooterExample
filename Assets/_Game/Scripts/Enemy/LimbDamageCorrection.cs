using UnityEngine;

public class LimbDamageCorrection : MonoBehaviour
{
    [SerializeField] private Health _health;
    public string SessionId { get => _health.SessionID;}
    [field: SerializeField] public float MultipleFactorDamage { get; private set; } = 1;
    [field:SerializeField] public bool IsHead { get; private set; }
}
