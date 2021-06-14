using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;

namespace КГ_лаба3._1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            DrawGraph();
        }

        //матрицы фигуры и точек
        List<double[]> Matrix = new List<double[]>();
        List<double[]> Points_Node = new List<double[]>();

        //матрицы возврата обратно в исходную позицию
        double[,] matrix_move = { { 1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };

        private void DrawGraph()
        {
            //отрисовка сетки

            //настройка области
            GraphPane pane = zgc.GraphPane;
            //pane.CurveList.Clear();
            pane.XAxis.Title.Text = "Ось X";
            pane.YAxis.Title.Text = "Ось Y";
            pane.XAxis.MajorGrid.IsZeroLine = true;

            //координаты отображаемой области
            pane.XAxis.Scale.Min = -15;
            pane.XAxis.Scale.Max = 15;
            pane.YAxis.Scale.Min = -15;
            pane.YAxis.Scale.Max = 15;

            //отрисовка вертикальных полос сетки
            for (int j = -20; j < 20; j++)
            {
                PointPairList list2 = new PointPairList();
                list2.Add(j, -20);
                list2.Add(j, 20);
                LineItem myFig2 = pane.AddCurve("", list2, Color.Aquamarine, SymbolType.None);
            }

            //отрисовка горризонтальных полос сетки
            for (int j = -20; j < 20; j++)
            {
                PointPairList list2 = new PointPairList();
                list2.Add(-20, j);
                list2.Add(20, j);
                LineItem myFig2 = pane.AddCurve("", list2, Color.Aquamarine, SymbolType.None);
            }

            zgc.AxisChange();
            zgc.Invalidate();
        }

        private void Circle()
        {
            //нахождение исходных координат окружности
            List<double[]> Points = new List<double[]>();
            int x0 = Convert.ToInt32(x1.Text);
            int y0 = Convert.ToInt32(y1.Text);
            int r = Convert.ToInt32(R.Text);

            double[] t2 = { x0 + r, y0, 1 };
            Points.Add(t2);
            for (int i = 0; i < 360; i+=10)
            {
                double[] t1 = { r*Math.Cos(i*Math.PI/180)+x0, r * Math.Sin(i * Math.PI / 180)+y0, 1 };
                Points.Add(t1);
            }
            double[] t3 = { r * Math.Cos(0 * Math.PI / 180) + x0, r * Math.Sin(0 * Math.PI / 180) + y0, 1 };
            Points.Add(t3);

            Matrix = Points;
        } 

        private void DrawCircle()
        {
            //отрисовка окружности
            GraphPane pane = zgc.GraphPane;
            try
            {
                PointPairList list = new PointPairList();
        
                for (int i = 0; i < Matrix.Count(); i++)
                {
                    list.Add(Matrix[i][0], Matrix[i][1]);
                }
                LineItem myCurve = pane.AddCurve("", list, Color.Blue, SymbolType.None);

                zgc.AxisChange();
                zgc.Invalidate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + " - у вас ошибка");
            }
        }

        private void DrawPoints()
        {
            //отрисовка точек
            GraphPane pane = zgc.GraphPane;

            try
            {
                PointPairList list = new PointPairList();
                for (int i = 0; i < Points_Node.Count(); ++i)
                {
                    list.Add(Points_Node[i][0], Points_Node[i][1]);
                }
                LineItem myCurve = pane.AddCurve("", list, Color.Blue, SymbolType.Diamond);
                myCurve.Line.IsVisible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + " - у вас ошибка");
            }
        }

        private double F(double x, double y, double r)
        {
            //функция проверки нахождения середины клетки
            return x*x+y*y-r*r;
        }

        private void Return_in_start()
        {          
            //возврат всех точек в исходное положение
            List<double[]> Matrix2 = new List<double[]>();
            List<double[]> Points_Node2 = new List<double[]>();

            for (int i = 0; i < Matrix.Count(); ++i)
            {
                Matrix2.Add(Multiplication_Matrix(Matrix[i], matrix_move));
            }

            for (int i = 0; i < Points_Node.Count(); ++i)
            {
                Points_Node2.Add(Multiplication_Matrix(Points_Node[i], matrix_move));
            }

            Matrix = Matrix2;
            Points_Node = Points_Node2;
        }

        private void Moving_along_axisX()
        {
            int r = Convert.ToInt32(R.Text);
            //перенос в начало координат
            try
            {
                int koeff = (int)Matrix[0][0] - r;
                int koeff2 = (int)Matrix[0][1];
                List<double[]> Points = new List<double[]>();
                double[,] matrix = { { 1, 0, -koeff }, { 0, 1, -koeff2 }, { 0, 0, 1 } };
                for (int i = 0; i < Matrix.Count(); ++i)
                {
                    Points.Add(Multiplication_Matrix(Matrix[i], matrix));
                }

                double[,] matrix2 = { { 1, 0, koeff }, { 0, 1, koeff2 }, { 0, 0, 1 } };
                matrix_move = matrix2;

                Matrix = Points;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + " - у вас ошибка");
            }
        }

        private double[] Multiplication_Matrix(double[] vector, double[,] matrix)
        {
            //умножение вектора на матрицу
            double[] newvector = new double[3];
            double tmp = 0;
            for (int i = 0; i < 3; ++i)
            {
                for (int j = 0; j < 3; ++j)
                {
                    tmp += vector[j] * matrix[i, j];
                }
                newvector[i] = tmp;
                tmp = 0;
            }
            return newvector;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //отрисовка исходной окружности
            GraphPane pane = zgc.GraphPane;
            pane.CurveList.Clear();
            Circle();
            DrawCircle();   
            DrawGraph();
            zgc.Invalidate();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            //смещение окружности в начало координат
            GraphPane pane = zgc.GraphPane;
            pane.CurveList.Clear();
            Moving_along_axisX();
            DrawCircle();
            DrawGraph();
            zgc.Invalidate();
        }


        private void button6_Click(object sender, EventArgs e)
        {
            int r = Convert.ToInt32(R.Text);
            List<double[]> Points_reserve = new List<double[]>();

            //нахождение координат точек
            try
            {
                //double func = 0;
                double D0 = 1 - r;
                int x0 = 0;
                int y0 = r;

                double[] t = { x0, y0, 1 };

                Points_reserve.Add(t);

                while (x0 < y0)
                {
                    //func = F(x0 + 1, y0 - 0.5, r);
                    if (D0 > 0)
                    {
                        D0 += (2 * x0  - 2 * y0 + 5);
                        x0++;
                        y0--;
                    }
                    else
                    {
                        D0 += (2 * x0 + 3);
                        x0++;
                    }
                    double[] t1 = { x0, y0, 1 };
                    Points_reserve.Add(t1);
                }

                //прямая х = у
                double[,] matrix = { { 0, 1, 0 }, { 1, 0, 0 }, { 0, 0, 1 } };
                int size = Points_reserve.Count();
                for (int i = 0; i < size; ++i)
                {
                    Points_reserve.Add(Multiplication_Matrix(Points_reserve[i], matrix));
                }

                //ось Ох
                double[,] matrix2 = { { 1, 0, 0 }, { 0, -1, 0 }, { 0, 0, 1 } };
                size = Points_reserve.Count();
                for (int i = 0; i < size; ++i)
                {
                    Points_reserve.Add(Multiplication_Matrix(Points_reserve[i], matrix2));
                }

                //ось Оу
                double[,] matrix3 = { { -1, 0, 0 }, { 0, 1, 0 }, { 0, 0, 1 } };
                size = Points_reserve.Count();
                for (int i = 0; i < size; ++i)
                {
                    Points_reserve.Add(Multiplication_Matrix(Points_reserve[i], matrix3));
                }

                Points_Node = Points_reserve;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex + " - у вас ошибка");
            }
            DrawGraph();
            DrawCircle();
            DrawPoints();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //вовзращение всех точек и окружности на исходную позицию
            GraphPane pane = zgc.GraphPane;
            pane.CurveList.Clear();
            Return_in_start();
            DrawCircle();
            DrawGraph();
            DrawPoints();
            zgc.Invalidate();
        }
    }
}
