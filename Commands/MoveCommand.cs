using System;
using System.Collections.Generic;

namespace SortingVisualiser.Commands
{
    public class MoveCommand : ICommand
    {
        private readonly Controller mController;
        private readonly int mIndexFrom;
        private readonly int mIndexTo;

        public MoveCommand(Controller controller, int indexFrom, int indexTo)
        {
            mController = controller;
            mIndexFrom = indexFrom;
            mIndexTo = indexTo;
        }

        public void Execute()
        {
            move(mIndexFrom, mIndexTo);
        }

        public void Undo()
        {
            move(mIndexTo, mIndexFrom);
        }

        private void move(int indexFrom, int indexTo)
        {
            IList<int> numbers = mController.Numbers;
            int number = numbers[indexFrom];

            numbers.RemoveAt(indexFrom);
            numbers.Insert(indexTo, number);

            int leastIndex = indexTo;
            int adjuster = 1;

            if (indexFrom < indexTo)
            {
                leastIndex = indexFrom + 1;
                adjuster = -1;
            }

            int count = Math.Abs(indexFrom - indexTo);
            var tuples = new Tuple<int, int>[count + 1];

            for (int i = 0; i < count; i++)
            {
                int index = leastIndex + i;

                tuples[i] = new Tuple<int, int>(index, index + adjuster);
            }

            tuples[count] = new Tuple<int, int>(indexFrom, indexTo);

            mController.MovedIndices = tuples;
        }
    }
}
