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

        public static IEnumerator<ICommand> InsertionSort(
            IList<int> numbers,
            Func<int, int, ICommand> changeComparedIndices,
            Func<int, int, ICommand> swapIndices,
            Func<int, ICommand> markAsSortedIndex
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

                yield return markAsSortedIndex(i - 1);
            }

            yield return markAsSortedIndex(i - 1);
        }
    }
}
