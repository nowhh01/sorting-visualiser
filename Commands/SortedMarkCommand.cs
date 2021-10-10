using System;

namespace SortingVisualiser.Commands
{
    public class SortedMarkCommand : ICommand
    {
        private readonly Controller mController;
        private readonly int mIndex;

        private Tuple<int, int>? mPrevIndices;

        public SortedMarkCommand(Controller controller, int index)
        {
            mController = controller;
            mIndex = index;
        }

        public void Execute()
        {
            mPrevIndices = mController.ComparedIndices;

            mController.ComparedIndices = new(-1, -1);
            mController.AreSorted[mIndex] = true;
        }

        public void Undo()
        {
            if (mPrevIndices is not null)
            {
                mController.ComparedIndices = mPrevIndices!;
                mController.AreSorted[mIndex] = false;
            }
        }
    }
}
