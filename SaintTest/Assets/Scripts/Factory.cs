using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    [SerializeField] private StorageBase _inputStorage;
    [SerializeField] private StorageBase _outputStorage;
    [SerializeField] private Recipe _recipe;

    [SerializeField] private WorkStatus _workStatus;

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
                yield return new WaitForSeconds(_recipe.ProductionTime);
                continue;
            }

            if (_recipe.Ingredients.Count > 0)
            {
                if(GetIngredientsFromStock(out List<ResourceView> ingredients))
                    DestroyIngredients(ingredients);
                else
                {
                    _workStatus = WorkStatus.NonResources;
                    yield return new WaitForSeconds(_recipe.ProductionTime);
                    continue;
                }
            }

            yield return new WaitForSeconds(_recipe.ProductionTime);

            ResourceModel itemModel = new ResourceModel(_recipe.Result.Type, _recipe.Result.Amount);
            
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
        {
            OnDone();
            return false;
        }

        if (_recipe.Ingredients.Count > 0 && !_inputStorage.HasItems(_recipe.Ingredients))
        {
            OnNonResources();
            return false;
        }

        _workStatus = WorkStatus.Working;
        return true;
    }

    private void OnNonResources()
    {
        if (_workStatus != WorkStatus.NonResources)
        {
            Debug.LogError($"No input resources for {gameObject.name}");
        }
        
        _workStatus = WorkStatus.NonResources;
    }

    private void OnDone()
    {
        if (_workStatus != WorkStatus.Done)
        {
            Debug.LogError($"Output storage full for {gameObject.name}");
        }
        
        _workStatus = WorkStatus.Done;
    }
}

public enum WorkStatus
{
    Working,
    Done,
    NonResources
}