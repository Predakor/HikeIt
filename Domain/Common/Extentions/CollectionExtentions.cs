namespace Domain.Common.Extentions;

public static class CollectionExtentions {
    public static bool NullOrEmpty<T>(this IEnumerable<T> collection) {
        return collection == null || collection.Any();
    }

    public static bool NullOrEmpty<T>(this IList<T> collection) {
        return collection == null || collection.Count == 0;
    }

    public static bool NotNullOrEmpty<T>(this IEnumerable<T> collection) {
        return !NullOrEmpty(collection);
    }

    public static bool NotNullOrEmpty<T>(this IList<T> collection) {
        return !NullOrEmpty(collection);
    }
}
