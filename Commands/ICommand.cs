namespace SortingVisualiser.Commands
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}
