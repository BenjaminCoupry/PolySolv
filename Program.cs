using System;
using System.Collections.Generic;
using System.Linq;


namespace PolySolv
{
    class Program
    {
        static void Main(string[] args)
        {
            Tester_Racines(100);
        }
        //Gestion des complexes
        public static double Norme(double re, double im)
        {
            return Math.Sqrt(Math.Pow(re, 2) + Math.Pow(im, 2));
        }
        public static double Argument(double re, double im)
        {
            return 2.0 * Math.Atan(im / (re + Norme(re, im)));
        }
        public static double RacineCubique(double a)
        {
            return Math.Sign(a) * Math.Pow(Math.Abs(a), 1.0 / 3.0);
        }
        //Resolution des problemes simplifies
        public static List<double> SolProbDeg3(double p, double q)
        {
            //sol de x^3+px+q=0
            List<double> solutions = new List<double>();
            if (p == 0 && q == 0)
            {
                //x^3=0
                solutions.Add(0.0);
                return solutions;
            }
            else if (p == 0)
            {
                //x^3 + q =0
                solutions.Add(-RacineCubique(q));
                return solutions;
            }
            else if (q == 0)
            {
                //x^3+px=0 => x(x^2+p) =0
                solutions.Add(0);
                if (p < 0)
                {
                    solutions.Add(-Math.Sqrt(-p));
                    solutions.Add(Math.Sqrt(-p));
                }
                return solutions;
            }
            else
            {
                double D = Math.Pow(q, 2) / 4.0 + Math.Pow(p, 3) / 27.0;
                double alpha = 0;
                if (D >= 0)
                {
                    //trouver la premiere solution
                    double a1 = (-q / 2.0) + Math.Sqrt(D);
                    double a2 = (-q / 2.0) - Math.Sqrt(D);
                    double x1 = RacineCubique(a1) + RacineCubique(a2);
                    alpha = x1;
                    solutions.Add(alpha);
                }
                else if (D < 0)
                {
                    double Nrme = Norme((-q / 2.0), Math.Sqrt(-D));
                    double arg = Argument((-q / 2.0), Math.Sqrt(-D));
                    double partReelle = RacineCubique(Nrme) * Math.Cos(arg / 3.0);
                    alpha = 2.0 * partReelle;
                    solutions.Add(alpha);
                }
                //trouver les autres racines
                double b = alpha;
                double c = Math.Pow(alpha, 2) + p;
                List<double> poly2 = RacinesPol2(1.0, b, c);
                for (int i = 0; i < poly2.Count; i++)
                {
                    solutions.Add(poly2.ElementAt(i));
                }
                return solutions;
            }
        }
        public static List<double> SolProbDeg4(double p, double q, double r)
        {
            //Sol de x^4+px^2+qx+r
            List<double> solutions = new List<double>();
            if (r == 0)
            {
                //x^4+qx=0 => x*(x^3+px+q)=0
                solutions = RacinesPol3(1, 0, p, q);
                solutions.Add(0.0);
                return solutions;
            }
            else
            {


                //recherche des cubiques resolvantes
                List<double> CubRel = RacinesPol3(8.0, -4.0 * p, -8.0 * r, 4.0 * r * p - q * q);
                //recherche de la plus grande des cubiques resolvantes
                double pgr = CubRel.ElementAt(0);
                for (int i = 1; i < CubRel.Count; i++)
                {
                    double s = CubRel.ElementAt(i);
                    if (s > pgr || (2.0 * pgr - p) == 0)
                    {
                        if ((2.0 * s - p) != 0)
                        {
                            pgr = s;
                        }
                    }
                }
                //calcul de a0 et b0
                //ici a0 vaut en réalité a0^2
                double a0 = 2.0 * pgr - p;
                if (a0 <= 0.0)
                {
                    //pas de cub rel trouvée de sorte que a0^2 soit non nul et positif
                    return solutions;
                }
                else
                {
                    //a0 prend sa vraie valeur
                    a0 = Math.Sqrt(a0);
                    double b0 = -q / (2.0 * a0);
                    solutions = RacinesPol2(1, a0, pgr + b0).Concat(RacinesPol2(1, -a0, pgr - b0)).ToList();
                    return solutions;
                }
            }
        }
        //Racines des polynomes de degre 2 a 4
        public static List<double> RacinesPol2(double a, double b, double c)
        {
            //ax^2+bx+c = 0
            List<double> solutions = new List<double>();
            if (a != 0)
            {
                double delta = b * b - 4 * a * c;
                if (delta > 0)
                {
                    double x1 = (-b + Math.Sqrt(delta)) / (2.0 * a);
                    double x2 = (-b - Math.Sqrt(delta)) / (2.0 * a);
                    solutions.Add(x1);
                    solutions.Add(x2);
                }
                else if (delta == 0)
                {
                    double x0 = -b / (2.0 * a);
                    solutions.Add(x0);
                }
            }
            else
            {
                if (b != 0)
                {
                    solutions.Add(-c / b);
                }
            }
            return solutions;

        }
        public static List<double> RacinesPol3(double a, double b, double c, double d)
        {
            //ax^3+bx^2+cx+d=0
            List<double> solutions = new List<double>();
            if (a != 0)
            {
                double p = (c / a) - (1.0 / 3.0) * Math.Pow(b / a, 2);
                double q = (2.0 / 27.0) * Math.Pow(b / a, 3) + (d / a) - (b * c / (Math.Pow(a, 2) * 3.0));
                List<double> sol1 = SolProbDeg3(p, q);
                for (int i = 0; i < sol1.Count; i++)
                {
                    solutions.Add(sol1.ElementAt(i) - (b / (3.0 * a)));
                }
            }
            else
            {
                solutions = RacinesPol2(b, c, d);
            }
            return solutions;
        }
        public static List<double> RacinesPol4(double a, double b, double c, double d, double e)
        {
            //ax ^ 4 + bx ^ 3 + cx^2 + dx + e = 0
            List<double> solutions = new List<double>();
            if (a != 0)
            {
                //on divide par a
                double bred = b / a;
                double cred = c / a;
                double dred = d / a;
                double ered = e / a;
                double p = cred - (3.0 * Math.Pow(bred, 2) / 8.0);
                double q = (Math.Pow(bred, 3) / 8.0) - (bred * cred / 2.0) + dred;
                double r = ered - (bred * dred / 4.0) + (Math.Pow(bred, 2) * cred / 16.0) - (3.0 * Math.Pow(bred, 4) / 256);
                List<double> sol1 = SolProbDeg4(p, q, r);
                for (int i = 0; i < sol1.Count; i++)
                {
                    solutions.Add(sol1.ElementAt(i) - (bred / (4.0)));
                }
            }
            else
            {
                solutions = RacinesPol3(b, c, d, e);
            }
            return solutions;
        }
        //Fonction de test
        public static void Tester_Racines(int n)
        {
            Random r = new Random();
            for (int i = 0; i < n; i++)
            {
                int[] degSelector = new int[4];
                int deg = r.Next(1, 5);
                for (int k = 0; k < deg; k++)
                {
                    degSelector[k] = 1;
                }
                for (int k = deg; k < 4; k++)
                {
                    degSelector[k] = 0;
                }
                double a = (r.NextDouble() * 2.0 - 1.0) * (double)degSelector[3];
                double b = (r.NextDouble() * 2.0 - 1.0) * (double)degSelector[2];
                double c = (r.NextDouble() * 2.0 - 1.0) * (double)degSelector[1];
                double d = (r.NextDouble() * 2.0 - 1.0) * (double)degSelector[0];
                double e = r.NextDouble() * 2.0 - 1.0;
                List<double> solut = RacinesPol4(a, b, c, d, e);
                int nbSolut = solut.Count;
                Console.WriteLine("");
                Console.WriteLine("(degre " + deg + ")");
                Console.WriteLine("E" + i + " : " + a + " x^4 + " + b + " x^3 + " + c + " x^2 + " + d + " x + " + e + " = 0");
                Console.WriteLine("=> Racines reelles : " + nbSolut);
                for (int j = 0; j < solut.Count; j++)
                {
                    double s = solut.ElementAt(j);
                    double rs = a * Math.Pow(s, 4) + b * Math.Pow(s, 3) + c * Math.Pow(s, 2) + d * s + e;
                    double lgrs = -Math.Log10(Math.Abs(rs));
                    string strlgrs = "";
                    if (double.IsInfinity(lgrs))
                    {
                        strlgrs = "Exact";
                    }
                    else
                    {
                        strlgrs = (Math.Round(lgrs * 100.0) / 100.0).ToString();
                    }
                    Console.WriteLine("    S" + j + " : " + s);
                    Console.WriteLine("        -> resultat = " + rs);
                    Console.WriteLine("        -> precision = " + strlgrs);
                }
            }
        }
    }
}
