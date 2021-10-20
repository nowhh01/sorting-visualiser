using SortingVisualiser.Commands;
using System;
using System.Collections.Generic;

namespace SortingVisualiser
{
    public partial class Algorithm
    {
        public static IEnumerator<ICommand> MergeSort(
            IList<int> numbers,
            Func<int, int, ICommand> changeComparedIndices,
            Func<int, int, ICommand> moveIndices,
            Func<int, ICommand> markAsSortedIndex
            )
        {
            IList<int> tmp = new int[numbers.Count];

            IEnumerable<ICommand> commands = mergeSort(
                numbers,
                tmp,
                0,
                tmp.Count - 1,
                changeComparedIndices,
                moveIndices,
                markAsSortedIndex
                );

            foreach (var command in commands)
            {
                yield return command;
            }
        }

        private static IEnumerable<ICommand> mergeSort(
            IList<int> numbers,
            IList<int> tmp,
            int leftIndex,
            int rightIndex,
            Func<int, int, ICommand> changeComparedIndices,
            Func<int, int, ICommand> moveIndices,
            Func<int, ICommand> markAsSortedIndex
            )
        {
            if (leftIndex < rightIndex)
            {
                int centerIndex = (leftIndex + rightIndex) / 2;

                IEnumerable<ICommand> leftCommands = mergeSort(
                    numbers,
                    tmp,
                    leftIndex,
                    centerIndex,
                    changeComparedIndices,
                    moveIndices,
                    markAsSortedIndex
                    );

                foreach (var command in leftCommands)
                {
                    yield return command;
                }

                IEnumerable<ICommand> rightCommands = mergeSort(
                    numbers,
                    tmp,
                    centerIndex + 1,
                    rightIndex,
                    changeComparedIndices,
                    moveIndices,
                    markAsSortedIndex
                    );

                foreach (var command in rightCommands)
                {
                    yield return command;
                }

                IEnumerable<ICommand> mergeCommands = merge(
                    numbers,
                    tmp,
                    leftIndex,
                    centerIndex + 1,
                    rightIndex,
                    changeComparedIndices,
                    moveIndices,
                    markAsSortedIndex
                    );

                foreach (var command in mergeCommands)
                {
                    yield return command;
                }
            }
        }

        private static IEnumerable<ICommand> merge(
            IList<int> numbers,
            IList<int> tmp,
            int leftOffset,
            int rightOffset,
            int rightEnd,
            Func<int, int, ICommand> changeComparedIndices,
            Func<int, int, ICommand> moveIndices,
            Func<int, ICommand> markAsSortedIndex
            )
        {
            for (int i = rightEnd; i > leftOffset - 1; --i)
            {
                tmp[i] = numbers[i];
            }

            int leftEnd = rightOffset - 1;
            int tmpIndex = leftOffset;
            int leftAdjuster = 0;
            int rightAdjuster = 0;
            bool bMarked = leftOffset == 0 && rightEnd + 1 == numbers.Count;

            while (leftOffset + leftAdjuster <= leftEnd
                && rightOffset + rightAdjuster <= rightEnd)
            {
                yield return changeComparedIndices(tmpIndex, rightOffset + rightAdjuster);

                if (tmp[leftOffset + leftAdjuster] <= tmp[rightOffset + rightAdjuster])
                {
                    if (bMarked)
                    {
                        yield return markAsSortedIndex(tmpIndex);
                    }

                    leftAdjuster++;
                }
                else
                {
                    yield return moveIndices(rightOffset + rightAdjuster, tmpIndex);

                    if (bMarked)
                    {
                        yield return markAsSortedIndex(tmpIndex);
                    }

                    rightAdjuster++;
                }

                tmpIndex++;
            }

            while (leftOffset + leftAdjuster <= leftEnd)
            {
                yield return changeComparedIndices(tmpIndex, leftOffset + leftAdjuster);

                if (bMarked)
                {
                    yield return markAsSortedIndex(tmpIndex);
                }

                leftAdjuster++;
                tmpIndex++;
            }

            while (rightOffset + rightAdjuster <= rightEnd)
            {
                yield return changeComparedIndices(tmpIndex, rightOffset + rightAdjuster);

                if (bMarked)
                {
                    yield return markAsSortedIndex(tmpIndex);
                }

                rightAdjuster++;
                tmpIndex++;
            }
        }
    }
}
