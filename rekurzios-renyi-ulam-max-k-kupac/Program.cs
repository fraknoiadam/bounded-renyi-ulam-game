using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace rekurzios_renyi_ulam_max_k_kupac
{
    class Program
    {
        const int NAGYSZAM = 1000000;
        const int MERET = 50;
        struct Allas
        {
            public int kerdesszam;
            public List<int[]> nyerolepes;
            public bool ellenpelda;
        }

        //[x0,x1,k]
        static Allas[,,] helyzetek = new Allas[151,151,76];
        static Allas[,,,] helyzetek2 = new Allas[101,101,101,51];
        static StreamWriter sw = new StreamWriter("rekurzios2.txt");
        static void Main(string[] args)
        {
           Helyzetek_1hazug();
           //Helyzetek_2hazug();
        



            nem_mindig_jobbe();
            Console.ReadLine();
        }
        static void nem_mindig_jobbe()
        {
            for (int a = 0; a < MERET; a++)
            {
                for (int b = 0; b < MERET; b++)
                {
                    for (int c = 0; c < MERET; c++)
                    {
                        for (int k = 0; k <= (a+b+c)/2; k++)
                        {
                            if (k <= MERET/2)
                            {
                                Console.WriteLine(a + " " + b + " " + c + " " + k);
                                if(helyzetek2[a, b, c, k].ellenpelda)
                                {
                                    int x = 0;
                                }
                            }
                        }
                    }
                }
            }
        }

        static Allas rekurzio(int x0, int x1, int k)
        {
            Allas valasz;
            valasz.kerdesszam = NAGYSZAM;
            valasz.nyerolepes = new List<int[]>();
            valasz.ellenpelda = false;
            for (int i = 0; i <= k && i<=x0; i++)
            {
                for (int j = 0; j <= k && j<=x1; j++)
                {
                    if(i+j<=k && i + j > 0)//triviális if függvény
                    {
                        if (x1 == 0)
                        {
                            int alma = 1;
                        }
                        int y0 = i;
                        int y1 = x0 - i + j;
                        int z0 = x0 - i;
                        int z1 = x1 + i - j;
                        if(helyzetek[y0,y1,k].kerdesszam+1 < valasz.kerdesszam && helyzetek[z0, z1, k].kerdesszam+1 < valasz.kerdesszam)
                        {
                            //Ez egy jobb kérdés
                            valasz.nyerolepes = new List<int[]>();
                            valasz.kerdesszam = Math.Max(helyzetek[y0, y1, k].kerdesszam + 1, helyzetek[z0, z1, k].kerdesszam + 1);
                            int[] jo_kerdes = { i, j, k };
                            valasz.nyerolepes.Add(jo_kerdes);
                        }
                        else if (helyzetek[y0, y1, k].kerdesszam+1 <= valasz.kerdesszam && helyzetek[z0, z1, k].kerdesszam+1 <= valasz.kerdesszam)
                        {
                            //Ez egy ugyanolyan jó kérdés
                            int[] jo_kerdes = { i, j, k };
                            valasz.nyerolepes.Add(jo_kerdes);
                        }
                    }
                }
            }

            return valasz;
        }

        static Allas rekurzios_lepes2(int[] x, int l, int k)
        {
            Allas valasz;
            valasz.kerdesszam = NAGYSZAM;
            valasz.nyerolepes = new List<int[]>();
            valasz.ellenpelda = true;
            for (int a = 0; a <= k && a <= x[0]; a++)
            {
                for (int b = 0; b <= k && b <= x[1]; b++)
                {
                    for (int c = 0; c <= k && c<=x[2]; c++)
                    {
                        if (a + b + c <= k && a + b + c > 0)//triviális if függvény
                        {
                            if (x[1] == 0)
                            {
                                int alma = 1;
                            }
                            int[] y = new int[l+1];
                            int[] z = new int[l+1];
                            y[0] = a;
                            y[1] = b-a+x[0];
                            y[2] = c-b+x[1];
                            z[0] = x[0] - a;
                            z[1] = x[1] - b + a;
                            z[2] = x[2] - c + b;
                            int nem_eseten = helyzetek2[z[0], z[1], z[2], k].kerdesszam + 1;
                            int igen_eseten = helyzetek2[y[0], y[1], y[2], k].kerdesszam + 1;
                            if (igen_eseten < valasz.kerdesszam && nem_eseten < valasz.kerdesszam)
                            {
                                //Ez egy jobb kérdés
                                valasz.ellenpelda = true;
                                valasz.nyerolepes = new List<int[]>();
                                valasz.kerdesszam = Math.Max(helyzetek2[y[0], y[1], y[2], k].kerdesszam + 1, helyzetek2[z[0], z[1], z[2], k].kerdesszam + 1);
                                int[] jo_kerdes = { a, b, c };
                                valasz.nyerolepes.Add(jo_kerdes);
                                if (igen_eseten <= nem_eseten) valasz.ellenpelda = false;
                            }
                            else if (igen_eseten <= valasz.kerdesszam && nem_eseten <= valasz.kerdesszam)
                            {
                                //Ez egy ugyanolyan jó kérdés
                                int[] jo_kerdes = { a, b, c };
                                valasz.nyerolepes.Add(jo_kerdes);
                                if (igen_eseten <= nem_eseten) valasz.ellenpelda = false;
                            }
                        }
                    }
                }
            }



            return valasz;
        }

        static void Helyzetek_2hazug()
        {
            for (int i = 0; i < 50; i++)
            {
                helyzetek2[0, 0, 0, i].kerdesszam = 0;
                helyzetek2[0, 0, 1, i].kerdesszam = 0;
            }

            //osszeg[0]: x0+x1+x2
            //osszeg[1]: x1+x2
            //osszeg[2]: x2
            int l = 2;  //Hazugságok száma
            int[] osszeg = new int[l+1];
            int teljesosszeg = MERET;


            //A program
            for (int sum = 1; sum <= teljesosszeg; sum++)
            {
                for (int k = 1; k < MERET/2; k++)
                {
                    //Init (minden összeg maximális)
                    for (int i = 0; i <= l; i++)
                    {
                        osszeg[i] = sum;
                    }

                    //Ha (osszeg[0]=)x0+x1+x2=konstans(<=teljesosszeg), akkor megnézi az összes esetet
                    do
                    {
                        //Kiszámolja az x[i] tömböt
                        int[] x = new int[l + 1];
                        for (int i = 0; i < l; i++)
                        {
                            x[i] = osszeg[i] - osszeg[i + 1];
                        }
                        x[l] = osszeg[l];

                        //Rekurziós lépés
                        if (2 * (k - 1) < osszeg[0] && (x[0] > 0 || x[1] > 0 || x[2]>1)) //másodk feltétel azért kell, hogy (0,1)-re 0 választ adjon
                        {
                            helyzetek2[x[0], x[1], x[2], k] = rekurzios_lepes2(x, l, k);
                        }
                        else
                        {
                            helyzetek2[x[0], x[1], x[2], k] = helyzetek2[x[0], x[1], x[2], k - 1];
                        }


                        //Beállítja az összegeket a következő körre
                        int meddig_csokkentsunk = l + 1;//Azért l+1, mert az osszeg[l+1] már nem létezik
                        for (int i = l; i >= 0; i--)
                        {
                            if (osszeg[i] != 0)
                            {
                                osszeg[i]--;
                                break;
                            }
                            else
                            {
                                meddig_csokkentsunk = i;
                            }
                        }
                        for (int i = meddig_csokkentsunk; i <= l; i++)
                        {
                            osszeg[i] = osszeg[meddig_csokkentsunk - 1];
                        }



                    } while (osszeg[0] == sum);
                    Console.WriteLine(sum);
                }
            }

            /*for (int osszeg0 = 1; osszeg0 < 150; osszeg0++)
            {
                for (int osszeg1 = 0; osszeg1 < length; osszeg1++)
                {
                    for (int x0 = 0; x0 <= osszeg0; x0++)
                    {
                        int x1 = osszeg0 - x0;
                        for (int k = 1; k < 75; k++)
                        {
                            if (2 * (k - 1) < x0 + x1 && (x0 > 0 || x1 > 1)) //másodk feltétel azért kell, hogy (0,1)-re 0 választ adjon
                            {
                                helyzetek[x0, x1, k] = rekurzio(x0, x1, k);
                                Console.WriteLine(x0 + "\t" + x1 + "\t" + k + "\t" + helyzetek[x0, x1, k].kerdesszam);
                                sw.Write(x0 + "\t" + x1 + "\t" + k + "\t" + helyzetek[x0, x1, k].kerdesszam);
                                for (int a = 0; a < helyzetek[x0, x1, k].nyerolepes.Count; a++)
                                {
                                    // sw.Write("\t"+helyzetek[x0, x1, k].nyerolepes[a]);
                                }
                                sw.WriteLine();
                            }
                            else
                            {
                                helyzetek[x0, x1, k] = helyzetek[x0, x1, k - 1];
                            }
                        }
                    }
                }
            }*/

        }


        static void Helyzetek_1hazug()
        {
            for (int i = 0; i < 50; i++)
            {
                helyzetek[0, 0, i].kerdesszam = 0;
                helyzetek[0, 1, i].kerdesszam = 0;
            }


            for (int osszeg = 1; osszeg < 150; osszeg++)
            {
                for (int x0 = 0; x0 <= osszeg; x0++)
                {
                    int x1 = osszeg - x0;
                    for (int k = 1; k < 75; k++)
                    {
                        if (2 * (k - 1) < x0 + x1 && (x0 > 0 || x1 > 1)) //másodk feltétel azért kell, hogy (0,1)-re 0 választ adjon
                        {
                            helyzetek[x0, x1, k] = rekurzio(x0, x1, k);
                            Console.WriteLine(x0 + "\t" + x1 + "\t" + k + "\t" + helyzetek[x0, x1, k].kerdesszam);
                            sw.Write(x0 + "\t" + x1 + "\t" + k + "\t" + helyzetek[x0, x1, k].kerdesszam);
                            for (int a = 0; a < helyzetek[x0, x1, k].nyerolepes.Count; a++)
                            {
                                // sw.Write("\t"+helyzetek[x0, x1, k].nyerolepes[a]);
                            }
                            sw.WriteLine();
                        }
                        else
                        {
                            helyzetek[x0, x1, k] = helyzetek[x0, x1, k - 1];
                        }
                    }
                }
            }
        }
    }
}
