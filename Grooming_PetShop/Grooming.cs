using System;
using System.Data.SqlClient;
namespace GroomingPetShop
{

    internal class Grooming
    {
        public void Main()
        {
            Grooming pr = new Grooming();
            string connectionString = "Data Source=BIMO-ADITYA-14P\\BIMO_ADITYA;Initial Catalog={0};User ID=sa;Password=bimbimbom";

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

                            // Membuat database jika belum ada
                            pr.CreateDatabase(dbName);

                            using (SqlConnection conn = new SqlConnection(string.Format(connectionString, dbName)))
                            {
                                conn.Open();
                                Console.Clear();

                                while (true)
                                {
                                    Console.WriteLine("\nMenu");
                                    Console.WriteLine("1. Melihat Seluruh Data");
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
                                            Console.WriteLine("Data Grooming\n");
                                            pr.ReadGrooming(conn);
                                            break;
                                        case '2':
                                            Console.Clear();
                                            pr.InsertGrooming(conn);
                                            break;
                                        case '3':
                                            Console.Clear();
                                            pr.DeleteGrooming(conn);
                                            break;
                                        case '4':
                                            Console.Clear();
                                            pr.SearchGrooming(conn);
                                            break;
                                        case '5':
                                            Console.Clear();
                                            pr.UpdateGrooming(conn);
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

        public void CreateDatabase(string dbName)
        {
            string masterConnectionString = "Data Source=BIMO-ADITYA-14P\\BIMO_ADITYA;Initial Catalog=master;User ID=sa;Password=bimbimbom";
            string createDbQuery = $"IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = '{dbName}') CREATE DATABASE {dbName}";

            using (SqlConnection conn = new SqlConnection(masterConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(createDbQuery, conn);
                cmd.ExecuteNonQuery();
            }
        }

        public void ReadGrooming(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT Id_grooming, Id_pegawai, Id_kucing, Nama_pegawai, Jenis_kucing, Jenis_bulu_kucing, Warna_kucing, Paket_yangdipilih FROM Grooming", con);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID Grooming: {reader.GetString(0)}, ID Pegawai: {reader.GetString(1)}, ID Kucing: {reader.GetString(2)}, Nama Pegawai: {reader.GetString(3)}, Jenis Kucing: {reader.GetString(4)}, Jenis Bulu Kucing: {reader.GetString(5)}, Warna Kucing: {reader.GetString(6)}, Paket: {reader.GetString(7)}");
                }
            }
        }

        public void InsertGrooming(SqlConnection con)
        {
            Console.WriteLine("Input data Grooming\n");
            Console.WriteLine("Masukkan ID Grooming (3 karakter): ");
            string idGrooming = Console.ReadLine();
            Console.WriteLine("Masukkan ID Pegawai (8 karakter): ");
            string idPegawai = Console.ReadLine();
            Console.WriteLine("Masukkan ID Kucing (4 karakter): ");
            string idKucing = Console.ReadLine();
            Console.WriteLine("Masukkan Nama Pegawai: ");
            string namaPegawai = Console.ReadLine();
            Console.WriteLine("Masukkan Jenis Kucing: ");
            string jenisKucing = Console.ReadLine();
            Console.WriteLine("Masukkan Jenis Bulu Kucing: ");
            string jenisBuluKucing = Console.ReadLine();
            Console.WriteLine("Masukkan Warna Kucing: ");
            string warnaKucing = Console.ReadLine();
            Console.WriteLine("Masukkan Paket yang dipilih (2 karakter): ");
            string paket = Console.ReadLine();

            string query = "INSERT INTO Grooming (Id_grooming, Id_pegawai, Id_kucing, Nama_pegawai, Jenis_kucing, Jenis_bulu_kucing, Warna_kucing, Paket_yangdipilih) VALUES (@idGrooming, @idPegawai, @idKucing, @namaPegawai, @jenisKucing, @jenisBuluKucing, @warnaKucing, @paket)";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@idGrooming", idGrooming);
            cmd.Parameters.AddWithValue("@idPegawai", idPegawai);
            cmd.Parameters.AddWithValue("@idKucing", idKucing);
            cmd.Parameters.AddWithValue("@namaPegawai", namaPegawai);
            cmd.Parameters.AddWithValue("@jenisKucing", jenisKucing);
            cmd.Parameters.AddWithValue("@jenisBuluKucing", jenisBuluKucing);
            cmd.Parameters.AddWithValue("@warnaKucing", warnaKucing);
            cmd.Parameters.AddWithValue("@paket", paket);

            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Grooming berhasil ditambahkan");
        }

        public void DeleteGrooming(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Grooming yang ingin dihapus: ");
            string idToDelete = Console.ReadLine();

            string query = "DELETE FROM Grooming WHERE Id_grooming = @id";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", idToDelete);
            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
                Console.WriteLine("Data Grooming berhasil dihapus");
            else
                Console.WriteLine("Data Grooming dengan ID tersebut tidak ditemukan");
        }

        public void SearchGrooming(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Grooming yang ingin dicari: ");
            string idToSearch = Console.ReadLine();

            string query = "SELECT Id_grooming, Id_pegawai, Id_kucing, Nama_pegawai, Jenis_kucing, Jenis_bulu_kucing, Warna_kucing, Paket_yangdipilih FROM Grooming WHERE Id_grooming = @id";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", idToSearch);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine($"ID Grooming: {reader.GetString(0)}, ID Pegawai: {reader.GetString(1)}, ID Kucing: {reader.GetString(2)}, Nama Pegawai: {reader.GetString(3)}, Jenis Kucing: {reader.GetString(4)}, Jenis Bulu Kucing: {reader.GetString(5)}, Warna Kucing: {reader.GetString(6)}, Paket: {reader.GetString(7)}");
                }
                else
                {
                    Console.WriteLine("Data Grooming tidak ditemukan");
                }
            }
        }

        public void UpdateGrooming(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Grooming yang ingin diperbarui: ");
            string idToUpdate = Console.ReadLine();

            string selectQuery = "SELECT Id_pegawai, Id_kucing, Nama_pegawai, Jenis_kucing, Jenis_bulu_kucing, Warna_kucing, Paket_yangdipilih FROM Grooming WHERE Id_grooming = @id";
            SqlCommand selectCmd = new SqlCommand(selectQuery, con);
            selectCmd.Parameters.AddWithValue("@id", idToUpdate);

            using (SqlDataReader reader = selectCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string currentIdPegawai = reader.GetString(0);
                    string currentIdKucing = reader.GetString(1);
                    string currentNamaPegawai = reader.GetString(2);
                    string currentJenisKucing = reader.GetString(3);
                    string currentJenisBuluKucing = reader.GetString(4);
                    string currentWarnaKucing = reader.GetString(5);
                    string currentPaket = reader.GetString(6);

                    Console.WriteLine($"Data saat ini - ID Pegawai: {currentIdPegawai}, ID Kucing: {currentIdKucing}, Nama Pegawai: {currentNamaPegawai}, Jenis Kucing: {currentJenisKucing}, Jenis Bulu Kucing: {currentJenisBuluKucing}, Warna Kucing: {currentWarnaKucing}, Paket: {currentPaket}");

                    Console.WriteLine("\nMasukkan informasi baru:");

                    Console.WriteLine("ID Pegawai (kosongkan jika tidak ingin mengubah): ");
                    string newIdPegawai = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newIdPegawai))
                        newIdPegawai = currentIdPegawai;

                    Console.WriteLine("ID Kucing (kosongkan jika tidak ingin mengubah): ");
                    string newIdKucing = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newIdKucing))
                        newIdKucing = currentIdKucing;

                    Console.WriteLine("Nama Pegawai (kosongkan jika tidak ingin mengubah): ");
                    string newNamaPegawai = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newNamaPegawai))
                        newNamaPegawai = currentNamaPegawai;

                    Console.WriteLine("Jenis Kucing (kosongkan jika tidak ingin mengubah): ");
                    string newJenisKucing = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newJenisKucing))
                        newJenisKucing = currentJenisKucing;

                    Console.WriteLine("Jenis Bulu Kucing (kosongkan jika tidak ingin mengubah): ");
                    string newJenisBuluKucing = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newJenisBuluKucing))
                        newJenisBuluKucing = currentJenisBuluKucing;

                    Console.WriteLine("Warna Kucing (kosongkan jika tidak ingin mengubah): ");
                    string newWarnaKucing = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newWarnaKucing))
                        newWarnaKucing = currentWarnaKucing;

                    Console.WriteLine("Paket (kosongkan jika tidak ingin mengubah): ");
                    string newPaket = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newPaket))
                        newPaket = currentPaket;

                    string updateQuery = "UPDATE Grooming SET Id_pegawai = @idPegawai, Id_kucing = @idKucing, Nama_pegawai = @namaPegawai, Jenis_kucing = @jenisKucing, Jenis_bulu_kucing = @jenisBuluKucing, Warna_kucing = @warnaKucing, Paket_yangdipilih = @paket WHERE Id_grooming = @id";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                    updateCmd.Parameters.AddWithValue("@id", idToUpdate);
                    updateCmd.Parameters.AddWithValue("@idPegawai", newIdPegawai);
                    updateCmd.Parameters.AddWithValue("@idKucing", newIdKucing);
                    updateCmd.Parameters.AddWithValue("@namaPegawai", newNamaPegawai);
                    updateCmd.Parameters.AddWithValue("@jenisKucing", newJenisKucing);
                    updateCmd.Parameters.AddWithValue("@jenisBuluKucing", newJenisBuluKucing);
                    updateCmd.Parameters.AddWithValue("@warnaKucing", newWarnaKucing);
                    updateCmd.Parameters.AddWithValue("@paket", newPaket);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        Console.WriteLine("Data Grooming berhasil diperbarui");
                    else
                        Console.WriteLine("Data Grooming gagal diperbarui");
                }
                else
                {
                    Console.WriteLine("Data Grooming tidak ditemukan");
                }
            }
        }
    }
}