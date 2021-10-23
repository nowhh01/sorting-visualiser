using SortingVisualiser.Commands;
using System;
using System.Collections.Generic;

namespace SortingVisualiser
{
    public partial class Algorithm
    {
        public static IEnumerator<ICommand> ShellSort(
            IList<int> numbers,
            Func<int, int, ICommand> changeComparedIndices,
            Func<int, int, ICommand> swapIndices,
            Func<ICommand> markAllAsSortedIndex
            )
        {
            int count = numbers.Count;
            
            for (int gap = count / 2; gap > 0; gap /= 2)
            {
                for (int i = gap; i < count; ++i)
                {
                    int j = i;

                    for (; j >= gap; j -= gap)
                    {
                        int comparedIndex = j - gap;

                        yield return changeComparedIndices(comparedIndex, j);

                        if (numbers[comparedIndex] > numbers[j])
                        {
                            yield return swapIndices(comparedIndex, j);
                        }
                    }
                }
            }

            yield return markAllAsSortedIndex();
        }
    }
}
