using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_cm
{
    static class Approximator
    {
        public static double epsilan = Math.Pow(10, -4);
        public static double epsilanIter = Math.Pow(10, -2);
        public static int maxIterationCount = 100000;

        public static Random rnd = new Random();

        public static double RandomNumber(double a, double b)
        {
            return rnd.Next(Convert.ToInt32(a / epsilan), Convert.ToInt32(b / epsilan)) * epsilan;
        }
        public static double SuitableRandomArgumnent(Condition cond, double a, double b)
        {
            double arg;

            do
            {
                arg = RandomNumber(a, b);
            } while (!cond(arg));

            return arg;
        }

        public delegate double Function(double x);
        public delegate double FunctionTwo(double x, double c);
        public delegate bool Condition(double func);

        private static bool IsGood(Condition cond, Function func, double a, double b)
        {
            try
            {
                for (double i = a; i <= b; i += epsilan)
                {
                    if (cond(func(i)))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch (NotFiniteNumberException)
            {
                return false;
            }
            catch (DivideByZeroException)
            {
                return false;
            }
        }
        private static bool IsSuitable(Function func1, double a, double b)
        {
            return IsGood((double func) => { return double.IsInfinity(func) || double.IsNaN(func); },
                          func1, a, b) &&
                   (IsGood((double func) => { return func >= 0; },
                          func1, a, b) ||
                   IsGood((double func) => { return func <= 0; },
                          func1, a, b));
        }

        private static double MinValue(Function func, double a, double b)
        {
            double min = double.MaxValue;

            for (double x = a; x <= b; x += epsilan)
            {
                double f = Math.Abs(func(x));

                if (f < min) min = f;
            }

            return min;
        }
        private static double MaxValue(Function func, double a, double b)
        {
            double max = double.MinValue;

            for (double x = a; x <= b; x += epsilan)
            {
                double f = Math.Abs(func(x));

                if (f > max) max = f;
            }

            return max;
        }

        private static Function FuncSimpleIteration(Function func1, double a, double b)
        {
            double min = MinValue(func1, a, b),
                   max = MaxValue(func1, a, b);

            int k = IsGood((double func) => { return func <= 0; }, func1, a, b) ? 1 : -1;

            return (double x) => { return k * 2.0 / (min + max); };
        }
        private static Function FuncNuitonMethod(Function func1, double a, double b)
        {
            return (double x) => { return -1.0 / func1(x); };
        }
        private static Function FuncHordMethod(Function func, Function func1, Function func2, double a, double b, double c)
        {
            double funcc = func(c);
            return (double x) => { return -(x - c) / func(x) - funcc; };
        }

        private static double InitialXSimpleIteration(double a, double b)
        {
            return RandomNumber(a, b);
        }
        private static double InitialXNuitonMethod(Function func, Function func2, double a, double b)
        {
            return SuitableRandomArgumnent((double arg) => { return func(arg) * func2(arg) < 0; }, a, b);
        }
        private static double InitialXHordMethod(Function func, double a, double b, double c)
        {
            return SuitableRandomArgumnent((double x) => { return func(x) * func(c) < 0; }, a, b);
        }

        private static double GenericIteration(Function func, Function func1, Function ksi, double a, double b, double x0)
        {
            double xn = x0, counter = 0;

            do
            {
                xn = xn + ksi(xn) * func(xn);
                counter++;
            } while (counter < maxIterationCount && !(Math.Abs(func(xn)) / MinValue(func1, a, b) <= epsilanIter));

            if (counter == maxIterationCount) return double.PositiveInfinity;

            return xn;
        }
        
        public static double SimpleIteration(Function func, Function func1, Function func2, double a, double b)
        {
            if (!IsSuitable(func1, a, b)) throw new Exception("Method can not be applied!");

            Function ksi = FuncSimpleIteration(func1, a, b);

            double x0 = InitialXSimpleIteration(a, b);

            return GenericIteration(func, func1, ksi, a, b, x0);
        }
        public static double NewtonMethod(Function func, Function func1, Function func2, double a, double b)
        {
            if (!IsSuitable(func1, a, b) ||
                !IsSuitable(func2, a, b))
                throw new Exception("Method can not be applied!");

            Function ksi = FuncNuitonMethod(func1, a, b);

            double x0 = InitialXNuitonMethod(func, func2, a, b);

            return GenericIteration(func, func1, ksi, a, b, x0);
        }
        public static double ChordsMethod(Function func, Function func1, Function func2, double a, double b)
        {
            if (!IsSuitable(func1, a, b) ||
                !IsSuitable(func2, a, b))
                throw new Exception("Method can not be applied!");

            double c = SuitableRandomArgumnent((double x) => { return func(x) * func2(x) > 0; }, a, b);

            Function ksi = FuncHordMethod(func, func1, func2, a, b, c);

            double x0 = InitialXHordMethod(func, a, b, c);

            return GenericIteration(func, func1, ksi, a, b, x0);
        }
        public static double HalfDivision(Function func, double a, double b)
        {
            if (func(a) * func(b) > 0)
                return double.PositiveInfinity;

            if (Math.Abs(func(a)) <= epsilanIter) return a;
            if (Math.Abs(func(b)) <= epsilanIter) return b;

            double xn = (a + b) / 2;

            while (Math.Abs(func(xn)) <= epsilanIter)
            {
                if (func(xn) * func(a) < 0) b = xn;
                else if (func(xn) * func(b) < 0) a = xn;
                xn = (a + b) / 2;
            }

            return xn;
        }
    }
}
