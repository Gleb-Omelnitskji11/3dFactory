using System;
using System.Collections.Generic;
using Game.Resources;
using UnityEngine;

namespace Game.Factory
{
    [Serializable]
    public class Recipe
    {
        [SerializeField] private List<ResourceModel> _ingredients = new ();
        [SerializeField] private ResourceModel _result;
        [SerializeField] private float _productionTime;
        
        public float ProductionTime => _productionTime;
        public ResourceModel Result => _result;
        public List<ResourceModel> Ingredients => _ingredients;
    }
}
