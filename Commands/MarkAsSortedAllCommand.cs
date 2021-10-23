using System;
using System.Collections.Generic;
using System.Linq;

namespace SortingVisualiser.Commands
{
    public class MarkAsSortedAllCommand : ICommand
    {
        private readonly Controller mController;

        private Tuple<int, int>? mPrevIndices;

        public MarkAsSortedAllCommand(Controller controller)
        {
            mController = controller;
        }

        public void Execute()
        {
            mPrevIndices = mController.ComparedIndices;

            mController.ComparedIndices = new(-1, -1);
            markAll(true);
        }

        public void Undo()
        {
            if (mPrevIndices is not null)
            {
                mController.ComparedIndices = mPrevIndices;
                
                markAll(false);
            }
        }

        private void markAll(bool bSorted)
        {
            IList<bool> bSortedNumbers = mController.HasSortedNumbers;

            for (int i = 0; i < bSortedNumbers.Count; i++)
            {
                bSortedNumbers[i] = bSorted;
            }
        }
    }
}
