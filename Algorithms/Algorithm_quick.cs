using SortingVisualiser.Commands;
using System;
using System.Collections.Generic;

namespace SortingVisualiser
{
    public partial class Algorithm
    {
        public static IEnumerator<ICommand> QuickSort(
            IList<int> numbers,
            Func<int, int, ICommand> changeComparedIndices,
            Func<int, int, ICommand> swapIndices,
            Func<int, ICommand> markAsSortedIndex
            )
        {
            IEnumerable<ICommand> commands = quickSort(
                numbers,
                0,
                numbers.Count - 1,
                changeComparedIndices,
                swapIndices,
                markAsSortedIndex
                );

            foreach (var command in commands)
            {
                yield return command;
            }
        }

        private static IEnumerable<ICommand> quickSort(
            IList<int> numbers,
            int leftOffset,
            int rightEnd,
            Func<int, int, ICommand> changeComparedIndices,
            Func<int, int, ICommand> swapIndices,
            Func<int, ICommand> markAsSortedIndex
            )
        {
            if (rightEnd >= leftOffset)
            {
                int pivot = numbers[leftOffset];
                int leftIndex = leftOffset + 1;
                int rightIndex = rightEnd;

                while (true)
                {
                    while (leftIndex < rightEnd)
                    {
                        yield return changeComparedIndices(leftIndex, leftOffset);

                        if (numbers[leftIndex] >= pivot)
                        {
                            break;
                        }

                        leftIndex++;
                    }

                    while (leftIndex > leftOffset)
                    {
                        yield return changeComparedIndices(leftOffset, rightIndex);

                        if (pivot >= numbers[rightIndex])
                        {
                            break;
                        }

                        rightIndex--;
                    }

                    if (leftIndex < rightIndex)
                    {
                        yield return swapIndices(leftIndex, rightIndex);
                    }
                    else
                    {
                        break;
                    }
                }

                if (leftOffset != rightIndex)
                {
                    yield return swapIndices(leftOffset, rightIndex);
                }

                yield return markAsSortedIndex(rightIndex);

                IEnumerable<ICommand> leftCommands = quickSort(
                    numbers,
                    leftOffset,
                    rightIndex - 1,
                    changeComparedIndices,
                    swapIndices,
                    markAsSortedIndex
                    );

                foreach (var command in leftCommands)
                {
                    yield return command;
                }

                IEnumerable<ICommand> rightCommands = quickSort(
                    numbers,
                    rightIndex + 1,
                    rightEnd,
                    changeComparedIndices,
                    swapIndices,
                    markAsSortedIndex
                    );

                foreach (var command in rightCommands)
                {
                    yield return command;
                }
            }
        }
    }
}
