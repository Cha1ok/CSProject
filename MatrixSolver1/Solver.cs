using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MatrixSolver1
{
    internal class Solver
    {
        private int _n;
        private int[][] _a;  

        public Solver(int n)
        {
            _n = n;
            _a = new int[n][];  
            for (int i = 0; i < n; i++)
            {
                _a[i] = new int[n];  
            }
        }
        
        public void SetMatrix()
        {
            for (int i = 0; i < _n; i++)
            {
                for (int j = 0; j < _n; j++)
                {
                    _a[i][j] = Convert.ToInt32(Console.Read());
                }
            }
        }

        public void PrintMaxtrix(int[][] x)
        {
            for(int i = 0;i < x.Length; i++)
            {
                Console.WriteLine("\n");
                for(int j = 0; j < x[i].Length;j++)
                {
                    Console.Write(x[i][j]+" ");
                }
            }
        }


        public int[][] SumMatrix(int[][] x)
        {
            int[][]t=new int[_n][];
            for(int i = 0;i<x.Length;i++)
            {
                t[i]=new int[_n];
                for(int j = 0; j < x[i].Length; j++)
                {
                    t[i][j] = x[i][j] + _a[i][j];
                }
            }
            return t;
        }

        public int[][] ProizvMatrix(int[][] x)
        {
            int [][]t=new int[_n][];
            for(int i = 0; i < x.Length; i++)
            {
                t[i]=new int[x[0].Length];
                for(int j = 0;j < x[i].Length; j++)
                {
                    t[i][j] = 0;
                    for(int k = 0; k < _a[0].Length; k++)
                    {
                        t[i][j] += x[i][k] * _a[k][j];
                    }
                }
            }
            return t;
        }
    }
}
