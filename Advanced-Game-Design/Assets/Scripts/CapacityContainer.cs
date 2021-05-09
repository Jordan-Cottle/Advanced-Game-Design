using UnityEngine;
public class CapacityContainer : MonoBehaviour
{
    [SerializeField]
    private float _maxCapacity;

    public float CurrentCapacity { get; protected set; }
    public float MaxCapacity { get => _maxCapacity; }

    protected void Start()
    {
        CurrentCapacity = MaxCapacity;
    }
}
