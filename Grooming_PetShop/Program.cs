using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroomingPetShop
{
    internal class main
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n ======= Halaman Utama ======= ");
                Console.WriteLine("1. Menu pegawai");
                Console.WriteLine("2. Menu pelanggan");
                Console.WriteLine("3. Menu kucing");
                Console.WriteLine("4. Menu Kepemilikan");
                Console.WriteLine("5. Menu Grooming");
                Console.WriteLine("6. Menu Laporan");

                Console.Write("Masukan Pilihan (1-6) : ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Pegawai pegawai = new Pegawai();
                        pegawai.Main();
                        break;
                    case "2":
                        Pelanggan pelanggan = new Pelanggan();
                        pelanggan.Main();
                        break;
                    case "3":
                        Kucing kucing = new Kucing();
                        kucing.Main();
                        break;
                    case "4":
                        Kepemilikan Kepemilikan = new Kepemilikan();
                        Kepemilikan.Main();
                        break;
                    case "5":
                        Grooming Grooming = new Grooming();
                        Grooming.Main();
                        break;
                    case "6":
                        Laporan Laporan = new Laporan();
                        Laporan.Main();
                        break;
                    default:
                        Console.WriteLine("Pilihan Tidak Tersedia!");
                        break;
                }
            }
        }
    }
}