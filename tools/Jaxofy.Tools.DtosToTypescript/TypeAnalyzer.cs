using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IronSphere.Extensions;
using IronSphere.Extensions.Reflection;

namespace Jaxofy.Tools.DtosToTypescript
{
    public interface ITypeAnalyzer
    {
        public IAnalyzedType Analyze();
    }

    public interface IAnalyzedType
    {
        Type WriteToPath(string exportDirectory);
    }

    public class TypeAnalyzer : ITypeAnalyzer, IAnalyzedType
    {
        private IEnumerable<PropertyInfo> _propertyInfos;

        public static ITypeAnalyzer Create(Type type)
        {
            return new TypeAnalyzer(type);
        }

        private readonly Type _type;

        private TypeAnalyzer(Type type)
        {
            _type = type;
        }

        public IAnalyzedType Analyze()
        {
            _propertyInfos = _type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            return this;
        }

        public Type WriteToPath(string exportDirectory)
        {
            string className = _type.GetShortReadableName();

            IList<string> generatedProperties = (
                    from property in _propertyInfos
                    let name = property.Name
                    let type = _getPropertyTypeString(property)
                    select name + ": " + type + ";"
                )
                .ToList();
            
            string generatedClass = $@"// {_type.GetFullReadableName()} as {className}

export interface {className} {{

{string.Join($"\t{Environment.NewLine}", generatedProperties)}

}}
";

            Console.WriteLine(generatedClass);
            return _type;
        }

        private string _getPropertyTypeString(PropertyInfo propertyInfo)
        {
            Type propertyType = propertyInfo.PropertyType;

            bool isNullable = false;
            Type underlyingType = Nullable.GetUnderlyingType(propertyType);

            if (!propertyType.IsValueType)
            {
                isNullable = true;
            }
            else if (underlyingType is not null)
            {
                isNullable = true;
                propertyType = underlyingType;
            }

            string mappedType = TypeMapping.GetValue(propertyType);
            if (mappedType is not null)
            {
                return isNullable ? mappedType + " | null" : mappedType;
            }

            if (propertyType.IsGenericType)
            {
                return "";
            }

            return propertyType.GetShortReadableName();
        }
        
        
        private static readonly Dictionary<Type, string> TypeMapping = new ()
        {
            {typeof(long), "bigint"},
            {typeof(byte), "number"},
            {typeof(short), "number"},
            {typeof(int), "number"},
            {typeof(float), "number"},
            {typeof(decimal), "number"},
            {typeof(double), "number"},
            {typeof(bool), "boolean"},
            {typeof(string), "string"},
            {typeof(DateTime), "date"},
            {typeof(IEnumerable<>), "Array<1>"}
        } ;
    }
}