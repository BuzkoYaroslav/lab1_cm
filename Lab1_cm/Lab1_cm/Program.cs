using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_cm
{
    class Program
    {
        const char simpleIterationSym = 'S';
        const char newtonMethodSym = 'N';
        const char chordsMethodSym = 'C';
        const char halfDivisionSym = 'D';

        const int accuracyDegree = 5;

        private delegate double Method(Approximator.Function func,
            Approximator.Function func1, Approximator.Function func2, double a, double b);
        //static Approximator.Function[] funcFirst = { (double x) => { return Math.Pow(x, 3) - 2.9 * x + 3; },
        //                                             (double x) => { return 3 * Math.Pow(x, 2) - 2.9; },
        //                                             (double x) => { return 6 * x; } };
        //static Approximator.Function[] funcSecond = { (double x) => { return Math.Tan(0.4 * x + 0.4) - Math.Pow(x, 2); },
        //                                             (double x) => { return 0.4 / Math.Pow(Math.Cos(0.4 * x + 0.4), 2) - 2 * x; },
        //                                             (double x) => { return -0.32 / Math.Pow(Math.Cos(0.4 * x + 0.4), 3) - 2; } };

        static Approximator.Function[] funcFirst = { (double x) => { return Math.Pow(x, 3) - 7 * x - 19; },
                                                     (double x) => { return 3 * Math.Pow(x, 2) - 7; },
                                                     (double x) => { return 6 * x; } };
        static Approximator.Function[] funcSecond = { (double x) => { return Math.Tan(0.4 * x + 0.4) - Math.Pow(x, 2); },
                                                     (double x) => { return 0.4 / Math.Pow(Math.Cos(0.4 * x + 0.4), 2) - 2 * x; },
                                                     (double x) => { return -0.32 / Math.Pow(Math.Cos(0.4 * x + 0.4), 3) - 2; } };
        static void Main(string[] args)
        {
            while (true)
            {
                char symbol = ' ';
                double a = 0,
                       b = 0;
                bool first = true;

                Console.WriteLine("Input left bound: ");
                try
                {
                    a = Convert.ToDouble(Console.ReadLine());
                    Console.WriteLine("Input right bound: ");
                    b = Convert.ToDouble(Console.ReadLine());

                    Console.WriteLine("Input command key:" +
                                      "\n{0} - Simple Iteration method" +
                                      "\n{1} - Newton's method" +
                                      "\n{2} - Chords' method" +
                                      "\n{3} - Half division",
                                      simpleIterationSym, newtonMethodSym, chordsMethodSym, halfDivisionSym);
                    symbol = Convert.ToChar(Console.ReadLine());

                    Console.WriteLine("Choose function (1 - first, 2 - for second): ");
                    int num = Convert.ToInt32(Console.ReadLine());
                    if (num != 1 && num != 2) throw new Exception("Incorrect func number!");
                    first = num == 1;
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp.Message);
                    continue;
                }

                try
                {
                    switch (symbol)
                    {
                        case simpleIterationSym:
                            ApplyMethod(Approximator.SimpleIteration, "Simple Iteration",
                                first ? funcFirst : funcSecond, a, b);
                            break;
                        case newtonMethodSym:
                            ApplyMethod(Approximator.NewtonMethod, "Newton method",
                                first ? funcFirst : funcSecond, a, b);
                            break;
                        case chordsMethodSym:
                            ApplyMethod(Approximator.ChordsMethod, "Chords' method",
                                first ? funcFirst : funcSecond, a, b);
                            break;
                        case halfDivisionSym:
                            Approximator.Function[] func = first ? funcFirst : funcSecond;
                            double xn = Approximator.HalfDivision(func[0], a, b);

                            Console.WriteLine("Half division" +
                                              "\n x = " + Math.Round(xn, accuracyDegree));
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp.Message);
                }
            }
        }

        static void ApplyMethod(Method MethodToApply, string output, Approximator.Function[] func, double a, double b)
        {
            double xn = MethodToApply(func[0], func[1], func[2], a, b);

            Console.WriteLine(output +
                              "\n x = " + Math.Round(xn, accuracyDegree));
        }
    }
}
