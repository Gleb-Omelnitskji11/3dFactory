using System;
using System.Collections.Generic;
using UnityEngine;

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