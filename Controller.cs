﻿using SortingVisualiser.Commands;
using System;
using System.Collections.Generic;

namespace SortingVisualiser
{
    public enum ESortType
    {
        None = -1,
        Bubble,
        Selection,
        Insertion,
        Merge,
        Quick,
        Heap,
        Comb,
        Shell,
    };

    public class Controller
    {
        private readonly List<ICommand> mCommands = new(50);
        private readonly List<int> mBackupNumbers = new(50);
        private readonly Func<IEnumerator<ICommand>>[] mSorts;
        private readonly Random mRandom = new();

        private IEnumerator<ICommand>? mSelectedSortingSteps;
        private int mCurrentStep = -1;

        public List<int> Numbers { get; } = new(50);
        public List<bool> HasSortedNumbers { get; set; } = new(50);
        public Tuple<int, int>[] MovedIndices { get; set; } = Array.Empty<Tuple<int, int>>();
        public Tuple<int, int> ComparedIndices { get; set; } = new(-1, -1);
        public Tuple<int, int> SwappedIndices { get; set; } = new(-1, -1);

        public ESortType SelectedSort { get; set; } = ESortType.None;
        public bool SkipForwarding { get; set; }
        public bool IsFullySorted { get; set; }
        public bool IsSorting { get; set; }
        public bool IsSwapped
        {
            get => SwappedIndices.Item1 != -1 && SwappedIndices.Item2 != -1;
            set
            {
                if (!value)
                {
                    SwappedIndices = new(-1, -1);
                }
            }
        }
        public bool IsMoved
        {
            get => MovedIndices != Array.Empty<Tuple<int, int>>();
            set
            {
                if (!value)
                {
                    MovedIndices = Array.Empty<Tuple<int, int>>();
                }
            }
        }

        public Controller(int numberCount)
        {
            mSorts = new Func<IEnumerator<ICommand>>[]
            {
                () => Algorithm.BubbleSort(Numbers, changeComparedIndices, swapIndices, markAsSortedIndex),
                () => Algorithm.SelectionSort(Numbers, changeComparedIndices, swapIndices, markAsSortedIndex),
                () => Algorithm.InsertionSort(Numbers, changeComparedIndices, swapIndices, markAllAsSortedIndex),
                () => Algorithm.MergeSort(Numbers, changeComparedIndices, moveIndices, markAsSortedIndex),
                () => Algorithm.QuickSort(Numbers, changeComparedIndices, swapIndices, markAsSortedIndex),
                () => Algorithm.HeapSort(Numbers, changeComparedIndices, swapIndices, markAsSortedIndex),
                () => Algorithm.CombSort(Numbers, changeComparedIndices, swapIndices, markAllAsSortedIndex),
                () => Algorithm.ShellSort(Numbers, changeComparedIndices, swapIndices, markAllAsSortedIndex),
            };

            RandomizeNumbers(numberCount);
        }

        public void RandomizeNumbers(int count)
        {
            Numbers.Clear();
            mBackupNumbers.Clear();
            HasSortedNumbers.Clear();

            for (int i = 0; i < count; i++)
            {
                int number = mRandom.Next(1, 100);

                Numbers.Add(number);
                mBackupNumbers.Add(number);
                HasSortedNumbers.Add(false);
            }
        }

        public void InitializeWithBackup()
        {
            Initialize();

            for (int i = 0; i < mBackupNumbers.Count; i++)
            {
                Numbers[i] = mBackupNumbers[i];
                HasSortedNumbers[i] = false;
            }
        }

        public void Initialize()
        {
            ComparedIndices = new(-1, -1);

            mSelectedSortingSteps = null;

            SelectedSort = ESortType.None;
            IsFullySorted = false;
            IsSwapped = false;

            mCurrentStep = -1;
            mCommands.Clear();
        }

        public void ChangeSort(ESortType sortType)
        {
            SelectedSort = sortType;

            Func<IEnumerator<ICommand>> enumerator = mSorts[(int)sortType];

            mSelectedSortingSteps = enumerator.Invoke();

            moveNextStep();
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
                command = mSelectedSortingSteps?.Current;

                moveNextStep();

                if (command is not null)
                {
                    mCommands.Add(command);
                }
            }

            command?.Execute();
        }

        public void StepBack()
        {
            ICommand command = mCommands[mCurrentStep];

            command.Undo();

            mCurrentStep--;

            if (IsFullySorted)
            {
                IsFullySorted = false;
            }
        }

        private void moveNextStep()
        {
            if (mSelectedSortingSteps is not null)
            {
                IsFullySorted = !mSelectedSortingSteps.MoveNext();
            }
        }

        private ICommand changeComparedIndices(int index1, int index2)
        {
            return new IndicesChangeCommand(this, index1, index2);
        }

        private ICommand swapIndices(int index1, int index2)
        {
            return new SwapCommand(this, index1, index2);
        }

        private ICommand moveIndices(int indexFrom, int indexTo)
        {
            return new MoveCommand(this, indexFrom, indexTo);
        }

        private ICommand markAsSortedIndex(int index)
        {
            return new MarkAsSortedCommand(this, index);
        }

        private ICommand markAllAsSortedIndex()
        {
            return new MarkAsSortedAllCommand(this);
        }
    }
}
