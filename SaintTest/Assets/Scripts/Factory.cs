using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField] private StorageBase _inputStorage;
    [SerializeField] private StorageBase _outputStorage;
    [SerializeField] private Recipe _recipe;

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

            if (_recipe.Ingredients.Count > 0 && GetIngredientsFromStock(out List<ResourceView> ingredients))
            {
                DestroyIngredients(ingredients);
            }

            yield return new WaitForSeconds(_recipe.ProductionTime);

            ResourceModel itemModel = new ResourceModel()
            {
                Type = _recipe.Result.Type,
                Amount = _recipe.Result.Amount,
            };
            
            var item = InstanceCreator.Instance.CreateObject(itemModel.Type);
            _outputStorage.TryAdd(item);
        }
    }

    private bool GetIngredientsFromStock(out List<ResourceView> ingredients)
    {
        ingredients = new List<ResourceView>();
        foreach (var ingredient in _recipe.Ingredients)
        {
            if (_inputStorage.TryGet(ingredient, out List<ResourceView> items))
            {
                ingredients.AddRange(items);
                continue;
            }
            
            return false;
        }
        
        return true;
    }

    private void DestroyIngredients(List<ResourceView> ingredients)
    {
        foreach (var ingredient in ingredients)
        {
            ingredient.Destroy();
        }
    }

    private bool CanProduce()
    {
        if (!_outputStorage.CanAdd(_recipe.Result.Type))
            return false;

        if (_recipe.Ingredients.Count > 0 && !_inputStorage.HasItems(_recipe.Ingredients))
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