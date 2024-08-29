using System.Reflection;

namespace FamilyHubs.OR.Umbraco;

public static class TypeExplorer
{
    /// <summary>
    /// Recursively gets all instances of a given type within an object.
    /// </summary>
    /// <typeparam name="TTypeToFind">The type to find.</typeparam>
    /// <param name="sourceObject">The source object in which to find.</param>
    public static IEnumerable<TTypeToFind> GetAllNestedPropertiesOfType<TTypeToFind>(object? sourceObject) where TTypeToFind : class
    {
        if (sourceObject is null)
            yield break;

        Stack<object> stack = new();
        HashSet<object> visited = [];

        stack.Push(sourceObject);

        while (stack.Count > 0)
        {
            object? currentObject = stack.Pop();

            if (currentObject is TTypeToFind currObjAsT)
                yield return currObjAsT;

            if (!visited.Add(currentObject))
                continue;

            PropertyInfo[] properties = currentObject.GetType().GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object? value = property.GetValue(currentObject);

                if (value is TTypeToFind currPropAsT)
                    yield return currPropAsT;

                if (value is null ||
                    visited.Contains(value) ||
                    property.PropertyType.IsPrimitive ||
                    property.PropertyType.IsEnum ||
                    property.PropertyType == typeof(string) ||
                    property.PropertyType == typeof(DateTime))
                    continue;

                if (value is not IEnumerable<object> enumerable)
                {
                    stack.Push(value);
                }
                else
                {
                    foreach (object item in enumerable)
                    {
                        if (visited.Contains(item))
                            continue;
                            
                        stack.Push(item);
                    }
                }
            }
        }
    }
}
