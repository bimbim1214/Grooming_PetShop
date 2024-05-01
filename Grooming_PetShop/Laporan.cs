using System;
using System.Data.SqlClient;

namespace GroomingPetShop
{
    internal class Program
    {
        private static string connectionString = "Data Source=BIMO-ADITYA-14P\\BIMO_ADITYA;Initial Catalog={0};User ID=sa;Password=bimbimbom";

        public static void Main()
        {
            Program program = new Program();

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
                                    Console.WriteLine("1. Melihat Seluruh Data");
                                    Console.WriteLine("2. Tambah Data");
                                    Console.WriteLine("3. Hapus Data");
                                    Console.WriteLine("4. Cari Data");
                                    Console.WriteLine("5. Perbarui Data");
                                    Console.WriteLine("6. Keluar");
                                    Console.WriteLine("\nEnter your choice (1-6): ");

                                    char ch = Char.ToUpper(Console.ReadKey().KeyChar);
                                    Console.WriteLine();

                                    switch (ch)
                                    {
                                        case '1':
                                            Console.Clear();
                                            Console.WriteLine("Data Laporan\n");
                                            program.ReadReports(conn);
                                            break;
                                        case '2':
                                            Console.Clear();
                                            program.InsertReport(conn);
                                            break;
                                        case '3':
                                            Console.Clear();
                                            program.DeleteReport(conn);
                                            break;
                                        case '4':
                                            Console.Clear();
                                            program.SearchReport(conn);
                                            break;
                                        case '5':
                                            Console.Clear();
                                            program.UpdateReport(conn);
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

        public void ReadReports(SqlConnection con)
        {
            string query = "SELECT Id_laporan, Id_pegawai, Id_pelanggan, Alamat, Nama_kucing, Jumlah_kucing, Harga_paket, Tanggal_transaksi FROM Laporan";

            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"ID Laporan: {reader.GetString(0)}, ID Pegawai: {reader.GetString(1)}, " +
                                          $"ID Pelanggan: {reader.GetString(2)}, Alamat: {reader.GetString(3)}, " +
                                          $"Nama Kucing: {reader.GetString(4)}, Jumlah Kucing: {reader.GetString(5)}, " +
                                          $"Harga Paket: {reader.GetString(6)}, Tanggal Transaksi: {reader.GetString(7)}");
                    }
                }
            }
        }

        public void InsertReport(SqlConnection con)
        {
            Console.WriteLine("Input data Laporan\n");
            Console.WriteLine("Masukkan ID Laporan (4 karakter): ");
            string idLaporan = Console.ReadLine();
            Console.WriteLine("Masukkan ID Pegawai (8 karakter): ");
            string idPegawai = Console.ReadLine();
            Console.WriteLine("Masukkan ID Pelanggan (8 karakter): ");
            string idPelanggan = Console.ReadLine();
            Console.WriteLine("Masukkan Alamat Pelanggan: ");
            string alamat = Console.ReadLine();
            Console.WriteLine("Masukkan Nama Kucing: ");
            string namaKucing = Console.ReadLine();
            Console.WriteLine("Masukkan Jumlah Kucing: ");
            string jumlahKucing = Console.ReadLine();
            Console.WriteLine("Masukkan Harga Paket: ");
            string hargaPaket = Console.ReadLine();
            Console.WriteLine("Masukkan Tanggal Transaksi (yyyy-MM-dd): ");
            string tanggalTransaksi = Console.ReadLine();

            string insertQuery = "INSERT INTO Laporan (Id_laporan, Id_pegawai, Id_pelanggan, Alamat, Nama_kucing, Jumlah_kucing, Harga_paket, Tanggal_transaksi) " +
                                 "VALUES (@idLaporan, @idPegawai, @idPelanggan, @alamat, @namaKucing, @jumlahKucing, @hargaPaket, @tanggalTransaksi)";

            using (SqlCommand cmd = new SqlCommand(insertQuery, con))
            {
                cmd.Parameters.AddWithValue("@idLaporan", idLaporan);
                cmd.Parameters.AddWithValue("@idPegawai", idPegawai);
                cmd.Parameters.AddWithValue("@idPelanggan", idPelanggan);
                cmd.Parameters.AddWithValue("@alamat", alamat);
                cmd.Parameters.AddWithValue("@namaKucing", namaKucing);
                cmd.Parameters.AddWithValue("@jumlahKucing", jumlahKucing);
                cmd.Parameters.AddWithValue("@hargaPaket", hargaPaket);
                cmd.Parameters.AddWithValue("@tanggalTransaksi", tanggalTransaksi);

                cmd.ExecuteNonQuery();
                Console.WriteLine("Data Laporan berhasil ditambahkan");
            }
        }

        public void DeleteReport(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Pelanggan yang ingin dihapus dari Laporan: ");
            string idPelangganToDelete = Console.ReadLine();

            string query = "DELETE FROM Laporan WHERE Id_pelanggan = @idPelanggan";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@idPelanggan", idPelangganToDelete);
            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
                Console.WriteLine("Data Laporan berhasil dihapus");
            else
                Console.WriteLine("Data dengan ID Pelanggan tersebut tidak ditemukan");
        }

        public void SearchReport(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Pelanggan yang ingin dicari di Laporan: ");
            string idPelangganToSearch = Console.ReadLine();

            string query = "SELECT * FROM Laporan WHERE Id_pelanggan = @idPelanggan";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@idPelanggan", idPelangganToSearch);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine($"ID Laporan: {reader.GetString(0)}, ID Pegawai: {reader.GetString(1)}, " +
                                      $"ID Pelanggan: {reader.GetString(2)}, Alamat: {reader.GetString(3)}, " +
                                      $"Nama Kucing: {reader.GetString(4)}, Jumlah Kucing: {reader.GetString(5)}, " +
                                      $"Harga Paket: {reader.GetString(6)}, Tanggal Transaksi: {reader.GetString(7)}");
                }
                else
                {
                    Console.WriteLine("Data tidak ditemukan");
                }
            }
        }

        public void UpdateReport(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Pelanggan yang ingin diperbarui di Laporan: ");
            string idPelangganToUpdate = Console.ReadLine();

            string selectQuery = "SELECT * FROM Laporan WHERE Id_pelanggan = @idPelanggan";
            SqlCommand selectCmd = new SqlCommand(selectQuery, con);
            selectCmd.Parameters.AddWithValue("@idPelanggan", idPelangganToUpdate);

            using (SqlDataReader reader = selectCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string currentAlamat = reader.GetString(3);
                    string currentNamaKucing = reader.GetString(4);
                    string currentJumlahKucing = reader.GetString(5);
                    string currentHargaPaket = reader.GetString(6);
                    string currentTanggalTransaksi = reader.GetString(7);

                    Console.WriteLine($"Data saat ini - Alamat: {currentAlamat}, Nama Kucing: {currentNamaKucing}, " +
                                      $"Jumlah Kucing: {currentJumlahKucing}, Harga Paket: {currentHargaPaket}, " +
                                      $"Tanggal Transaksi: {currentTanggalTransaksi}");

                    reader.Close();

                    Console.WriteLine("\nMasukkan informasi baru:");

                    Console.WriteLine("Alamat (kosongkan jika tidak ingin mengubah): ");
                    string newAlamat = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newAlamat))
                        newAlamat = currentAlamat;

                    Console.WriteLine("Nama Kucing (kosongkan jika tidak ingin mengubah): ");
                    string newNamaKucing = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newNamaKucing))
                        newNamaKucing = currentNamaKucing;

                    Console.WriteLine("Jumlah Kucing (kosongkan jika tidak ingin mengubah): ");
                    string newJumlahKucing = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newJumlahKucing))
                        newJumlahKucing = currentJumlahKucing;

                    Console.WriteLine("Harga Paket (kosongkan jika tidak ingin mengubah): ");
                    string newHargaPaket = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newHargaPaket))
                        newHargaPaket = currentHargaPaket;

                    Console.WriteLine("Tanggal Transaksi (yyyy-MM-dd, kosongkan jika tidak ingin mengubah): ");
                    string newTanggalTransaksi = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newTanggalTransaksi))
                        newTanggalTransaksi = currentTanggalTransaksi;

                    string updateQuery = "UPDATE Laporan SET Alamat = @alamat, Nama_kucing = @namaKucing, " +
                                         "Jumlah_kucing = @jumlahKucing, Harga_paket = @hargaPaket, " +
                                         "Tanggal_transaksi = @tanggalTransaksi WHERE Id_pelanggan = @idPelanggan";

                    SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                    updateCmd.Parameters.AddWithValue("@alamat", newAlamat);
                    updateCmd.Parameters.AddWithValue("@namaKucing", newNamaKucing);
                    updateCmd.Parameters.AddWithValue("@jumlahKucing", newJumlahKucing);
                    updateCmd.Parameters.AddWithValue("@hargaPaket", newHargaPaket);
                    updateCmd.Parameters.AddWithValue("@tanggalTransaksi", newTanggalTransaksi);
                    updateCmd.Parameters.AddWithValue("@idPelanggan", idPelangganToUpdate);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        Console.WriteLine("Data Laporan berhasil diperbarui");
                    else
                        Console.WriteLine("Data Laporan gagal diperbarui");
                }
                else
                {
                    Console.WriteLine("Data tidak ditemukan");
                }
            }
        }
    }
}
