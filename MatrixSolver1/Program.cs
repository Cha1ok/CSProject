namespace MatrixSolver1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int n = 3;
            Solver solver = new Solver(n);
            solver.SetMatrix();
            int[][] Tm =
            {
                new int[] {1,2,3},
                new int[] { 4, 5, 6 },
                new int[] { 6, 7, 8 }
            };

            Tm = solver.SumMatrix(Tm);
            solver.PrintMaxtrix(Tm);
        }
    }
}
