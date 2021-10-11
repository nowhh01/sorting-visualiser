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
            swap(mIndices);
        }

        public void Undo()
        {
            swap(new(mIndices.Item2, mIndices.Item1));
        }

        private void swap(Tuple<int, int> indices)
        {
            mController.Numbers.Swap(indices.Item1, indices.Item2);

            mController.SwappedIndices = indices;
        }
    }
}
