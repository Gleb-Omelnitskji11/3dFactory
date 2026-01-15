using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public Storage InputStorage;
    public Storage OutputStorage;
    public Recipe _recipe;

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
                yield return new WaitForSeconds(_recipe.ProductionTime);
                continue;
            }

            _isWorking = true;
            UpdateStatus();

            yield return new WaitForSeconds(_recipe.ProductionTime);
            foreach (var ingredient in _recipe.Ingredients)
            {
                InputStorage?.Remove(ingredient.Type);
            }

            ResourceModel item = new ResourceModel()
            {
                Type = _recipe.Result.Type,
                Amount = _recipe.Result.Amount,
            };
            OutputStorage.Add(item);
        }
    }

    private bool CanProduce()
    {
        if (!OutputStorage.CanAdd(_recipe.Result.Type))
            return false;

        if (_recipe.Ingredients.Count > 0 && !InputStorage.HasItems(_recipe.Ingredients))
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

[Serializable]
public class Recipe
{
    public List<ResourceModel> Ingredients = new List<ResourceModel>();
    public ResourceModel Result;
    public float ProductionTime;
}