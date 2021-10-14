using SortingVisualiser.Commands;
using System;
using System.Collections.Generic;

namespace SortingVisualiser
{
    public partial class Algorithm
    {
        public static IEnumerator<ICommand> SelectionSort(
            IList<int> numbers,
            Func<int, int, ICommand> changeComparedIndices,
            Func<int, int, ICommand> swapIndices,
            Func<int, ICommand> markAsSortedIndex
            )
        {
            int count = numbers.Count;

            for (int i = 0; i < count - 1; i++)
            {
                int leastIndex = i;
                int comparedIndex = i + 1;
                for (; comparedIndex < count; comparedIndex++)
                {
                    yield return changeComparedIndices(leastIndex, comparedIndex);

                    if (numbers[leastIndex] > numbers[comparedIndex])
                    {
                        leastIndex = comparedIndex;
                    }
                }

                if (leastIndex != i)
                {
                    yield return swapIndices(leastIndex, i);
                }

                yield return markAsSortedIndex(i);
            }

            yield return markAsSortedIndex(count - 1);
        }
    }
}
