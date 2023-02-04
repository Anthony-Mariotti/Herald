namespace Herald.Core.Domain.Common.Extensions;

public static partial class ApplicationCommonExtensions
{
    public static IList<T> RemoveLast<T>(this IList<T> list, int count) =>
        list.Reverse().Skip(count).Reverse().ToList();

    public static IEnumerable<T> RemoveLast<T>(this IEnumerable<T> list, int count) =>
        list.Reverse().Skip(count).Reverse();

    public static IList<T> RemoveLast<T>(this IList<T> list, Func<T, bool> predicate, int count)
    {
        var modified = list.ToList();
        var selected = list.Where(predicate).RemoveLast(count);

        _ = modified.RemoveAll(x => selected.Contains(x));
        return modified;
    }
}