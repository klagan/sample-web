namespace Sample.WebApi.Shaping.Controllers
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Reflection;

    public static class ObjectExtensions
    {
        public static ExpandoObject Shape<TSource>(
            this TSource source,
            string[] fields
        )
        {
            var result = new ExpandoObject();

            var propertyInfoList = new List<PropertyInfo>();

            if (!fields.Any())
            {
                var propertyInfo = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                propertyInfoList.AddRange(propertyInfo);
            }
            else
            {
                foreach (var field in fields)
                {
                    var propertyName = field.Trim();

                    var propertyInfo = typeof(TSource).GetProperty(propertyName,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                    if (propertyInfo == null)
                    {
                        throw new KeyNotFoundException(nameof(propertyName));
                    }

                    propertyInfoList.Add(propertyInfo);
                }
            }

            foreach (var propertyInfo in propertyInfoList)
            {
                var value = propertyInfo.GetValue(source);

                ((IDictionary<string, object>) result).Add(propertyInfo.Name, value);
            }

            return result;
        }

        public static IList<ExpandoObject> Shape<TSource>(
            this IEnumerable<TSource> source,
            string[] fields
        )
        {
            return source.Select(entity => entity.Shape(fields)).ToList();
        }

        public static IList<ExpandoObject> Shape<TSource>(
            this IList<TSource> source,
            string[] fields
        )
        {
            return source.AsEnumerable().Shape(fields);
        }

        public static IList<ExpandoObject> Shape<TSource>(
            this TSource[] source,
            string[] fields
        )
        {
            return source.AsEnumerable().Shape(fields);
        }

        public static IList<ExpandoObject> Shape<TSource>(
            this List<TSource> source,
            string[] fields
        )
        {
            return source.AsEnumerable().Shape(fields);
        }
    }
}
