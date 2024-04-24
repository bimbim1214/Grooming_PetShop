using System;
using System.Data.SqlClient;

namespace GroomingPetShop
{
    internal class Kepemilikan
    {
        public void Main()
        {
            Kepemilikan pr = new Kepemilikan();
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
                                    Console.WriteLine("1. Melihat Seluruh Data Kepemilikan");
                                    Console.WriteLine("2. Tambah Data Kepemilikan");
                                    Console.WriteLine("3. Hapus Data Kepemilikan");
                                    Console.WriteLine("4. Cari Data Kepemilikan");
                                    Console.WriteLine("5. Keluar");
                                    Console.WriteLine("\nEnter your choice (1-5): ");

                                    char ch = Char.ToUpper(Console.ReadKey().KeyChar);
                                    Console.WriteLine();

                                    switch (ch)
                                    {
                                        case '1':
                                            Console.Clear();
                                            Console.WriteLine("Data Kepemilikan\n");
                                            pr.ReadOwnership(conn);
                                            break;
                                        case '2':
                                            Console.Clear();
                                            pr.InsertOwnership(conn);
                                            break;
                                        case '3':
                                            Console.Clear();
                                            pr.DeleteOwnership(conn);
                                            break;
                                        case '4':
                                            Console.Clear();
                                            pr.SearchOwnership(conn);
                                            break;
                                        case '5':
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

        public void ReadOwnership(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT Id_pelanggan, Id_kucing, Nama_pelanggan, Nama_kucing, Jenis_kucing FROM Kepemilikan", con);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID Pelanggan: {reader.GetString(0)}, ID Kucing: {reader.GetString(1)}, Nama Pelanggan: {reader.GetString(2)}, Nama Kucing: {reader.GetString(3)}, Jenis Kucing: {reader.GetString(4)}");
                }
            }
        }

        public void InsertOwnership(SqlConnection con)
        {
            Console.WriteLine("Input data Kepemilikan\n");
            Console.WriteLine("Masukkan ID Pelanggan (8 karakter): ");
            string idPelanggan = Console.ReadLine();
            Console.WriteLine("Masukkan ID Kucing (4 karakter): ");
            string idKucing = Console.ReadLine();
            Console.WriteLine("Masukkan Nama Pelanggan: ");
            string namaPelanggan = Console.ReadLine();
            Console.WriteLine("Masukkan Nama Kucing: ");
            string namaKucing = Console.ReadLine();
            Console.WriteLine("Masukkan Jenis Kucing: ");
            string jenisKucing = Console.ReadLine();

            string query = "INSERT INTO Kepemilikan (Id_pelanggan, Id_kucing, Nama_pelanggan, Nama_kucing, Jenis_kucing) VALUES (@idPelanggan, @idKucing, @namaPelanggan, @namaKucing, @jenisKucing)";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@idPelanggan", idPelanggan);
            cmd.Parameters.AddWithValue("@idKucing", idKucing);
            cmd.Parameters.AddWithValue("@namaPelanggan", namaPelanggan);
            cmd.Parameters.AddWithValue("@namaKucing", namaKucing);
            cmd.Parameters.AddWithValue("@jenisKucing", jenisKucing);

            cmd.ExecuteNonQuery();
            Console.WriteLine("Data Kepemilikan berhasil ditambahkan");
        }

        public void DeleteOwnership(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Pelanggan yang ingin dihapus: ");
            string idPelangganToDelete = Console.ReadLine();
            Console.WriteLine("Masukkan ID Kucing yang ingin dihapus: ");
            string idKucingToDelete = Console.ReadLine();

            string query = "DELETE FROM Kepemilikan WHERE Id_pelanggan = @idPelanggan AND Id_kucing = @idKucing";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@idPelanggan", idPelangganToDelete);
            cmd.Parameters.AddWithValue("@idKucing", idKucingToDelete);

            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
                Console.WriteLine("Data Kepemilikan berhasil dihapus");
            else
                Console.WriteLine("Data Kepemilikan dengan ID tersebut tidak ditemukan");
        }

        public void SearchOwnership(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Pelanggan yang ingin dicari: ");
            string idPelangganToSearch = Console.ReadLine();
            Console.WriteLine("Masukkan ID Kucing yang ingin dicari: ");
            string idKucingToSearch = Console.ReadLine();

            string query = "SELECT Id_pelanggan, Id_kucing, Nama_pelanggan, Nama_kucing, Jenis_kucing FROM Kepemilikan WHERE Id_pelanggan = @idPelanggan AND Id_kucing = @idKucing";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@idPelanggan", idPelangganToSearch);
            cmd.Parameters.AddWithValue("@idKucing", idKucingToSearch);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine($"ID Pelanggan: {reader.GetString(0)}, ID Kucing: {reader.GetString(1)}, Nama Pelanggan: {reader.GetString(2)}, Nama Kucing: {reader.GetString(3)}, Jenis Kucing: {reader.GetString(4)}");
                }
                else
                {
                    Console.WriteLine("Data Kepemilikan tidak ditemukan");
                }
            }
        }

        public void UpdateOwnership(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Pelanggan yang ingin diperbarui: ");
            string idPelangganToUpdate = Console.ReadLine();
            Console.WriteLine("Masukkan ID Kucing yang ingin diperbarui: ");
            string idKucingToUpdate = Console.ReadLine();

            string selectQuery = "SELECT Nama_pelanggan, Nama_kucing, Jenis_kucing FROM Kepemilikan WHERE Id_pelanggan = @idPelanggan AND Id_kucing = @idKucing";
            SqlCommand selectCmd = new SqlCommand(selectQuery, con);
            selectCmd.Parameters.AddWithValue("@idPelanggan", idPelangganToUpdate);
            selectCmd.Parameters.AddWithValue("@idKucing", idKucingToUpdate);

            using (SqlDataReader reader = selectCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string currentNamaPelanggan = reader.GetString(0);
                    string currentNamaKucing = reader.GetString(1);
                    string currentJenisKucing = reader.GetString(2);

                    Console.WriteLine($"Data saat ini - Nama Pelanggan: {currentNamaPelanggan}, Nama Kucing: {currentNamaKucing}, Jenis Kucing: {currentJenisKucing}");

                    Console.WriteLine("\nMasukkan informasi baru:");

                    Console.WriteLine("Nama Pelanggan (kosongkan jika tidak ingin mengubah): ");
                    string newNamaPelanggan = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newNamaPelanggan))
                        newNamaPelanggan = currentNamaPelanggan;

                    Console.WriteLine("Nama Kucing (kosongkan jika tidak ingin mengubah): ");
                    string newNamaKucing = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newNamaKucing))
                        newNamaKucing = currentNamaKucing;

                    Console.WriteLine("Jenis Kucing (kosongkan jika tidak ingin mengubah): ");
                    string newJenisKucing = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newJenisKucing))
                        newJenisKucing = currentJenisKucing;

                    string updateQuery = "UPDATE Kepemilikan SET Nama_pelanggan = @namaPelanggan, Nama_kucing = @namaKucing, Jenis_kucing = @jenisKucing WHERE Id_pelanggan = @idPelanggan AND Id_kucing = @idKucing";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                    updateCmd.Parameters.AddWithValue("@idPelanggan", idPelangganToUpdate);
                    updateCmd.Parameters.AddWithValue("@idKucing", idKucingToUpdate);
                    updateCmd.Parameters.AddWithValue("@namaPelanggan", newNamaPelanggan);
                    updateCmd.Parameters.AddWithValue("@namaKucing", newNamaKucing);
                    updateCmd.Parameters.AddWithValue("@jenisKucing", newJenisKucing);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        Console.WriteLine("Data Kepemilikan berhasil diperbarui");
                    else
                        Console.WriteLine("Data Kepemilikan gagal diperbarui");
                }
                else
                {
                    Console.WriteLine("Data Kepemilikan tidak ditemukan");
                }
            }
        }
    }
}

