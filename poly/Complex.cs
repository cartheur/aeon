//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace SweetPolynomial
{
    /// <summary>
    /// Class representation of a complex number.
    /// </summary>
    public class Complex
    {
        /// <summary>
        /// Contains the real part of a complex number.
        /// </summary>
        public double Real { get; set; }
        /// <summary>
        /// Contains the imaginary part of a complex number.
        /// </summary>
        public double Imaginary { get; set; }
        /// <summary>
        /// The difference threshold to prevent rounding errors.
        /// </summary>
        const double Epsilon = 1E-6;
        /// <summary>
        /// Imaginary unit.
        /// </summary>
        public static Complex I
        {
            get
            {
                return new Complex(0, 1);
            }
        }
        /// <summary>
        /// Complex number zero.
        /// </summary>
        public static Complex Zero
        {
            get
            {
                return new Complex(0, 0);
            }
        }
        /// <summary>
        /// Complex number valued at one.
        /// </summary>
        public static Complex One
        {
            get
            {
                return new Complex(1, 0);
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Complex"/> class as (0, 0).
        /// </summary>
        public Complex()
        {
            Real = 0;
            Imaginary = 0;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Complex"/> class with imaginary part equal to 0.
        /// </summary>
        /// <param name="realPart">The real part.</param>
        public Complex(double realPart)
        {
            Real = realPart;
            Imaginary = 0;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Complex"/> class.
        /// </summary>
        /// <param name="realPart">The real part.</param>
        /// <param name="imaginaryPart">The imaginary part.</param>
        public Complex(double realPart, double imaginaryPart)
        {
            Real = realPart;
            Imaginary = imaginaryPart;
        }
        /// <summary>
        /// Inits complex number from string like "a+bi".
        /// </summary>
        /// <param name="s"></param>
        public Complex(string s)
        {
            var split = s.Split('+');
            Real = Convert.ToDouble(split[0]);
            var next = split[1].Remove(1);
            Imaginary = Convert.ToDouble(next);
        }
        /// <summary>
        /// Tests the specified input.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static Match Test(string s)
        {

            const string dp = "([0-9]+[.]?[0-9]*|[.][0-9]+)";
            const string dm = "[-]?" + dp;
            var r = new Regex("^(?<RePart>(" + dm + ")[-+](?<ImPart>(" + dp + "))[i])$");

            return r.Match(s);
        }

        #region Operators

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Complex operator +(Complex a, Complex b)
        {
            //if (a == null) return b;
            //else if (b == null) return a;
            //else 
            return new Complex(a.Real + b.Real, a.Imaginary + b.Imaginary);
        }
        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Complex operator +(Complex a, double b)
        {
            return new Complex(a.Real + b, a.Imaginary);
        }
        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Complex operator +(double a, Complex b)
        {
            return new Complex(a + b.Real, b.Imaginary);
        }
        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Complex operator -(Complex a, Complex b)
        {
            return new Complex(a.Real - b.Real, a.Imaginary - b.Imaginary);
        }
        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Complex operator -(Complex a, double b)
        {
            return new Complex(a.Real - b, a.Imaginary);
        }
        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Complex operator -(double a, Complex b)
        {
            return new Complex(a - b.Real, -b.Imaginary);
        }
        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="a">a.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Complex operator -(Complex a)
        {
            return new Complex(-a.Real, -a.Imaginary);
        }
        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Complex operator *(Complex a, Complex b)
        {
            return new Complex(a.Real * b.Real - a.Imaginary * b.Imaginary,
                a.Imaginary * b.Real + a.Real * b.Imaginary);
        }
        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="d">The d.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Complex operator *(Complex a, double d)
        {
            return new Complex(d * a.Real, d * a.Imaginary);
        }
        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="a">a.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Complex operator *(double d, Complex a)
        {
            return new Complex(d * a.Real, d * a.Imaginary);
        }
        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Complex operator /(Complex a, Complex b)
        {
            return a * Conj(b) * (1 / (Abs(b) * Abs(b)));
        }
        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Complex operator /(Complex a, double b)
        {
            return a * (1 / b);
        }
        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Complex operator /(double a, Complex b)
        {
            return a * Conj(b) * (1 / (Abs(b) * Abs(b)));
        }
        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Complex a, Complex b)
        {
            return a.Real == b.Real && Math.Abs(a.Imaginary - b.Imaginary) < Epsilon;
        }
        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(Complex a, double b)
        {
            return a == new Complex(b);
        }
        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator ==(double a, Complex b)
        {
            return new Complex(a) == b;
        }
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Complex a, Complex b)
        {
            return !(a == b);
        }
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(Complex a, double b)
        {
            return !(a == b);
        }
        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator !=(double a, Complex b)
        {
            return !(a == b);
        }

        #endregion

        #region Static funcs & overrides

        /// <summary>
        /// Calcs the absolute value of a complex number.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double Abs(Complex a)
        {
            return Math.Sqrt(a.Imaginary * a.Imaginary + a.Real * a.Real);
        }
        /// <summary>
        /// Inverts a.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Complex Inv(Complex a)
        {
            return new Complex(a.Real / (a.Real * a.Real + a.Imaginary * a.Imaginary),
                -a.Imaginary / (a.Real * a.Real + a.Imaginary * a.Imaginary));
        }
        /// <summary>
        /// Tangent of a.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Complex Tan(Complex a)
        {
            return Sin(a) / Cos(a);
        }
        /// <summary>
        /// Hyperbolic cosine of a.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Complex Cosh(Complex a)
        {
            return (Exp(a) + Exp(-a)) / 2;
        }
        /// <summary>
        /// Hyperbolic sine of a.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Complex Sinh(Complex a)
        {
            return (Exp(a) - Exp(-a)) / 2;
        }
        /// <summary>
        /// Hyperbolic tangent of a.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Complex Tanh(Complex a)
        {
            return (Exp(2 * a) - 1) / (Exp(2 * a) + 1);
        }
        /// <summary>
        /// Hyperbolic cotangent of a.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Complex Coth(Complex a)
        {
            return (Exp(2 * a) + 1) / (Exp(2 * a) - 1);
        }
        /// <summary>
        /// Hyperbolic secant of a.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Complex Sech(Complex a)
        {
            return Inv(Cosh(a));
        }
        /// <summary>
        /// Hyperbolic cosecant of a.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Complex Csch(Complex a)
        {
            return Inv(Sinh(a));
        }
        /// <summary>
        /// Cotangent of a.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Complex Cot(Complex a)
        {
            return Cos(a) / Sin(a);
        }
        /// <summary>
        /// Computes the conjugation of a complex number.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Complex Conj(Complex a)
        {
            return new Complex(a.Real, -a.Imaginary);
        }
        /// <summary>
        /// Complex square root.
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Complex Sqrt(double d)
        {
            if (d >= 0)
                return new Complex(Math.Sqrt(d));
            return new Complex(0, Math.Sqrt(-d));
        }
        /// <summary>
        /// Complex square root.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>   
        public static Complex Sqrt(Complex a)
        {
            return Pow(a, .5);
        }
        /// <summary>
        /// Complex exponential function.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Complex Exp(Complex a)
        {
            return new Complex(Math.Exp(a.Real) * Math.Cos(a.Imaginary), Math.Exp(a.Real) * Math.Sin(a.Imaginary));
        }
        /// <summary>
        /// Main value of the complex logarithm.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Complex Log(Complex a)
        {
            // Log[|w|]+I*(Arg[w]+2*Pi*k)            

            return new Complex(Math.Log(Abs(a)), Arg(a));
        }
        /// <summary>
        /// Argument of the complex number.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static double Arg(Complex a)
        {
            if (a.Real < 0)
            {
                if (a.Imaginary < 0)
                    return Math.Atan(a.Imaginary / a.Real) - Math.PI;
                return Math.PI - Math.Atan(-a.Imaginary / a.Real);
            }
            return Math.Atan(a.Imaginary / a.Real);
        }
        /// <summary>
        /// Complex cosine.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Complex Cos(Complex a)
        {
            return .5 * (Exp(I * a) + Exp(-I * a));
        }
        /// <summary>
        /// Complex sine.
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Complex Sin(Complex a)
        {
            return (Exp(I * a) - Exp(-I * a)) / (2 * I);
        }
        /// <summary>
        /// Power of a by b.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static Complex Pow(Complex a, Complex b)
        {
            return Exp(b * Log(a));
        }
        /// <summary>
        /// Power of a by b.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static Complex Pow(double a, Complex b)
        {
            return Exp(b * Math.Log(a));
        }
        /// <summary>
        /// Power of a by b.
        /// </summary>
        /// <param name="a">a.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static Complex Pow(Complex a, double b)
        {
            return Exp(b * Log(a));
        }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (this == Zero) return "0";

            string imaginary, sign;

            if (Imaginary < 0)
            {
                sign = Math.Abs(Real) < Epsilon ? "-" : " - ";
            }
            else if (Imaginary > 0 && Math.Abs(Real) > Epsilon) sign = " + ";
            else sign = "";

            var real = Math.Abs(Real) < Epsilon ? "" : Real.ToString(CultureInfo.InvariantCulture);

            if (Math.Abs(Imaginary) < Epsilon) imaginary = "";
            else if (Math.Abs(Imaginary - (-1)) < Epsilon || Math.Abs(Imaginary - 1) < Epsilon) imaginary = "i";
            else imaginary = Math.Abs(Imaginary) + "i";

            return real + sign + imaginary;
        }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public string ToString(string format)
        {
            if (this == Zero) return "0";
            if (double.IsInfinity(Real) || double.IsInfinity(Imaginary)) return "oo";
            if (double.IsNaN(Real) || double.IsNaN(Imaginary)) return "?";

            string imaginary, sign;

            if (Imaginary < 0)
            {
                sign = Math.Abs(Real) < Epsilon ? "-" : " - ";
            }
            else if (Imaginary > 0 && Math.Abs(Real) > Epsilon) sign = " + ";
            else sign = "";

            string real = Math.Abs(Real) < Epsilon ? "" : Real.ToString(format);

            if (Math.Abs(Imaginary) < Epsilon) imaginary = "";
            else if (Math.Abs(Imaginary - (-1)) < Epsilon || Math.Abs(Imaginary - 1) < Epsilon) imaginary = "i";
            else imaginary = Math.Abs(Imaginary).ToString(format) + "i";

            return real + sign + imaginary;
        }
        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj.ToString() == ToString();
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return -1;
        }

        #endregion

        #region Dynamics

        /// <summary>
        /// Determines whether this value is real.
        /// </summary>
        /// <returns></returns>
        public bool IsReal()
        {
            return (Math.Abs(Imaginary) < Epsilon);
        }
        /// <summary>
        /// Determines whether this value is imaginary.
        /// </summary>
        /// <returns></returns>
        public bool IsImaginary()
        {
            return (Math.Abs(Real) < Epsilon);
        }

        #endregion
    }
}
