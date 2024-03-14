using System.Collections.ObjectModel;

namespace ICS.BL.Mappers
{
    public static class ObservableCollectionExtension
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> values)
        => new(values);
    }
}