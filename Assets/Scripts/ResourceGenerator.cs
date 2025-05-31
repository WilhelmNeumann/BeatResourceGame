using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class ResourceGenerator
{
    private static System.Random _random = new System.Random();

    public static List<ResourceType> GenerateResources(int number)
    {
        var enumValues = Enum.GetValues(typeof(ResourceType)).Cast<ResourceType>().ToList();

        if (number < enumValues.Count)
            throw new ArgumentException("Number must be at least equal to the number of ResourceType values.");

        var result = new List<ResourceType>(enumValues);

        while (result.Count < number)
        {
            var randomResource = enumValues[_random.Next(enumValues.Count)];
            result.Add(randomResource);
        }

        // Shuffle the list
        for (int i = result.Count - 1; i > 0; i--)
        {
            int j = _random.Next(i + 1);
            (result[i], result[j]) = (result[j], result[i]);
        }

        return result;
    }
}
