using System;
using System.Collections.Generic;
using System.Reflection;

public class TableFieldMapping
{
    public Dictionary<string, TableFieldProperty> PropertiesMap;

    public TableFieldMapping(Type dataType)
    {
        PropertiesMap = new Dictionary<string, TableFieldProperty>();
        _initPropertiesByType(dataType);
    }

    private void _initPropertiesByType(Type dataType)
    {
        PropertyInfo[] pis = dataType.GetProperties();
        foreach (PropertyInfo pi in pis)
        {
            var property = new TableFieldProperty();
            property.ItemName = pi.Name;
            property.ItemType = pi.PropertyType;
            property.Method = pi.GetSetMethod();
            PropertiesMap[property.ItemName] = property;
        }
    }
}
