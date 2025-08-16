namespace Application.Commons;

public static partial class Extentions {
    public static Uri? ToResoutceUrl<T>(this T? resource, Func<T, string> toPath) {
        return resource is not null
            ? new Uri(toPath(resource), UriKind.Relative)
            : null;
    }
}
