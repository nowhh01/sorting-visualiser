using SortingVisualiser.Commands;
using System;
using System.Collections.Generic;

namespace SortingVisualiser
{
    public class Controller
    {
        private readonly List<ICommand> mCommands = new(50);
        private readonly List<int> mBackupNumbers = new(50);
        private readonly Random mRandom = new();

        private IEnumerator<ICommand>? mSelectedSortingSteps;

        private Tuple<int, int> mComparedIndices = new(-1, -1);
        private int mCurrentStep = -1;

        public IEnumerator<ICommand>? SelectedSortingSteps
        {
            get => mSelectedSortingSteps;
            set
            {
                mSelectedSortingSteps = value;

                if (value is not null)
                {
                    moveNextStep();
                }
            }
        }
        public List<int> Numbers { get; } = new(50);
        public List<bool> AreSorted { get; set; } = new(50);
        public Tuple<int, int> ComparedIndices
        {
            get => mComparedIndices;
            set
            {
                mComparedIndices = value;
            }
        }
        public bool SkipForwarding { get; set; }
        public bool IsFullySorted { get; set; }
        public bool IsSorting { get; set; }
        public bool IsSwapped { get; set; }

        public Controller(int numberCount)
        {
            RandomizeNumbers(numberCount);
        }

        public void RandomizeNumbers(int count)
        {
            Numbers.Clear();
            mBackupNumbers.Clear();
            AreSorted.Clear();

            for (int i = 0; i < count; i++)
            {
                int number = mRandom.Next(1, 100);

                Numbers.Add(number);
                mBackupNumbers.Add(number);
                AreSorted.Add(false);
            }
        }

        public void InitializeWithBackup()
        {
            Initialize();

            for (int i = 0; i < mBackupNumbers.Count; i++)
            {
                Numbers[i] = mBackupNumbers[i];
                AreSorted[i] = false;
            }
        }

        public void Initialize()
        {
            ComparedIndices = new(-1, -1);

            SelectedSortingSteps = null;

            IsFullySorted = false;
            IsSwapped = false;

            mCurrentStep = -1;
            mCommands.Clear();
        }

        public void StepForward()
        {
            mCurrentStep++;

            ICommand? command;

            if (mCurrentStep < mCommands.Count)
            {
                command = mCommands[mCurrentStep];
            }
            else
            {
                command = SelectedSortingSteps?.Current;

                if (command is not null)
                {
                    mCommands.Add(command);
                }
            }

            command?.Execute();

            moveNextStep();
        }

        public void StepBack()
        {
            ICommand command = mCommands[mCurrentStep];

            command.Undo();

            mCurrentStep--;
        }

        private void moveNextStep()
        {
            if (mSelectedSortingSteps is not null)
            {
                IsFullySorted = !mSelectedSortingSteps.MoveNext();
            }
        }
    }
}
