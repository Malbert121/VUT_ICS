using System.Collections.ObjectModel;

namespace ICS.BL;

public static class ObservableCollectionExtension
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> values)
    => new(values);
}
