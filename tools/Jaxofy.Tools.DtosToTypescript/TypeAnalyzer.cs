using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
        Task WriteToPath(string exportDirectory);
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

        public async Task WriteToPath(string exportDirectory)
        {
            string className = _type.GetShortReadableName();

            IList<string> generatedProperties = (
                    from property in _propertyInfos
                    let name = property.Name
                    let type = _getPropertyTypeString(property)
                    select $"\t{name}: {type};"
                )
                .ToList();
            
            string generatedClass = $@"// {_type.GetFullReadableName()} as {className}

export interface {className} {{
{string.Join($"{Environment.NewLine}", generatedProperties)}
}}
";
            string fileName = _classNameToFileName(className);
            await AsyncFile.WriteTextAsync(Path.Combine(exportDirectory, $"{fileName}.ts"), generatedClass);
        }

        private static string _classNameToFileName(string className)
        {
            IList<char> characters = new List<char>();

            for (int index = 0; index < className.Length; index++)
            {
                char c = className[index];

                if (char.IsUpper(c))
                {
                    c = char.ToLower(c);
                    if (index > 0)
                        characters.Add('-');
                }
                
                characters.Add(c);
            }

            return new(characters.ToArray());
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
            {typeof(IEnumerable<>), "Array<1>"}
        } ;
    }
}