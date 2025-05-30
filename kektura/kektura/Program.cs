﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace kektura
{
    public struct turaAdat
    {
        public string kezdoPont, celPont;
        public double tavHossz;
        public int emelkedesek, lejtesek;
        public bool pecsetHely;

    }
    internal class Program
    {
        public static int kezdoMagassag;
        static void Main(string[] args)
        {
            var turaAdatok = adatRead();
            Console.WriteLine($"3. feladat: Szakaszok száma: {turaAdatok.Count()}");
            Console.WriteLine($"4. feladat: A túra teljes hossza: {osszTav(turaAdatok)} km");
            var rovidTura = legrovidebbTav(turaAdatok);
            Console.WriteLine($"5. feladat: A legrövidebb szakasz adatai:\n \tKezdete: {rovidTura.kezdoPont}");
            Console.WriteLine($"\tVége: {rovidTura.celPont}");
            Console.WriteLine($"\tTávolság: {rovidTura.tavHossz} km");
            Console.WriteLine($"7. feladat: Hiányos állomásnevek:\n {hianyosAllomasok(turaAdatok)}");
            var legmagasabb = LegmagasabbPont(turaAdatok);
            Console.WriteLine($"8. feladat: A túra legmagasabban fekvő végpontja:");
            Console.WriteLine($"\t A végpont neve: {legmagasabb.nev}");
            Console.WriteLine($"\t Magasság: {legmagasabb.magassag} m");
            Console.ReadKey();
        }
        public static List<turaAdat> adatRead(string filename = "kektura.csv")
        {
            {
                List<turaAdat> turaAdatok = new List<turaAdat>();
                try
                {
                    StreamReader sr = new StreamReader(filename);
                    kezdoMagassag = int.Parse(sr.ReadLine());
                    while (!sr.EndOfStream)
                    {
                        string sor = sr.ReadLine();
                        string[] sorDb = sor.Split(';');
                        turaAdat ujAdat = new turaAdat();
                        ujAdat.kezdoPont = sorDb[0];
                        ujAdat.celPont = sorDb[1];
                        ujAdat.tavHossz = double.Parse(sorDb[2]);
                        ujAdat.emelkedesek = int.Parse(sorDb[3]);
                        ujAdat.lejtesek = int.Parse(sorDb[4]);
                        string pecset = sorDb[5];
                        if (pecset == "i")
                        {
                            ujAdat.pecsetHely = true;
                        }
                        else
                        {
                            ujAdat.pecsetHely = false;
                        }
                        turaAdatok.Add(ujAdat);
                    }
                    sr.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Az adatReadben hiba történt! {e.Message}");
                    return null;
                }
                return turaAdatok;
            }
        }
        public static double osszTav(List<turaAdat> turaAdatok)
        {
            double ossztav = 0;
            foreach (var adat in turaAdatok)
            {
                ossztav += adat.tavHossz;
            }
            return ossztav;
        }
        public static turaAdat legrovidebbTav(List<turaAdat> turaAdatok)
        {
            turaAdat minTura = turaAdatok[0];
            foreach (var turaAdat in turaAdatok)
            {
                if (turaAdat.tavHossz < minTura.tavHossz)
                {
                    minTura = turaAdat;
                }
            }
            return minTura;
        }
        public static bool hianyosNev(turaAdat turaAdatok)
        {
            return turaAdatok.pecsetHely && !turaAdatok.celPont.ToLower().Contains("pecsetelohely");
        }
        public static string hianyosAllomasok(List<turaAdat> turaAdatok)
        {
            string eredmeny = "";
            foreach (var tura in turaAdatok)
            {
                if (hianyosNev(tura))
                {
                    eredmeny += $"\t{tura.celPont}\n";
                }
            }
            if (eredmeny == "")
            {
                Console.WriteLine("Nincs hiányos állomásnév!");
            }
            return eredmeny.TrimEnd();
        }
        public static (string nev, int magassag) LegmagasabbPont(List<turaAdat> turaAdatok)
        {
            int aktualisMagassag = kezdoMagassag;
            int maxMagassag = aktualisMagassag;
            string maxHely = turaAdatok[0].kezdoPont;

            foreach (var tura in turaAdatok)
            {
                aktualisMagassag += tura.emelkedesek - tura.lejtesek;
                if (aktualisMagassag > maxMagassag)
                {
                    maxMagassag = aktualisMagassag;
                    maxHely = tura.celPont;
                }
            }

            return (maxHely, maxMagassag);
        }
    }
}
