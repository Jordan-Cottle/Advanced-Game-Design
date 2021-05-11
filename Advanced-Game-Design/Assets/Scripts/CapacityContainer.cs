using UnityEngine;
public class CapacityContainer : MonoBehaviour
{
    [SerializeField]
    private float _maxCapacity;
    private float _currentCapacity;


    public float MaxCapacity { get => _maxCapacity; }
    public float CurrentCapacity
    {
        get => _currentCapacity;
        protected set => _currentCapacity = Mathf.Clamp(value, 0, MaxCapacity);
    }

    public bool Full => CurrentCapacity == MaxCapacity;
    public bool Empty => CurrentCapacity == 0;

    protected void Start()
    {
        CurrentCapacity = MaxCapacity;
    }
}
