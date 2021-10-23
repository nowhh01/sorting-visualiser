using System;

namespace SortingVisualiser.Commands
{
    public class MarkAsSortedCommand : ICommand
    {
        private readonly Controller mController;
        private readonly int mIndex;

        private Tuple<int, int>? mPrevIndices;

        public MarkAsSortedCommand(Controller controller, int index)
        {
            mController = controller;
            mIndex = index;
        }

        public void Execute()
        {
            mPrevIndices = mController.ComparedIndices;

            mController.ComparedIndices = new(-1, -1);
            mController.HasSortedNumbers[mIndex] = true;
        }

        public void Undo()
        {
            if (mPrevIndices is not null)
            {
                mController.ComparedIndices = mPrevIndices!;
                mController.HasSortedNumbers[mIndex] = false;
            }
        }
    }
}
