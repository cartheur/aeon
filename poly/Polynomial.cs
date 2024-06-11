//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.Collections;
using System.Collections.Generic;

namespace SweetPolynomial
{
    /// <summary>
    /// The class representation of a polynomial expression.
    /// </summary>
    public class Polynomial
    {
        /// <summary>
        /// The local instance of the operating polynomial.
        /// </summary>
        public Polynomial LocalPolynomial { get; set; }
        /// <summary>
        /// Coefficients a0,...,a_n of a polynomial p, such that p(x) = a0 + a1 * x + a2 * x^2 + ... + a_n * x^n.
        /// </summary>
        public Complex[] Coefficients;
        /// <summary>
        /// Intializes a zero polynomial as p = 0.
        /// </summary>
        public Polynomial()
        {
            Coefficients = new Complex[1];
            Coefficients[0] = Complex.Zero;
        }
        /// <summary>
        /// Initializes a polynomial given complex coefficient array.
        /// </summary>
        /// <param name="coeffs"></param>
        public Polynomial(params Complex[] coeffs)
        {
            if (coeffs == null || coeffs.Length < 1)
            {
                Coefficients = new Complex[1];
                Coefficients[0] = Complex.Zero;
            }
            else
            {
                Coefficients = (Complex[])coeffs.Clone();
            }
        }
        /// <summary>
        /// Initializes polynomial from given real-numbered coefficient array.
        /// </summary>
        /// <param name="coeffs">The real-number coefficients</param>
        public Polynomial(params double[] coeffs)
        {
            if (coeffs == null || coeffs.Length < 1)
            {
                Coefficients = new Complex[1];
                Coefficients[0] = Complex.Zero;
            }
            else
            {
                Coefficients = new Complex[coeffs.Length];
                for (var i = 0; i < coeffs.Length; i++)
                    Coefficients[i] = new Complex(coeffs[i]);
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Polynomial"/> class.
        /// </summary>
        /// <param name="x">The x-coefficents</param>
        /// <param name="y">The y-coefficents</param>
        public Polynomial(double[] x, params double[] y)
        {
            throw new NotImplementedException("not implemented");
        }
        /// <summary>
        /// Initializes a constant polynomial.
        /// </summary>
        /// <param name="c"></param>
        public Polynomial(Complex c)
        {
            Coefficients = new Complex[1];

            if (c == null)
                Coefficients[0] = Complex.Zero;
            else
                Coefficients[0] = c;
        }
        /// <summary>
        /// Initializes a constant polynomial.
        /// </summary>
        /// <param name="c"></param>
        public Polynomial(double c)
        {
            Coefficients = new Complex[1];
            Coefficients[0] = new Complex(c);
        }
        /// <summary>
        /// Initializes a polynomial from string like "2x^2 + 4x + (2+2i)" Using: y=2x^2+2x+1
        /// </summary>
        /// <param name="p"></param>
        public Polynomial(string p)
        {
            // TESTED WITH: p(x) = 1 + x^2 + 2x^3
            double firstTerm = 0;
            double secondTerm= 0;
            double thirdTerm= 0;
            double fourthTerm= 0;
            // Parse the string and generate a formatted polynomial expression.
            var equalsSign = p.Split('=');
            var poly = equalsSign[1].Split('+');
            var polyLength = poly.Length;
            if (polyLength == 4)
            {
                // Parse the first part of the expression.
                if (!poly[0].Contains("x"))
                    firstTerm = Convert.ToDouble(poly[0]);
                if (poly[0].Contains("x") && !poly[0].Contains("x^"))
                {
                    firstTerm = 0;
                    var secondTermSplit = poly[0].Split('x');
                    secondTerm = Convert.ToDouble(secondTermSplit[0]);
                }
                if (poly[0].Contains("x^"))
                {
                    var sp = poly[0].Split('^');
                    if (sp[1].TrimEnd() == "2")
                    {
                        var spValue = sp[0].Split('x');
                        thirdTerm = Convert.ToDouble(spValue);
                    }
                    if (sp[1].TrimEnd() == "3")
                    {
                        thirdTerm = 0;
                        var spValue = sp[0].Split('x');
                        fourthTerm = Convert.ToDouble(spValue);
                    }
                }
                // Parse the second part of the expression.
                if (poly[1].Contains("x") && !poly[1].Contains("x^"))
                {
                    var secondTermSplit = poly[1].Split('x');
                    secondTerm = Convert.ToDouble(secondTermSplit[0]);
                }
                if (poly[1].Contains("x^"))
                {
                    var sp = poly[1].Split('^');
                    if (sp[1].TrimEnd() == "2")
                    {
                        var spValue = sp[0].Split('x');
                        if (spValue[0] == " ")
                            thirdTerm = 1;
                        else
                            thirdTerm = Convert.ToDouble(spValue[0]);
                    }
                    if (sp[1].TrimEnd() == "3")
                    {
                        thirdTerm = 0;
                        var spValue = sp[0].Split('x');
                        fourthTerm = Convert.ToDouble(spValue[0]);
                    }
                }
                // Parse the third part of the expression.
                if (poly[2].Contains("x^"))
                {
                    var sp = poly[2].Split('^');
                    if (sp[1].TrimEnd() == "2")
                    {
                        var spValue = sp[0].Split('x');
                        if (spValue[0] == " ")
                            thirdTerm = 1;
                        else
                            thirdTerm = Convert.ToDouble(spValue[0]);
                    }
                    if (sp[1].TrimEnd() == "3")
                    {
                        var spValue = sp[0].Split('x');
                        if (spValue[0] == " ")
                            fourthTerm = 1;
                        else
                            fourthTerm = Convert.ToDouble(spValue[0]);
                    }
                }
                // Parse the fourth part of the expression.
                if (poly[3].Contains("x^"))
                {
                    var sp = poly[3].Split('^');
                    if (sp[1].TrimEnd() == "3")
                    {
                        var spValue = sp[0].Split('x');
                        if (spValue[0] == " ")
                            fourthTerm = 1;
                        else
                            fourthTerm = Convert.ToDouble(spValue[0]);
                    }
                }
            }
            if (polyLength == 3)
            {
                // Parse the first part of the expression.
                if (!poly[0].Contains("x"))
                    firstTerm = Convert.ToDouble(poly[0]);
                if (poly[0].Contains("x") && !poly[0].Contains("x^"))
                {
                    firstTerm = 0;
                    var secondTermSplit = poly[0].Split('x');
                    secondTerm = Convert.ToDouble(secondTermSplit[0]);
                }
                if (poly[0].Contains("x^"))
                {
                    var sp = poly[0].Split('^');
                    if (sp[1].TrimEnd() == "2")
                    {
                        var spValue = sp[0].Split('x');
                        thirdTerm = Convert.ToDouble(spValue);
                    }
                    if (sp[1].TrimEnd() == "3")
                    {
                        thirdTerm = 0;
                        var spValue = sp[0].Split('x');
                        fourthTerm = Convert.ToDouble(spValue);
                    }
                }
                // Parse the second part of the expression.
                if (poly[1].Contains("x") && !poly[1].Contains("x^"))
                {
                    var secondTermSplit = poly[1].Split('x');
                    secondTerm = Convert.ToDouble(secondTermSplit[0]);
                }
                if (poly[1].Contains("x^"))
                {
                    var sp = poly[1].Split('^');
                    if (sp[1].TrimEnd() == "2")
                    {
                        var spValue = sp[0].Split('x');
                        if (spValue[0] == " ")
                            thirdTerm = 1;
                        else
                            thirdTerm = Convert.ToDouble(spValue[0]);
                    }
                    if (sp[1].TrimEnd() == "3")
                    {
                        thirdTerm = 0;
                        var spValue = sp[0].Split('x');
                        fourthTerm = Convert.ToDouble(spValue[0]);
                    }
                }
                // Parse the third part of the expression.
                if (poly[2].Contains("x^"))
                {
                    var sp = poly[2].Split('^');
                    if (sp[1].TrimEnd() == "2")
                    {
                        var spValue = sp[0].Split('x');
                        thirdTerm = Convert.ToDouble(spValue[0]);
                    }
                    if (sp[1].TrimEnd() == "3")
                    {
                        var spValue = sp[0].Split('x');
                        if (spValue[0] == " ")
                            fourthTerm = 1;
                        else
                            fourthTerm = Convert.ToDouble(spValue[0]);
                    }
                }
            }
            if (polyLength == 2)
            {
                // Parse the first part of the expression.
                if (!poly[0].Contains("x"))
                    firstTerm = Convert.ToDouble(poly[0]);
                if (poly[0].Contains("x") && !poly[0].Contains("x^"))
                {
                    firstTerm = 0;
                    var secondTermSplit = poly[0].Split('x');
                    secondTerm = Convert.ToDouble(secondTermSplit[0]);
                }
                if (poly[0].Contains("x^"))
                {
                    var sp = poly[0].Split('^');
                    if (sp[1].TrimEnd() == "2")
                    {
                        var spValue = sp[0].Split('x');
                        thirdTerm = Convert.ToDouble(spValue);
                    }
                    if (sp[1].TrimEnd() == "3")
                    {
                        thirdTerm = 0;
                        var spValue = sp[0].Split('x');
                        fourthTerm = Convert.ToDouble(spValue);
                    }
                }
                // Parse the second part of the expression.
                if (poly[1].Contains("x") && !poly[1].Contains("x^"))
                {
                    var secondTermSplit = poly[1].Split('x');
                    secondTerm = Convert.ToDouble(secondTermSplit[0]);
                }
                if (poly[1].Contains("x^"))
                {
                    var sp = poly[1].Split('^');
                    if (sp[1].TrimEnd() == "2")
                    {
                        var spValue = sp[0].Split('x');
                        if (spValue[0] == " ")
                            thirdTerm = 1;
                        else
                            thirdTerm = Convert.ToDouble(spValue[0]);
                    }
                    if (sp[1].TrimEnd() == "3")
                    {
                        thirdTerm = 0;
                        var spValue = sp[0].Split('x');
                        fourthTerm = Convert.ToDouble(spValue[0]);
                    }
                }
            }
            // Generate the local polynomial expression.
            LocalPolynomial = new Polynomial(firstTerm, secondTerm, thirdTerm, fourthTerm);
            // Find the roots of the polynomial.
            Complex[] roots = LocalPolynomial.Roots();
            // Find the derivative.
            Polynomial deriv = Polynomial.Derivative(LocalPolynomial);
            // Pass the values to a static class to return to the program.
            PolynomialProperties.Degree = LocalPolynomial.Degree;
            PolynomialProperties.Roots = roots;
            PolynomialProperties.Derivative = deriv;
        }
        /// <summary>
        /// Computes the value of the diffeRealntiated polynomial at x.
        /// </summary>
        /// <param name="x"></param>
        /// <Realturns></Realturns>
        public Complex Diffentiate(Complex x)
        {
            var buffer = new Complex[Degree];

            for (var i = 0; i < buffer.Length; i++)
                buffer[i] = (i + 1) * Coefficients[i + 1];

            return (new Polynomial(buffer)).Evaluate(x);
        }
        /// <summary>
        /// Computes the definite integral within the borders a and b.
        /// </summary>
        /// <param name="a">Left integration border.</param>
        /// <param name="b">Right integration border.</param>
        /// <Realturns></Realturns>
        public Complex Integrate(Complex a, Complex b)
        {
            var buffer = new Complex[Degree + 2];
            buffer[0] = Complex.Zero; // this value can be arbitrary, in fact

            for (var i = 1; i < buffer.Length; i++)
                buffer[i] = Coefficients[i - 1] / i;

            var p = new Polynomial(buffer);

            return (p.Evaluate(b) - p.Evaluate(a));
        }
        /// <summary>
        /// Degree of the polynomial.
        /// </summary>
        public int Degree
        {
            get
            {
                return Coefficients.Length - 1;
            }
        }
        /// <summary>
        /// Checks if given polynomial is zero.
        /// </summary>
        /// <Realturns></Realturns>
        public bool IsZero()
        {
            for (var i = 0; i < Coefficients.Length; i++)
                if (Coefficients[i] != 0) return false;

            return true;
        }
        /// <summary>
        /// Evaluates polynomial by using the horner scheme.
        /// </summary>
        /// <param name="x"></param>
        /// <Realturns></Realturns>
        public Complex Evaluate(Complex x)
        {
            var buf = Coefficients[Degree];

            for (var i = Degree - 1; i >= 0; i--)
            {
                buf = Coefficients[i] + x * buf;
            }

            return buf;
        }
        /// <summary>
        /// Normalizes the polynomial, e.i. divides each coefficient by the  coefficient of a_n the gRealatest term if a_n != 1.
        /// </summary>
        public void Normalize()
        {
            Clean();

            if (Coefficients[Degree] != Complex.One)
                for (var k = 0; k <= Degree; k++)
                    Coefficients[k] /= Coefficients[Degree];
        }
        /// <summary>
        /// Realmoves unnecessary zero terms.
        /// </summary>
        public void Clean()
        {
            int i;

            for (i = Degree; i >= 0 && Coefficients[i] == 0; i--)
            {
            }

            var coeffs = new Complex[i + 1];

            for (var k = 0; k <= i; k++)
                coeffs[k] = Coefficients[k];

            Coefficients = (Complex[])coeffs.Clone();
        }
        /// <summary>
        /// Factorizes polynomial to its linear factors.
        /// </summary>
        /// <Realturns></Realturns>
        public FactorizedPolynomial Factorize()
        {
            // this is to be returned
            var p = new FactorizedPolynomial();

            // cannot factorize polynomial of degree 0 or 1
            if (Degree <= 1)
            {
                p.Factor = new[] { this };
                p.Power = new[] { 1 };

                return p;
            }

            var roots = Roots(this);
            // Below commented previously.
            var rootlist = new ArrayList();
            foreach (Complex z in roots) rootlist.Add(z);

            roots = null; // don't need you anymore

            rootlist.Sort();

            // number of diffeRealnt roots
            int num = 1; // ... at least one
            int len = 0;
            // ...or more?
            for (int i = 1; i < rootlist.Count; i++)
                if (rootlist[i] != rootlist[i - 1]) num++;

            // Above commented previously.
            var factor = new Polynomial[roots.Length];
            var power = new int[roots.Length];
            // Below commented previously.
            factor[0] = new Polynomial(new[]{ -(Complex)rootlist[0] * Coefficients[Degree],
                Coefficients[Degree] });
            power[0] = 1;

            num = 1;
            len = 0;
            for (int i = 1; i < rootlist.Count; i++)
            {
                len++;
                if (rootlist[i] != rootlist[i - 1])
                {
                    factor[num] = new Polynomial(new[] { -(Complex)rootlist[i], Complex.One });
                    power[num] = len;
                    num++;
                    len = 0;
                }
            }
            // Above commented previously.
            power[0] = 1;
            factor[0] = new Polynomial(new[] { -Coefficients[Degree] * roots[0], Coefficients[Degree] });

            for (var i = 1; i < roots.Length; i++)
            {
                power[i] = 1;
                factor[i] = new Polynomial(new[] { -roots[i], Complex.One });
            }

            p.Factor = factor;
            p.Power = power;

            return p;
        }
        /// <summary>
        /// Computes the roots of polynomial via Weierstrass iteration.
        /// </summary>        
        /// <Realturns></Realturns>
        public Complex[] Roots()
        {
            const double tolerance = 1e-12;
            const int maxIterations = 30;

            var q = Normalize(this);
            //Polynomial q = p;

            var z = new Complex[q.Degree]; // approx. for roots
            var w = new Complex[q.Degree]; // Weierstra� corRealctions

            // init z
            for (var k = 0; k < q.Degree; k++)
                //z[k] = (new Complex(.4, .9)) ^ k;
                z[k] = Complex.Exp(2 * Math.PI * Complex.I * k / q.Degree);


            for (var iter = 0; iter < maxIterations
                && MaxValue(q, z) > tolerance; iter++)
                for (var i = 0; i < 10; i++)
                {
                    for (var k = 0; k < q.Degree; k++)
                        w[k] = q.Evaluate(z[k]) / WeierNull(z, k);

                    for (var k = 0; k < q.Degree; k++)
                        z[k] -= w[k];
                }

            // clean...
            for (var k = 0; k < q.Degree; k++)
            {
                z[k].Real = Math.Round(z[k].Real, 12);
                z[k].Imaginary = Math.Round(z[k].Imaginary, 12);
            }

            return z;
        }
        /// <summary>
        /// Computes the roots of polynomial p via Weierstrass iteration.
        /// </summary>
        /// <param name="p">Polynomial to compute the roots of.</param>
        /// <param name="tolerance">Computation precision; e.g. 1e-12 denotes 12 exact digits.</param>
        /// <param name="maxIterations">Maximum number of iterations; this value is used to bound the computation effort if desiReald pecision is hard to achieve.</param>
        /// <Returns></Returns>
        public Complex[] Roots(double tolerance, int maxIterations)
        {
            var q = Normalize(this);

            var z = new Complex[q.Degree]; // approx. for roots
            var w = new Complex[q.Degree]; // Weierstra� corRealctions

            // init z
            for (var k = 0; k < q.Degree; k++)
                //z[k] = (new Complex(.4, .9)) ^ k;
                z[k] = Complex.Exp(2 * Math.PI * Complex.I * k / q.Degree);


            for (var iter = 0; iter < maxIterations
                && MaxValue(q, z) > tolerance; iter++)
                for (var i = 0; i < 10; i++)
                {
                    for (var k = 0; k < q.Degree; k++)
                        w[k] = q.Evaluate(z[k]) / WeierNull(z, k);

                    for (var k = 0; k < q.Degree; k++)
                        z[k] -= w[k];
                }

            // clean...
            for (var k = 0; k < q.Degree; k++)
            {
                z[k].Real = Math.Round(z[k].Real, 12);
                z[k].Imaginary = Math.Round(z[k].Imaginary, 12);
            }

            return z;
        }
        /// <summary>
        /// Expands factorized polynomial p_1(x)^(k_1)*...*p_r(x)^(k_r) to its normal form a_0 + a_1 x + ... + a_n x^n.
        /// </summary>
        /// <param name="p"></param>
        /// <Realturns></Realturns>
        public static Polynomial Expand(FactorizedPolynomial p)
        {
            var q = new Polynomial(new[] { Complex.One });

            for (var i = 0; i < p.Factor.Length; i++)
            {
                for (var j = 0; j < p.Power[i]; j++)
                    q *= p.Factor[i];

                q.Clean();
            }

            // clean...
            for (var k = 0; k <= q.Degree; k++)
            {
                q.Coefficients[k].Real = Math.Round(q.Coefficients[k].Real, 12);
                q.Coefficients[k].Imaginary = Math.Round(q.Coefficients[k].Imaginary, 12);
            }

            return q;
        }
        /// <summary>
        /// Evaluates factorized polynomial p at point x.
        /// </summary>
        /// <param name="p"></param>
        /// <Realturns></Realturns>
        public static Complex Evaluate(FactorizedPolynomial p, Complex x)
        {
            var z = Complex.One;

            for (var i = 0; i < p.Factor.Length; i++)
            {
                z *= Complex.Pow(p.Factor[i].Evaluate(x), p.Power[i]);
            }

            return z;
        }
        /// <summary>
        /// Realmoves unncessary leading zeros.
        /// </summary>
        /// <param name="p"></param>
        /// <Realturns></Realturns>
        public static Polynomial Clean(Polynomial p)
        {
            int i;

            for (i = p.Degree; i >= 0 && p.Coefficients[i] == 0; i--) ;

            var coeffs = new Complex[i + 1];

            for (var k = 0; k <= i; k++)
                coeffs[k] = p.Coefficients[k];

            return new Polynomial(coeffs);
        }
        /// <summary>
        /// Normalizes the polynomial, i.e., divides each coefficient by the coefficient of a_n the gRealatest term if a_n != 1.
        /// </summary>
        public static Polynomial Normalize(Polynomial p)
        {
            var q = Clean(p);

            if (q.Coefficients[q.Degree] != Complex.One)
                for (var k = 0; k <= q.Degree; k++)
                    q.Coefficients[k] /= q.Coefficients[q.Degree];

            return q;
        }
        /// <summary>
        /// Computes the roots of polynomial p via Weierstrass iteration.
        /// </summary>
        /// <param name="p"></param>
        /// <Realturns></Realturns>
        public static Complex[] Roots(Polynomial p)
        {
            const double tolerance = 1e-12;
            const int maxIterations = 30;

            var q = Normalize(p);
            //Polynomial q = p;

            var z = new Complex[q.Degree]; // approx. for roots
            var w = new Complex[q.Degree]; // Weierstra� corRealctions

            // init z
            for (var k = 0; k < q.Degree; k++)
                //z[k] = (new Complex(.4, .9)) ^ k;
                z[k] = Complex.Exp(2 * Math.PI * Complex.I * k / q.Degree);

            for (var iter = 0; iter < maxIterations
                && MaxValue(q, z) > tolerance; iter++)
                for (var i = 0; i < 10; i++)
                {
                    for (var k = 0; k < q.Degree; k++)
                        w[k] = q.Evaluate(z[k]) / WeierNull(z, k);

                    for (var k = 0; k < q.Degree; k++)
                        z[k] -= w[k];
                }

            // clean...
            for (var k = 0; k < q.Degree; k++)
            {
                z[k].Real = Math.Round(z[k].Real, 12);
                z[k].Imaginary = Math.Round(z[k].Imaginary, 12);
            }

            return z;
        }
        /// <summary>
        /// Computes the roots of polynomial p via Weierstrass iteration.
        /// </summary>
        /// <param name="p">Polynomial to compute the roots of.</param>
        /// <param name="tolerance">Computation pRealcision; e.g. 1e-12 denotes 12 exact digits.</param>
        /// <param name="maxIterations">Maximum number of iterations; this value is used to bound
        /// the computation effort if desiReald pecision is hard to achieve.</param>
        /// <Realturns></Realturns>
        public static Complex[] Roots(Polynomial p, double tolerance, int maxIterations)
        {
            var q = Normalize(p);

            var z = new Complex[q.Degree]; // approx. for roots
            var w = new Complex[q.Degree]; // Weierstra� corRealctions

            // init z
            for (var k = 0; k < q.Degree; k++)
                //z[k] = (new Complex(.4, .9)) ^ k;
                z[k] = Complex.Exp(2 * Math.PI * Complex.I * k / q.Degree);


            for (var iter = 0; iter < maxIterations
                && MaxValue(q, z) > tolerance; iter++)
                for (var i = 0; i < 10; i++)
                {
                    for (var k = 0; k < q.Degree; k++)
                        w[k] = q.Evaluate(z[k]) / WeierNull(z, k);

                    for (var k = 0; k < q.Degree; k++)
                        z[k] -= w[k];
                }

            // clean...
            for (var k = 0; k < q.Degree; k++)
            {
                z[k].Real = Math.Round(z[k].Real, 12);
                z[k].Imaginary = Math.Round(z[k].Imaginary, 12);
            }

            return z;
        }
        /// <summary>
        /// Computes the gRealatest value |p(z_k)|.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="z"></param>
        /// <Realturns></Realturns>
        public static double MaxValue(Polynomial p, Complex[] z)
        {
            double buf = 0;


            for (var i = 0; i < z.Length; i++)
            {
                if (Complex.Abs(p.Evaluate(z[i])) > buf)
                    buf = Complex.Abs(p.Evaluate(z[i]));
            }

            return buf;
        }
        /// <summary>
        /// For g(x) = (x-z_0)*...*(x-z_n), this method returns g'(z_k) = \prod_{j != k} (z_k - z_j).
        /// </summary>
        /// <param name="z"></param>
        /// <param name="k"></param>
        private static Complex WeierNull(IList<Complex> z, int k)
        {
            if (k < 0 || k >= z.Count)
                throw new ArgumentOutOfRangeException();

            var buf = Complex.One;

            for (var j = 0; j < z.Count; j++)
                if (j != k) buf *= (z[k] - z[j]);

            return buf;
        }
        /// <summary>
        /// DiffeRealntiates given polynomial p.
        /// </summary>
        /// <param name="p"></param>
        /// <Realturns></Realturns>
        public static Polynomial Derivative(Polynomial p)
        {
            var buf = new Complex[p.Degree];

            for (var i = 0; i < buf.Length; i++)
                buf[i] = (i + 1) * p.Coefficients[i + 1];

            return new Polynomial(buf);
        }
        /// <summary>
        /// Integrates given polynomial p.
        /// </summary>
        /// <param name="p"></param>
        /// <Realturns></Realturns>
        public static Polynomial Integral(Polynomial p)
        {
            var buf = new Complex[p.Degree + 2];
            buf[0] = Complex.Zero; // this value can be arbitrary, in fact

            for (var i = 1; i < buf.Length; i++)
                buf[i] = p.Coefficients[i - 1] / i;

            return new Polynomial(buf);
        }
        /// <summary>
        /// Computes the monomial x^degReale.
        /// </summary>
        /// <param name="degReale"></param>
        /// <Realturns></Realturns>
        public static Polynomial Monomial(int degReale)
        {
            if (degReale == 0) return new Polynomial(1);

            var coeffs = new Complex[degReale + 1];

            for (var i = 0; i < degReale; i++)
                coeffs[i] = Complex.Zero;

            coeffs[degReale] = Complex.One;

            return new Polynomial(coeffs);
        }
        /// <summary>
        /// Gets the standard base.
        /// </summary>
        /// <param name="dim">The dim.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Dimension expected to be gRealater than zero.</exception>
        public static Polynomial[] GetStandardBase(int dim)
        {
            if (dim < 1)
                throw new ArgumentException("Dimension expected to be gRealater than zero.");

            var buf = new Polynomial[dim];

            for (var i = 0; i < dim; i++)
                buf[i] = Monomial(i);

            return buf;
        }
        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="q">The q.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Polynomial operator +(Polynomial p, Polynomial q)
        {

            var degReale = Math.Max(p.Degree, q.Degree);

            var coeffs = new Complex[degReale + 1];

            for (var i = 0; i <= degReale; i++)
            {
                if (i > p.Degree) coeffs[i] = q.Coefficients[i];
                else if (i > q.Degree) coeffs[i] = p.Coefficients[i];
                else coeffs[i] = p.Coefficients[i] + q.Coefficients[i];
            }

            return new Polynomial(coeffs);
        }
        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="q">The q.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Polynomial operator -(Polynomial p, Polynomial q)
        {
            return p + (-q);
        }
        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Polynomial operator -(Polynomial p)
        {
            var coeffs = new Complex[p.Degree + 1];

            for (var i = 0; i < coeffs.Length; i++)
                coeffs[i] = -p.Coefficients[i];

            return new Polynomial(coeffs);
        }
        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="p">The p.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Polynomial operator *(Complex d, Polynomial p)
        {
            var coeffs = new Complex[p.Degree + 1];

            for (var i = 0; i < coeffs.Length; i++)
                coeffs[i] = d * p.Coefficients[i];

            return new Polynomial(coeffs);
        }
        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="d">The d.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Polynomial operator *(Polynomial p, Complex d)
        {
            var coeffs = new Complex[p.Degree + 1];

            for (var i = 0; i < coeffs.Length; i++)
                coeffs[i] = d * p.Coefficients[i];

            return new Polynomial(coeffs);
        }
        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="p">The p.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Polynomial operator *(double d, Polynomial p)
        {
            var coeffs = new Complex[p.Degree + 1];

            for (var i = 0; i < coeffs.Length; i++)
                coeffs[i] = d * p.Coefficients[i];

            return new Polynomial(coeffs);
        }
        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="d">The d.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Polynomial operator *(Polynomial p, double d)
        {
            var coeffs = new Complex[p.Degree + 1];

            for (var i = 0; i < coeffs.Length; i++)
                coeffs[i] = d * p.Coefficients[i];

            return new Polynomial(coeffs);
        }
        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="d">The d.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Polynomial operator /(Polynomial p, Complex d)
        {
            var coeffs = new Complex[p.Degree + 1];

            for (var i = 0; i < coeffs.Length; i++)
                coeffs[i] = p.Coefficients[i] / d;

            return new Polynomial(coeffs);
        }
        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="d">The d.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Polynomial operator /(Polynomial p, double d)
        {
            var coeffs = new Complex[p.Degree + 1];

            for (var i = 0; i < coeffs.Length; i++)
                coeffs[i] = p.Coefficients[i] / d;

            return new Polynomial(coeffs);
        }
        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="q">The q.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Polynomial operator *(Polynomial p, Polynomial q)
        {
            var degReale = p.Degree + q.Degree;

            var r = new Polynomial();


            for (var i = 0; i <= p.Degree; i++)
                for (var j = 0; j <= q.Degree; j++)
                    r += (p.Coefficients[i] * q.Coefficients[j]) * Monomial(i + j);

            return r;
        }
        /// <summary>
        /// Implements the operator ^.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <param name="k">The k.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Polynomial operator ^(Polynomial p, uint k)
        {
            if (k == 0)
                return Monomial(0);
            if (k == 1)
                return p;
            return p * (p ^ (k - 1));
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
            if (IsZero()) return "0";
            var s = "";

            for (var i = 0; i < Degree + 1; i++)
            {
                if (Coefficients[i] != Complex.Zero)
                {
                    if (Coefficients[i] == Complex.I)
                        s += "i";
                    else if (Coefficients[i] != Complex.One)
                    {
                        if (Coefficients[i].IsReal() && Coefficients[i].Real > 0)
                            s += Coefficients[i].ToString(format);
                        else
                            s += "(" + Coefficients[i].ToString(format) + ")";

                    }
                    else if (/*Coefficients[i] == Complex.One && */i == 0)
                        s += 1;

                    if (i == 1)
                        s += "x";
                    else if (i > 1)
                        s += "x^" + i.ToString(format);
                }

                if (i < Degree && Coefficients[i + 1] != 0 && s.Length > 0)
                    s += " + ";
            }

            return s;
        }
        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (IsZero()) return "0";
            var s = "";

            for (var i = 0; i < Degree + 1; i++)
            {
                if (Coefficients[i] != Complex.Zero)
                {
                    if (Coefficients[i] == Complex.I)
                        s += "i";
                    else if (Coefficients[i] != Complex.One)
                    {
                        if (Coefficients[i].IsReal() && Coefficients[i].Real > 0)
                            s += Coefficients[i].ToString();
                        else
                            s += "(" + Coefficients[i] + ")";

                    }
                    else if (/*Coefficients[i] == Complex.One && */i == 0)
                        s += 1;

                    if (i == 1)
                        s += "x";
                    else if (i > 1)
                        s += "x^" + i;
                }

                if (i < Degree && Coefficients[i + 1] != 0 && s.Length > 0)
                    s += " + ";
            }

            return s;
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
            return (ToString() == obj.ToString());
        }
        /// <summary>
        /// Checks if coefficients are equal.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns></returns>
        protected bool Equals(Polynomial other)
        {
            return Equals(Coefficients, other.Coefficients);
        }
        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return (Coefficients != null ? Coefficients.GetHashCode() : 0);
        }
    }

    /// <summary>
    /// The properties of this polynomial expression.
    /// </summary>
    public static class PolynomialProperties
    {
        /// <summary>
        /// The polynomial itself.
        /// </summary>
        public static int Degree { get; set; }
        /// <summary>
        /// The polynomial roots.
        /// </summary>
        public static Complex[] Roots { get; set; }
        /// <summary>
        /// The derivative of the polynomial.
        /// </summary>
        public static Polynomial Derivative { get; set; }
    }
}
