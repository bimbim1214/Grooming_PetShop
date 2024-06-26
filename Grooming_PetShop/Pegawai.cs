﻿using System;
using System.Data.SqlClient;

namespace GroomingPetShop
{
    internal class Pegawai
    {
        private string connectionString = "Data Source=BIMO-ADITYA-14P\\BIMO_ADITYA;Initial Catalog={0};User ID=sa;Password=bimbimbom";

        public void Main()
        {
            Pegawai pegawaiHandler = new Pegawai();

            while (true)
            {
                try
                {
                    Console.Write("\nKetik 'k' untuk terhubung ke database atau 'E' untuk keluar dari aplikasi: ");
                    char chr = Char.ToUpper(Console.ReadKey().KeyChar);
                    Console.WriteLine();

                    switch (chr)
                    {
                        case 'K':
                            Console.Clear();
                            Console.WriteLine("Masukkan nama database yang dituju kemudian tekan Enter: ");
                            string dbName = Console.ReadLine().Trim();

                            using (SqlConnection conn = new SqlConnection(string.Format(connectionString, dbName)))
                            {
                                conn.Open();
                                Console.Clear();

                                while (true)
                                {
                                    Console.WriteLine("\nMenu");
                                    Console.WriteLine("1. Melihat Seluruh Data Pegawai");
                                    Console.WriteLine("2. Tambah Data Pegawai");
                                    Console.WriteLine("3. Hapus Data Pegawai");
                                    Console.WriteLine("4. Cari Data Pegawai");
                                    Console.WriteLine("5. Perbarui Data Pegawai");
                                    Console.WriteLine("6. Keluar");
                                    Console.WriteLine("\nEnter your choice (1-6): ");

                                    char ch = Char.ToUpper(Console.ReadKey().KeyChar);
                                    Console.WriteLine();

                                    switch (ch)
                                    {
                                        case '1':
                                            Console.Clear();
                                            Console.WriteLine("Data Pegawai\n");
                                            pegawaiHandler.ReadEmployees(conn);
                                            break;
                                        case '2':
                                            Console.Clear();
                                            pegawaiHandler.InsertEmployee(conn);
                                            break;
                                        case '3':
                                            Console.Clear();
                                            pegawaiHandler.DeleteEmployee(conn);
                                            break;
                                        case '4':
                                            Console.Clear();
                                            pegawaiHandler.SearchEmployee(conn);
                                            break;
                                        case '5':
                                            Console.Clear();
                                            pegawaiHandler.UpdateEmployee(conn);
                                            break;
                                        case '6':
                                            conn.Close();
                                            Console.Clear();
                                            Console.WriteLine("Exiting application...");
                                            return;
                                        default:
                                            Console.Clear();
                                            Console.WriteLine("\nInvalid option");
                                            break;
                                    }
                                }
                            }
                            break;

                        case 'E':
                            Console.WriteLine("Exiting application...");
                            return;

                        default:
                            Console.WriteLine("\nInvalid option");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Error: {ex.Message}\n");
                    Console.ResetColor();
                }
            }
        }

        public void ReadEmployees(SqlConnection con)
        {
            string query = "SELECT Id_pegawai, Nama_pegawai FROM Pegawai";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID: {reader.GetString(0)}, Nama: {reader.GetString(1)}");
                    }
                }
            }
        }

        public void InsertEmployee(SqlConnection con)
        {
            Console.WriteLine("Input data Pegawai\n");
            Console.WriteLine("Masukkan ID Pegawai (8 karakter): ");
            string id = Console.ReadLine();

            Console.WriteLine("Masukkan Nama Pegawai (tanpa angka): ");
            string nama = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(nama) || ContainsNumbers(nama))
            {
                Console.WriteLine("Nama Pegawai tidak valid. Harap masukkan nama tanpa angka.");
                return;
            }

            string insertQuery = "INSERT INTO Pegawai (Id_pegawai, Nama_pegawai) VALUES (@id, @nama)";

            using (SqlCommand cmd = new SqlCommand(insertQuery, con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@nama", nama);

                cmd.ExecuteNonQuery();
                Console.WriteLine("Data Pegawai berhasil ditambahkan");
            }
        }

        public void DeleteEmployee(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Pegawai yang ingin dihapus: ");
            string idToDelete = Console.ReadLine();

            string query = "DELETE FROM Pegawai WHERE Id_pegawai = @id";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", idToDelete);
            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
                Console.WriteLine("Data Pegawai berhasil dihapus");
            else
                Console.WriteLine("Data Pegawai dengan ID tersebut tidak ditemukan");
        }

        public void SearchEmployee(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Pegawai yang ingin dicari: ");
            string idToSearch = Console.ReadLine();

            string query = "SELECT Id_pegawai, Nama_pegawai FROM Pegawai WHERE Id_pegawai = @id";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", idToSearch);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine($"ID: {reader.GetString(0)}, Nama: {reader.GetString(1)}");
                }
                else
                {
                    Console.WriteLine("Data Pegawai tidak ditemukan");
                }
            }
        }

        public void UpdateEmployee(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Pegawai yang ingin diperbarui: ");
            string idToUpdate = Console.ReadLine();

            string selectQuery = "SELECT Nama_pegawai FROM Pegawai WHERE Id_pegawai = @id";
            SqlCommand selectCmd = new SqlCommand(selectQuery, con);
            selectCmd.Parameters.AddWithValue("@id", idToUpdate);

            using (SqlDataReader reader = selectCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string currentNama = reader.GetString(0);

                    Console.WriteLine($"Data saat ini - Nama: {currentNama}");

                    // Tutup SqlDataReader sebelum menjalankan perintah UPDATE
                    reader.Close(); // Tutup SqlDataReader di sini

                    Console.WriteLine("\nMasukkan informasi baru:");

                    Console.WriteLine("Nama Pegawai (kosongkan jika tidak ingin mengubah): ");
                    string newNama = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newNama))
                        newNama = currentNama;

                    string updateQuery = "UPDATE Pegawai SET Nama_pegawai = @nama WHERE Id_pegawai = @id";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                    updateCmd.Parameters.AddWithValue("@id", idToUpdate);
                    updateCmd.Parameters.AddWithValue("@nama", newNama);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        Console.WriteLine("Data Pegawai berhasil diperbarui");
                    else
                        Console.WriteLine("Data Pegawai gagal diperbarui");
                }
                else
                {
                    Console.WriteLine("Data Pegawai tidak ditemukan");
                }
            }
        }

        private bool ContainsNumbers(string input)
        {
            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
