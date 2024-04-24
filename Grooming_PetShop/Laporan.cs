using System;
using System.Data.SqlClient;

namespace GroomingPetShop
{
    internal class Laporan
    {
        public void Main()
        {
            Laporan pr = new Laporan();
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
                                    Console.WriteLine("2. Tambah Data Laporan");
                                    Console.WriteLine("3. Hapus Data Laporan");
                                    Console.WriteLine("4. Cari Data Laporan");
                                    Console.WriteLine("5. Perbarui Data Laporan");
                                    Console.WriteLine("6. Keluar");
                                    Console.WriteLine("\nEnter your choice (1-6): ");

                                    char ch = Char.ToUpper(Console.ReadKey().KeyChar);
                                    Console.WriteLine();

                                    switch (ch)
                                    {
                                        case '1':
                                            Console.Clear();
                                            Console.WriteLine("Data Laporan\n");
                                            pr.ReadLaporan(conn);
                                            break;
                                        case '2':
                                            Console.Clear();
                                            pr.InsertLaporan(conn);
                                            break;
                                        case '3':
                                            Console.Clear();
                                            pr.DeleteLaporan(conn);
                                            break;
                                        case '4':
                                            Console.Clear();
                                            pr.SearchLaporan(conn);
                                            break;
                                        case '5':
                                            Console.Clear();
                                            pr.UpdateLaporan(conn);
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

        public void ReadLaporan(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT Id_laporan, Id_pegawai, Id_pelanggan, Alamat, Nama_kucing, Jumlah_kucing, Harga_paket, Tanggal_transaksi FROM Laporan", con);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID Laporan: {reader.GetString(0)}, ID Pegawai: {reader.GetString(1)}, ID Pelanggan: {reader.GetString(2)}, Alamat: {reader.GetString(3)}, Nama Kucing: {reader.GetString(4)}, Jumlah Kucing: {reader.GetString(5)}, Harga Paket: {reader.GetString(6)}, Tanggal Transaksi: {reader.GetString(7)}");
                }
            }
        }

        public void InsertLaporan(SqlConnection con)
        {
            Console.WriteLine("Input data Laporan\n");
            Console.WriteLine("Masukkan ID Laporan (4 karakter): ");
            string idLaporan = Console.ReadLine();
            Console.WriteLine("Masukkan ID Pegawai (8 karakter): ");
            string idPegawai = Console.ReadLine();
            Console.WriteLine("Masukkan ID Pelanggan (8 karakter): ");
            string idPelanggan = Console.ReadLine();
            Console.WriteLine("Masukkan Alamat: ");
            string alamat = Console.ReadLine();
            Console.WriteLine("Masukkan Nama Kucing: ");
            string namaKucing = Console.ReadLine();
            Console.WriteLine("Masukkan Jumlah Kucing: ");
            string jumlahKucing = Console.ReadLine();
            Console.WriteLine("Masukkan Harga Paket: ");
            string hargaPaket = Console.ReadLine();
            Console.WriteLine("Masukkan Tanggal Transaksi: ");
            string tanggalTransaksi = Console.ReadLine();

            string query = "INSERT INTO Laporan (Id_laporan, Id_pegawai, Id_pelanggan, Alamat, Nama_kucing, Jumlah_kucing, Harga_paket, Tanggal_transaksi) VALUES (@idLaporan, @idPegawai, @idPelanggan, @alamat, @namaKucing, @jumlahKucing, @hargaPaket, @tanggalTransaksi)";
            SqlCommand cmd = new SqlCommand(query, con);

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

        public void DeleteLaporan(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Laporan yang ingin dihapus: ");
            string idToDelete = Console.ReadLine();

            string query = "DELETE FROM Laporan WHERE Id_laporan = @id";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", idToDelete);
            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
                Console.WriteLine("Data Laporan berhasil dihapus");
            else
                Console.WriteLine("Data Laporan dengan ID tersebut tidak ditemukan");
        }

        public void SearchLaporan(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Laporan yang ingin dicari: ");
            string idToSearch = Console.ReadLine();

            string query = "SELECT Id_laporan, Id_pegawai, Id_pelanggan, Alamat, Nama_kucing, Jumlah_kucing, Harga_paket, Tanggal_transaksi FROM Laporan WHERE Id_laporan = @id";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", idToSearch);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine($"ID Laporan: {reader.GetString(0)}, ID Pegawai: {reader.GetString(1)}, ID Pelanggan: {reader.GetString(2)}, Alamat: {reader.GetString(3)}, Nama Kucing: {reader.GetString(4)}, Jumlah Kucing: {reader.GetString(5)}, Harga Paket: {reader.GetString(6)}, Tanggal Transaksi: {reader.GetString(7)}");
                }
                else
                {
                    Console.WriteLine("Data Laporan tidak ditemukan");
                }
            }
        }

        public void UpdateLaporan(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Laporan yang ingin diperbarui: ");
            string idToUpdate = Console.ReadLine();

            string selectQuery = "SELECT Id_pegawai, Id_pelanggan, Alamat, Nama_kucing, Jumlah_kucing, Harga_paket, Tanggal_transaksi FROM Laporan WHERE Id_laporan = @id";
            SqlCommand selectCmd = new SqlCommand(selectQuery, con);
            selectCmd.Parameters.AddWithValue("@id", idToUpdate);

            using (SqlDataReader reader = selectCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string currentIdPegawai = reader.GetString(0);
                    string currentIdPelanggan = reader.GetString(1);
                    string currentAlamat = reader.GetString(2);
                    string currentNamaKucing = reader.GetString(3);
                    string currentJumlahKucing = reader.GetString(4);
                    string currentHargaPaket = reader.GetString(5);
                    string currentTanggalTransaksi = reader.GetString(6);

                    Console.WriteLine($"Data saat ini - ID Pegawai: {currentIdPegawai}, ID Pelanggan: {currentIdPelanggan}, Alamat: {currentAlamat}, Nama Kucing: {currentNamaKucing}, Jumlah Kucing: {currentJumlahKucing}, Harga Paket: {currentHargaPaket}, Tanggal Transaksi: {currentTanggalTransaksi}");

                    Console.WriteLine("\nMasukkan informasi baru:");

                    Console.WriteLine("ID Pegawai (kosongkan jika tidak ingin mengubah): ");
                    string newIdPegawai = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newIdPegawai))
                        newIdPegawai = currentIdPegawai;

                    Console.WriteLine("ID Pelanggan (kosongkan jika tidak ingin mengubah): ");
                    string newIdPelanggan = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newIdPelanggan))
                        newIdPelanggan = currentIdPelanggan;

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

                    Console.WriteLine("Tanggal Transaksi (kosongkan jika tidak ingin mengubah): ");
                    string newTanggalTransaksi = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newTanggalTransaksi))
                        newTanggalTransaksi = currentTanggalTransaksi;

                    string updateQuery = "UPDATE Laporan SET Id_pegawai = @idPegawai, Id_pelanggan = @idPelanggan, Alamat = @alamat, Nama_kucing = @namaKucing, Jumlah_kucing = @jumlahKucing, Harga_paket = @hargaPaket, Tanggal_transaksi = @tanggalTransaksi WHERE Id_laporan = @id";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                    updateCmd.Parameters.AddWithValue("@id", idToUpdate);
                    updateCmd.Parameters.AddWithValue("@idPegawai", newIdPegawai);
                    updateCmd.Parameters.AddWithValue("@idPelanggan", newIdPelanggan);
                    updateCmd.Parameters.AddWithValue("@alamat", newAlamat);
                    updateCmd.Parameters.AddWithValue("@namaKucing", newNamaKucing);
                    updateCmd.Parameters.AddWithValue("@jumlahKucing", newJumlahKucing);
                    updateCmd.Parameters.AddWithValue("@hargaPaket", newHargaPaket);
                    updateCmd.Parameters.AddWithValue("@tanggalTransaksi", newTanggalTransaksi);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        Console.WriteLine("Data Laporan berhasil diperbarui");
                    else
                        Console.WriteLine("Data Laporan gagal diperbarui");
                }
                else
                {
                    Console.WriteLine("Data Laporan tidak ditemukan");
                }
            }
        }
    }
}
