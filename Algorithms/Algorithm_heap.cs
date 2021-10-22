using SortingVisualiser.Commands;
using System;
using System.Collections.Generic;

namespace SortingVisualiser
{
    public partial class Algorithm
    {
        public static IEnumerator<ICommand> HeapSort(
            IList<int> numbers,
            Func<int, int, ICommand> changeComparedIndices,
            Func<int, int, ICommand> swapIndices,
            Func<int, ICommand> markAsSortedIndex
            )
        {
            int count = numbers.Count;

            for (int i = count / 2 - 1; i >= 0; --i)
            {
                IEnumerable<ICommand> commands = heapify(
                    numbers,
                    i,
                    count,
                    changeComparedIndices,
                    swapIndices
                    );

                foreach (var command in commands)
                {
                    yield return command;
                }
            }

            for (int i = count - 1; i > 0; --i)
            {
                yield return markAsSortedIndex(i);
                yield return swapIndices(0, i);

                IEnumerable<ICommand> commands = heapify(
                    numbers,
                    0,
                    i,
                    changeComparedIndices,
                    swapIndices
                    );

                foreach (var command in commands)
                {
                    yield return command;
                }
            }

            yield return markAsSortedIndex(0);
        }

        private static IEnumerable<ICommand> heapify(
            IList<int> numbers,
            int i,
            int size,
            Func<int, int, ICommand> changeComparedIndices,
            Func<int, int, ICommand> swapIndices
            )
        {
            int parentIndex = i;
            int childIndex;

            for (; getLeftChildIndex(parentIndex) < size; parentIndex = childIndex)
            {
                childIndex = getLeftChildIndex(parentIndex);

                yield return changeComparedIndices(childIndex, childIndex + 1);

                if (childIndex != size - 1 && numbers[childIndex] < numbers[childIndex + 1])
                {
                    childIndex++;
                }

                yield return changeComparedIndices(parentIndex, childIndex);

                if (numbers[parentIndex] < numbers[childIndex])
                {
                    yield return swapIndices(parentIndex, childIndex);
                }
                else
                {
                    break;
                }
            }
        }

        private static int getLeftChildIndex(int i)
        {
            return 2* i + 1;
        }
    }
}
