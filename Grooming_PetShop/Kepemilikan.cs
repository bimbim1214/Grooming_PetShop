using System;
using System.Data.SqlClient;

namespace GroomingPetShop
{
    internal class Kepemilikan
    {
        private string connectionString = "Data Source=BIMO-ADITYA-14P\\BIMO_ADITYA;Initial Catalog={0};User ID=sa;Password=bimbimbom";

        public void Main()
        {
            Kepemilikan kepemilikanHandler = new Kepemilikan();

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
                                    Console.WriteLine("1. Melihat Seluruh Data Kepemilikan");
                                    Console.WriteLine("2. Tambah Data Kepemilikan");
                                    Console.WriteLine("3. Hapus Data Kepemilikan");
                                    Console.WriteLine("4. Cari Data Kepemilikan");
                                    Console.WriteLine("5. Perbarui Data Kepemilikan");
                                    Console.WriteLine("6. Keluar");
                                    Console.WriteLine("\nEnter your choice (1-6): ");

                                    char ch = Char.ToUpper(Console.ReadKey().KeyChar);
                                    Console.WriteLine();

                                    switch (ch)
                                    {
                                        case '1':
                                            Console.Clear();
                                            Console.WriteLine("Data Kepemilikan\n");
                                            kepemilikanHandler.ReadOwnership(conn);
                                            break;
                                        case '2':
                                            Console.Clear();
                                            kepemilikanHandler.InsertOwnership(conn);
                                            break;
                                        case '3':
                                            Console.Clear();
                                            kepemilikanHandler.DeleteOwnership(conn);
                                            break;
                                        case '4':
                                            Console.Clear();
                                            kepemilikanHandler.SearchOwnership(conn);
                                            break;
                                        case '5':
                                            Console.Clear();
                                            kepemilikanHandler.UpdateOwnership(conn);
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

        public void ReadOwnership(SqlConnection con)
        {
            string query = "SELECT k.Id_pelanggan, p.Nama_pelanggan, k.Id_kucing, c.Nama_kucing FROM Kepemilikan k " +
                           "INNER JOIN Pelanggan p ON k.Id_pelanggan = p.Id_pelanggan " +
                           "INNER JOIN Kucing c ON k.Id_kucing = c.Id_kucing";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID Pelanggan: {reader.GetString(0)}, Nama Pelanggan: {reader.GetString(1)}, " +
                                          $"ID Kucing: {reader.GetString(2)}, Nama Kucing: {reader.GetString(3)}");
                    }
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

            // Lakukan validasi untuk memastikan bahwa idPelanggan dan idKucing valid
            if (!IsValidPelangganId(idPelanggan, con))
            {
                Console.WriteLine("ID Pelanggan tidak valid.");
                return;
            }

            if (!IsValidKucingId(idKucing, con))
            {
                Console.WriteLine("ID Kucing tidak valid.");
                return;
            }

            string insertQuery = "INSERT INTO Kepemilikan (Id_pelanggan, Id_kucing) VALUES (@idPelanggan, @idKucing)";

            using (SqlCommand cmd = new SqlCommand(insertQuery, con))
            {
                cmd.Parameters.AddWithValue("@idPelanggan", idPelanggan);
                cmd.Parameters.AddWithValue("@idKucing", idKucing);

                cmd.ExecuteNonQuery();
                Console.WriteLine("Data Kepemilikan berhasil ditambahkan");

                // Menampilkan informasi nama pemilik dan nama kucing setelah memasukkan data ke dalam Kepemilikan
                DisplayPelangganKucingInfo(idPelanggan, idKucing, con);
            }
        }

        private void DisplayPelangganKucingInfo(string idPelanggan, string idKucing, SqlConnection con)
        {
            string query = "SELECT p.Nama_pelanggan, c.Nama_kucing FROM Pelanggan p " +
                           "INNER JOIN Kucing c ON p.Id_pelanggan = @idPelanggan AND c.Id_kucing = @idKucing";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@idPelanggan", idPelanggan);
                cmd.Parameters.AddWithValue("@idKucing", idKucing);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string namaPelanggan = reader.GetString(0);
                        string namaKucing = reader.GetString(1);
                        Console.WriteLine($"Nama Pemilik: {namaPelanggan}, Nama Kucing: {namaKucing}");
                    }
                }
            }
        }

        // Method IsValidPelangganId dan IsValidKucingId di sini sama seperti sebelumnya
        private bool IsValidPelangganId(string idPelanggan, SqlConnection con)
        {
            string query = "SELECT COUNT(*) FROM Pelanggan WHERE Id_pelanggan = @idPelanggan";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@idPelanggan", idPelanggan);

            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }

        private bool IsValidKucingId(string idKucing, SqlConnection con)
        {
            string query = "SELECT COUNT(*) FROM Kucing WHERE Id_kucing = @idKucing";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@idKucing", idKucing);

            int count = (int)cmd.ExecuteScalar();
            return count > 0;
        }


        public void DeleteOwnership(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Pelanggan yang ingin dihapus dari Kepemilikan: ");
            string idPelanggan = Console.ReadLine();

            Console.WriteLine("Masukkan ID Kucing yang ingin dihapus dari Kepemilikan: ");
            string idKucing = Console.ReadLine();

            string query = "DELETE FROM Kepemilikan WHERE Id_pelanggan = @idPelanggan AND Id_kucing = @idKucing";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@idPelanggan", idPelanggan);
            cmd.Parameters.AddWithValue("@idKucing", idKucing);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected > 0)
                Console.WriteLine("Data Kepemilikan berhasil dihapus");
            else
                Console.WriteLine("Data Kepemilikan dengan ID tersebut tidak ditemukan");
        }

        public void SearchOwnership(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Pelanggan yang ingin dicari di Kepemilikan: ");
            string idPelanggan = Console.ReadLine();

            Console.WriteLine("Masukkan ID Kucing yang ingin dicari di Kepemilikan: ");
            string idKucing = Console.ReadLine();

            string query = "SELECT Id_pelanggan, Id_kucing FROM Kepemilikan WHERE Id_pelanggan = @idPelanggan AND Id_kucing = @idKucing";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@idPelanggan", idPelanggan);
            cmd.Parameters.AddWithValue("@idKucing", idKucing);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine($"ID Pelanggan: {reader.GetString(0)}, ID Kucing: {reader.GetString(1)}");
                }
                else
                {
                    Console.WriteLine("Data Kepemilikan tidak ditemukan");
                }
            }
        }

        public void UpdateOwnership(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Pelanggan yang ingin diperbarui di Kepemilikan: ");
            string idPelanggan = Console.ReadLine();

            Console.WriteLine("Masukkan ID Kucing yang ingin diperbarui di Kepemilikan: ");
            string idKucing = Console.ReadLine();

            // Lakukan validasi untuk memastikan bahwa idPelanggan dan idKucing valid
            if (!IsValidPelangganId(idPelanggan, con))
            {
                Console.WriteLine("ID Pelanggan tidak valid.");
                return;
            }

            if (!IsValidKucingId(idKucing, con))
            {
                Console.WriteLine("ID Kucing tidak valid.");
                return;
            }

            string selectQuery = "SELECT Id_pelanggan, Id_kucing FROM Kepemilikan WHERE Id_pelanggan = @idPelanggan AND Id_kucing = @idKucing";
            SqlCommand selectCmd = new SqlCommand(selectQuery, con);
            selectCmd.Parameters.AddWithValue("@idPelanggan", idPelanggan);
            selectCmd.Parameters.AddWithValue("@idKucing", idKucing);

            using (SqlDataReader reader = selectCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine($"Data Kepemilikan saat ini - ID Pelanggan: {reader.GetString(0)}, ID Kucing: {reader.GetString(1)}");

                    Console.WriteLine("\nMasukkan informasi baru:");

                    Console.WriteLine("ID Pelanggan (kosongkan jika tidak ingin mengubah): ");
                    string newIdPelanggan = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newIdPelanggan))
                        newIdPelanggan = idPelanggan;

                    Console.WriteLine("ID Kucing (kosongkan jika tidak ingin mengubah): ");
                    string newIdKucing = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newIdKucing))
                        newIdKucing = idKucing;

                    string updateQuery = "UPDATE Kepemilikan SET Id_pelanggan = @newIdPelanggan, Id_kucing = @newIdKucing WHERE Id_pelanggan = @idPelanggan AND Id_kucing = @idKucing";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                    updateCmd.Parameters.AddWithValue("@idPelanggan", idPelanggan);
                    updateCmd.Parameters.AddWithValue("@idKucing", idKucing);
                    updateCmd.Parameters.AddWithValue("@newIdPelanggan", newIdPelanggan);
                    updateCmd.Parameters.AddWithValue("@newIdKucing", newIdKucing);

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
