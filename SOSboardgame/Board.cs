namespace SOSboardgame
{
    public class Board
    {
        public int Size { get; }
        public char[,] Cells { get; }

        public Board(int size)
        {
            Size = size;
            Cells = new char[size, size];
        }

        public char this[int r, int c]
        {
            get => Cells[r, c];
            set => Cells[r, c] = value;
        }

        public bool IsFull()
        {
            for (int r = 0; r < Size; r++)
                for (int c = 0; c < Size; c++)
                    if (Cells[r, c] == '\0')
                        return false;

            return true;
        }
    }
}
