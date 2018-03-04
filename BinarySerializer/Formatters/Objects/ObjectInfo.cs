using System;
using System.Collections.Generic;
using System.Reflection;
using BinarySerializer.Extensions;

namespace BinarySerializer.Formatters.Objects
{
    internal class ObjectInfo
    {
        public ConstructorInfo Constructor { get; }
        public FieldInfo[] ConstructorFields { get; }
        public FieldInfo[] Fields;

        public ObjectInfo(ConstructorInfo constructor, FieldInfo[] constructorFields, FieldInfo[] fields)
        {
            Constructor = constructor;
            ConstructorFields = constructorFields;
            Fields = fields;
        }

        public static ObjectInfo Create(Type type)
        {
            (var initOnlyFields, var fields) = GetFields(type);

            var ctorAndFields = GetConstructor(type, initOnlyFields);

            if (!ctorAndFields.success)
                return null;

            return new ObjectInfo(ctorAndFields.ctor, ctorAndFields.fields, fields);
        }

        private static (Dictionary<string, FieldInfo> initOnlyFields, FieldInfo[] fields) GetFields(Type type)
        {
            var initOnlyFields = new Dictionary<string, FieldInfo>(StringComparer.OrdinalIgnoreCase);
            var fields = new List<FieldInfo>();
            var currentType = type;

            do
            {
                foreach (var field in currentType.GetFields(BindingFlagsEx.DeclaredInstance))
                {
                    if (field.IsNotSerialized)
                        continue;

                    if (!GenericFormatter.IsSerializableType(field.FieldType))
                        continue;

                    if (field.IsInitOnly)
                    {
                        var fieldOrPropertyName = GetFieldOrPropertyName(field);

                        if (!initOnlyFields.ContainsKey(fieldOrPropertyName))
                            initOnlyFields.Add(fieldOrPropertyName, field);
                    }
                    else
                    {
                        fields.Add(field);
                    }
                }
            } while ((currentType = currentType.BaseType) != null);

            return (initOnlyFields, fields.ToArray());
        }

        private static string GetFieldOrPropertyName(FieldInfo field)
        {
            if (field.Name.StartsWith("<", StringComparison.Ordinal) && field.Name.EndsWith(">k__BackingField", StringComparison.Ordinal))
                return field.Name.Substring("<".Length, field.Name.Length - "<".Length - ">k__BackingField".Length);

            return field.Name;
        }

        private static (ConstructorInfo ctor, FieldInfo[] fields, bool success) GetConstructor(Type type,
                                                                                                             Dictionary<string, FieldInfo> initOnlyFields)
        {
            if (initOnlyFields.Count > 0)
            {
                var visited = new HashSet<FieldInfo>();

                foreach (var ctor in type.GetConstructors())
                {
                    var parameters = ctor.GetParameters();

                    if (!ContainsAllInitOnlyFields(initOnlyFields, visited, parameters))
                        continue;

                    var fields = new FieldInfo[parameters.Length];

                    for (int i = 0; i < parameters.Length; i++)
                        fields[i] = initOnlyFields[parameters[i].Name];

                    return (ctor, fields, true);
                }

            }
            else if (type.IsClass)
            {
                var ctor = type.GetConstructor(BindingFlagsEx.Instance, null, Type.EmptyTypes, null);

                if (ctor != null)
                    return (ctor, Array.Empty<FieldInfo>(), true);
            }
            else
            {
                return (null, Array.Empty<FieldInfo>(), true);
            }

            return (null, default(FieldInfo[]), false);
        }

        private static bool ContainsAllInitOnlyFields(Dictionary<string, FieldInfo> initOnlyFields,
                                                      HashSet<FieldInfo> visited,
                                                      ParameterInfo[] parameters)
        {
            if (parameters.Length != initOnlyFields.Count)
                return false;

            visited.Clear();

            foreach (var parameter in parameters)
            {
                if (!initOnlyFields.TryGetValue(parameter.Name, out var value))
                    return false;

                if (visited.Contains(value))
                    return false;

                if (parameter.ParameterType != value.FieldType)
                    return false;

                visited.Add(value);
            }

            return true;
        }
    }
}