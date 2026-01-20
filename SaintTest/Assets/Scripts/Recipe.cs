using System;
using System.Collections.Generic;

[Serializable]
public class Recipe
{
    public List<ResourceModel> Ingredients = new List<ResourceModel>();
    public ResourceModel Result;
    public float ProductionTime;
}