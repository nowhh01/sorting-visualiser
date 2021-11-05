using SortingVisualiser.Commands;
using System;
using System.Collections.Generic;

namespace SortingVisualiser
{
    public partial class Algorithm
    {
        public static IEnumerator<ICommand> InsertionSort(
            IList<int> numbers,
            Func<int, int, ICommand> changeComparedIndices,
            Func<int, int, ICommand> swapIndices,
            Func<ICommand> markAllAsSortedIndex
            )
        {
            int i = 1;
            for (; i < numbers.Count; i++)
            {
                int comparedIndex = i;

                for (int j = i - 1; j >= 0; j--)
                {
                    yield return changeComparedIndices(j, comparedIndex);

                    if (numbers[comparedIndex] < numbers[j])
                    {
                        yield return swapIndices(j, comparedIndex);

                        comparedIndex = j;
                    }
                    else
                    {
                        break;
                    }
                }

            }

            yield return markAllAsSortedIndex();
        }
    }
}
