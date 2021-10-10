using SortingVisualiser.Commands;
using System;
using System.Collections.Generic;

namespace SortingVisualiser
{
    public class Algorithm
    {
        public static IEnumerator<ICommand> BubbleSort(
            IList<int> numbers,
            Func<int, int, ICommand> changeComparedIndices,
            Func<int, int, ICommand> swapIndices,
            Func<int, ICommand> markAsSortedIndex
            )
        {
            int count = numbers.Count;

            for (int i = 0; i < count - 1; i++)
            {
                int j = 0;
                for (; j < count - i - 1; j++)
                {
                    int comparedIndex = j + 1;

                    yield return changeComparedIndices(j, comparedIndex);

                    if (numbers[j] > numbers[comparedIndex])
                    {
                        yield return swapIndices(j, comparedIndex);
                    }
                }

                yield return markAsSortedIndex(j);
            }

            yield return markAsSortedIndex(0);
        }
    }
}
