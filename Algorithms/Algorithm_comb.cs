using SortingVisualiser.Commands;
using System;
using System.Collections.Generic;

namespace SortingVisualiser
{
    public partial class Algorithm
    {
        public static IEnumerator<ICommand> CombSort(
            IList<int> numbers,
            Func<int, int, ICommand> changeComparedIndices,
            Func<int, int, ICommand> swapIndices,
            Func<ICommand> markAllAsSortedIndex
            )
        {
            int count = numbers.Count;
            float shrink = 1.3f;
            float gap = count;
            bool bSorted = false;

            while (!bSorted)
            {
                gap /= shrink;

                if (gap < 2)
                {
                    bSorted = true;
                    gap = 1;    
                }

                for (int i = 0; i < count - gap; i++)
                {
                    int comparedIndex = (int)gap + i;

                    yield return changeComparedIndices(i, comparedIndex);

                    if (numbers[i] > numbers[comparedIndex])
                    {
                        bSorted = false;
                        
                        yield return swapIndices(i, comparedIndex);
                    }
                }
            }

            yield return markAllAsSortedIndex();
        }
    }
}
