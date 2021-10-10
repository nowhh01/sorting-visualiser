using SortingVisualiser.Extensions;
using System;

namespace SortingVisualiser.Commands
{
    public class SwapCommand : ICommand
    {
        private readonly Controller mController;
        private readonly Tuple<int, int> mIndices;

        public SwapCommand(Controller controller, int index1, int index2)
        {
            mController = controller;
            mIndices = new(index1, index2);
        }

        public void Execute()
        {
            swap(mIndices.Item1, mIndices.Item2);
        }

        public void Undo()
        {
            swap(mIndices.Item2, mIndices.Item1);
        }

        private void swap(int index1, int index2)
        {
            mController.IsSwapped = true;

            mController.Numbers.Swap(index1, index2);
        }
    }
}
