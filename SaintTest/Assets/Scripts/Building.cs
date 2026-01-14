using System.Collections;
using UnityEngine;

public class Building : MonoBehaviour
{
    public Storage InputStorage;
    public Storage OutputStorage;

    public ResourceType ProduceType;
    public float ProductionTime;

    [SerializeField] private bool _isWorking;

    private void Start()
    {
        StartCoroutine(ProductionLoop());
    }

    private IEnumerator ProductionLoop()
    {
        while (true)
        {
            if (!CanProduce())
            {
                _isWorking = false;
                UpdateStatus();
                yield return new WaitForSeconds(ProductionTime);
                continue;
            }

            _isWorking = true;
            UpdateStatus();

            yield return new WaitForSeconds(ProductionTime);
            InputStorage?.Remove();

            ResourceModel item = new ResourceModel()
            {
                Type = ProduceType,
                Amount = 1
            };
            OutputStorage.Add(item);
        }
    }

    private bool CanProduce()
    {
        if (OutputStorage != null && !OutputStorage.CanAdd())
            return false;

        if (InputStorage != null && !InputStorage.HasItem())
            return false;

        return true;
    }

    private void UpdateStatus()
    {
        // if (OutputStorage != null && !OutputStorage.CanAdd())
        //     Debug.LogError($"Output storage full for {gameObject.name}");
        // else if (InputStorage != null && !InputStorage.HasItem())
        //     Debug.LogError($"No input resources for {gameObject.name}");
        // else
        //     Debug.Log($"Producing... for {gameObject.name}");
    }
}