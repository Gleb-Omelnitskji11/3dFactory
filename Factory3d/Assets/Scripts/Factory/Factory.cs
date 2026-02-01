using System.Collections;
using System.Collections.Generic;
using Game.Core;
using Game.Resources;
using Game.Storage;
using UnityEngine;

namespace Game.Factory
{
    public class Factory : MonoBehaviour
    {
        [SerializeField] private StorageBase _inputStorage;
        [SerializeField] private StorageBase _outputStorage;
        [SerializeField] private Recipe _recipe;
        [SerializeField] private Transform _inputPoint;
        [SerializeField] private Transform _outputPoint;

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

                List<ResourceView> ingredients = new List<ResourceView>();
                if (_recipe.Ingredients.Count > 0 && !TryGetIngredientsFromStock(out ingredients))
                {
                    _workStatus = WorkStatus.NonResources;
                    yield return new WaitForSeconds(_recipe.ProductionTime);
                    continue;
                }

                yield return StartCoroutine(TakeIngredients(ingredients));
                yield return new WaitForSeconds(_recipe.ProductionTime);
                ResourceModel itemModel = new ResourceModel(_recipe.Result.Type, _recipe.Result.Amount);

                var item = ResourcePooledCreator.Instance.GetObject(itemModel.Type);
                item.transform.position = _outputPoint.position;
                _outputStorage.TryAdd(item);
            }
        }

        private bool TryGetIngredientsFromStock(out List<ResourceView> ingredients)
        {
            ingredients = new List<ResourceView>();
            foreach (var ingredient in _recipe.Ingredients)
            {
                if (_inputStorage.TryGetWithoutRemoving(ingredient, out List<ResourceView> items))
                {
                    ingredients.AddRange(items);
                    continue;
                }

                return false;
            }

            _inputStorage.RemoveItems(ingredients);
            return true;
        }

        private IEnumerator TakeIngredients(List<ResourceView> ingredients)
        {
            foreach (var ingredient in ingredients)
            {
                ingredient.transform.SetParent(transform);
                Vector3 oldPos = ingredient.transform.localPosition;
                ingredient.gameObject.SetActive(true);
                ResourceTweenHelper.Move(ingredient.transform, oldPos, _inputPoint.localPosition);

                yield return new WaitForEndOfFrame();
            }
            
            yield return new WaitForSeconds(ResourceTweenHelper.ItemMovingDelay);

            foreach (var ingredient in ingredients)
            {
                ingredient.TurnOff();
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
                Debug.Log($"No input resources for {gameObject.name}");
            }

            _workStatus = WorkStatus.NonResources;
        }

        private void OnDone()
        {
            if (_workStatus != WorkStatus.Done)
            {
                Debug.Log($"Output storage full for {gameObject.name}");
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
}
