/**
 * JsonHelper for Unity
 * 
 * This static helper class provides methods for converting arrays to and from JSON strings in Unity. 
 * Unity's built-in JsonUtility does not natively support serializing and deserializing arrays, so this 
 * class wraps arrays in a container object to facilitate this process.
 * 
 * Key functionalities provided by this class include:
 * - Converting JSON strings to arrays of objects.
 * - Converting arrays of objects to JSON strings.
 * - Supporting pretty-printing for more readable JSON output.
 * 
 * Note:
 * - Ensure that the data structures used with this helper are serializable by Unity's JsonUtility.
 */

using System;
using UnityEngine;

/// <summary>
/// Provides methods for converting arrays to and from JSON strings using Unity's JsonUtility.
/// </summary>
public static class JsonHelper
{
    /// <summary>
    /// Converts a JSON string to an array of objects of type T.
    /// </summary>
    /// <typeparam name="T">The type of objects in the array.</typeparam>
    /// <param name="json">The JSON string to convert.</param>
    /// <returns>An array of objects of type T.</returns>
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(WrapJson(json));
        return wrapper.Items;
    }

    /// <summary>
    /// Converts an array of objects to a JSON string.
    /// </summary>
    /// <typeparam name="T">The type of objects in the array.</typeparam>
    /// <param name="array">The array of objects to convert.</param>
    /// <returns>A JSON string representing the array of objects.</returns>
    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new()
        {
            Items = array
        };
        return JsonUtility.ToJson(wrapper);
    }

    /// <summary>
    /// Converts an array of objects to a pretty-printed JSON string.
    /// </summary>
    /// <typeparam name="T">The type of objects in the array.</typeparam>
    /// <param name="array">The array of objects to convert.</param>
    /// <param name="prettyPrint">Whether to format the JSON string with indentation for readability.</param>
    /// <returns>A pretty-printed JSON string representing the array of objects.</returns>
    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new()
        {
            Items = array
        };
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    /// <summary>
    /// Wraps a JSON string in a container object to facilitate array serialization.
    /// </summary>
    /// <param name="json">The JSON string to wrap.</param>
    /// <returns>A JSON string wrapped in a container object.</returns>
    private static string WrapJson(string json)
    {
        return "{\"Items\":" + json + "}";
    }

    /// <summary>
    /// A container class used to wrap arrays for JSON serialization.
    /// </summary>
    /// <typeparam name="T">The type of objects in the array.</typeparam>
    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
