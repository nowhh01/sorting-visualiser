using System;

namespace SortingVisualiser.Commands
{
    public class IndicesChangeCommand : ICommand
    {
        private readonly Controller mController;
        private readonly Tuple<int, int> mIndices;

        private Tuple<int, int>? mPrevIndices;

        public IndicesChangeCommand(Controller controller, int index1, int index2)
        {
            mController = controller;
            mIndices = new(index1, index2);
        }

        public void Execute()
        {
            mPrevIndices = mController.ComparedIndices;

            mController.ComparedIndices = mIndices;
        }

        public void Undo()
        {
            if (mPrevIndices is not null)
            {
                mController.ComparedIndices = mPrevIndices!;
            }
        }
    }
}
