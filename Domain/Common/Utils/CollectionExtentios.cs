namespace Domain.Common.Utils;

public static class CollectionExtentios {
    public static bool NotNullOrEmpty<T>(this IEnumerable<T> collection) {
        return collection != null && collection.Any();
    }

    public static bool NullOrEmpty<T>(this IEnumerable<T> collection) {
        return collection is null || !collection.Any();
    }

    public static bool NullOrEmpty<T>(this IList<T> collection) {
        return collection is null || collection.Count == 0;
    }
}
