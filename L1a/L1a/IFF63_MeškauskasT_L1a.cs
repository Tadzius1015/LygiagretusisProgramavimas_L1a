// Tadas Meškauskas IFF-6/3 L1a darbas
// 1.Kokia tvarka jos yra užrašytos.
// 2.Atsitiktine.
// 3.Atsitiktinį skaičių.
// 4.Atsitiktine.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Data;

namespace L1a
{
    class IFF63_MeškauskasT_L1a
    {
        private static List<String> poGiju = new List<string>(); // Saugomi rezultatai po darbo su gijomis.
        static void Main(string[] args)
        { 
            if(File.Exists("IFF-63_MeškauskasT_L1a_rez.txt"))
            {
                File.Delete("IFF-63_MeškauskasT_L1a_rez.txt");
            }
            List<Parduotuve> parduotuves = new List<Parduotuve>();
            parduotuves = Skaitymas();
            Lygiagretinimas(parduotuves);
        }
        /// <summary>
        /// Skaitymo metodas
        /// </summary>
        /// <returns></returns>
        public static List<Parduotuve> Skaitymas()
        {
            List<Parduotuve> pard = new List<Parduotuve>();
            string failas = "IFF_63_MeskauskasT_L1a_skait.txt"; // Failo kelias.
            string[] eilutes = File.ReadAllLines(failas);
            for (int i = 0; i < eilutes.Count(); i++)
            {
                if(eilutes[i].IndexOf('*') == 0) // tikrinama sąlyga, ar eilutės 0 indeksas yra * simbolis
                {
                    pard.Add(new Parduotuve(eilutes[i]));
                    continue;
                }
                pard.Last(); // gražinama paskutinioji parduotuvė
                pard[pard.Count - 1].Dvir.Add(new Dviratis(eilutes[i])); // pridedami dviračiai į konkrečią parduotuvę.
            }
            return pard;
        }
        /// <summary>
        /// Metodas, kuris atlieka tam tikras funkcijas gijose.
        /// </summary>
        /// <param name="pard">parduotuvių sąrašas</param>
        static void Lygiagretinimas(List<Parduotuve> pard)
        {            
            ThreadStart ts = delegate { RasymasISarasa(pard, 0); };
            Thread aa = new Thread(ts);
            ThreadStart ts1 = delegate { RasymasISarasa(pard, 1); };
            Thread aa1 = new Thread(ts1);
            ThreadStart ts2 = delegate { RasymasISarasa(pard, 2); };
            Thread aa2 = new Thread(ts2);
            ThreadStart ts3 = delegate { RasymasISarasa(pard, 3); };
            Thread aa3 = new Thread(ts3);
            ThreadStart ts4 = delegate { RasymasISarasa(pard, 4); };
            Thread aa4 = new Thread(ts4);
            aa.Start();
            aa1.Start();
            aa2.Start();
            aa3.Start();
            aa4.Start();
            /*aa.Join();
            aa1.Join();
            aa2.Join();
            aa3.Join();
            aa4.Join();*/
            using (StreamWriter rasytojas = new StreamWriter("IFF-63_MeškauskasT_L1a_rez.txt"))
            {
                foreach (var par in pard)
                {
                    rasytojas.WriteLine(par.Pavad);
                    rasytojas.WriteLine(new string('-', 100));
                    rasytojas.WriteLine("{0,6} {1,15} {2,7} {3,8}","Eil.Nr", "Gamintojas", "Kiekis", "Kaina");
                    for (int i = 0; i < par.Dvir.Count; i++)
                    {
                        rasytojas.WriteLine("{0,6}" + par.Dvir[i].ToString(), i);
                    }
                    rasytojas.WriteLine(new string('-', 100));
                }
                rasytojas.WriteLine(new string('-', 100));
                rasytojas.WriteLine("Po gijų:");
                rasytojas.WriteLine(new string('-', 100));
                rasytojas.WriteLine("{0,9} {1,13} {2,15} {3,7} {4,8}", "Gijos Pav", "Gijos numeris", "Gamintojas", "Kiekis", "Kaina");
                foreach (var eil in poGiju)
                {
                    rasytojas.WriteLine(eil);
                }
            }
        }
        /// <summary>
        /// Metodas, kuris suraso gijos informacija į String sąrašą.
        /// </summary>
        /// <param name="pard">parduotuvių sąrašas</param>
        /// <param name="i">i-tosios parduotuvės indeksas</param>
        static void RasymasISarasa(List<Parduotuve> pard, int i)
        {
            for (int j = 0; j < pard[i].Dvir.Count; j++)
            {
                poGiju.Add(String.Format("{0,9} {1,13}" + pard[i].Dvir[j].ToString(), "gija" + i, j));
            }
        }
    }
    class Dviratis
    {
        public string Gamintojas { get; set; }
        public int Kiekis { get; set; }
        public double Kaina { get; set; }

        public Dviratis()
        {

        }
        public Dviratis(string gamintojas, int kiekis, double kaina)
        {
            this.Gamintojas = gamintojas;
            this.Kiekis = kiekis;
            this.Kaina = kaina;
        }
        public Dviratis(string eilute)
        {
            string[] dalys = eilute.Split(' ');
            Gamintojas = dalys[0];
            Kiekis = int.Parse(dalys[1]);
            Kaina = double.Parse(dalys[2]);
        }
        public override string ToString()
        {
            return String.Format("{0,15} {1,7} {2,8}", Gamintojas, Kiekis, Kaina);
        }
    }
    class Parduotuve : List<Dviratis>
    {
        public string Pavad;
        public List<Dviratis> Dvir;

        public Parduotuve(string pavad)
        {
            this.Pavad = pavad;
            this.Dvir = new List<Dviratis>();
        }
        public Parduotuve() { }
    }
}
