using System.Collections.Generic;

namespace SortingVisualiser.Extensions
{
    public static class IListExtensions
    {
        public static void Swap<T>(this IList<T> array, int index1, int index2)
        {
            T temp = array[index1];
            array[index1] = array[index2];
            array[index2] = temp;
        }
    }
}
