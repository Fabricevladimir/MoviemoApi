using System;

namespace Moviemo.API.Extensions
{
    public static class Utils
    {
        public static string NotFoundMessage (Type entity, string property, object value)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentException("message", nameof(property));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return $"{entity.Name} with {property} {value.ToString()} was not found.";
        }

        public static string ConflictMessage (Type entity, string property, object value)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            if (string.IsNullOrEmpty(property))
            {
                throw new ArgumentException("message", nameof(property));
            }

            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return $"{entity.Name} with {property} {value.ToString()} already exists.";
        }

        public const string ResourceEndpointIdError = "Resource id and id from endpoint do not match.";
    }
}
