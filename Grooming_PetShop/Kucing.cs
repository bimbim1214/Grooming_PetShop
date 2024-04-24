using System;
using System.Data.SqlClient;

namespace GroomingPetShop

{
    internal class Kucing
    {
        public void Main()
        {
            Kucing pr = new Kucing();
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
                                            Console.WriteLine("Data Kucing\n");
                                            pr.ReadCats(conn);
                                            break;
                                        case '2':
                                            Console.Clear();
                                            pr.InsertCat(conn);
                                            break;
                                        case '3':
                                            Console.Clear();
                                            pr.DeleteCat(conn);
                                            break;
                                        case '4':
                                            Console.Clear();
                                            pr.SearchCat(conn);
                                            break;
                                        case '5':
                                            Console.Clear();
                                            pr.UpdateCat(conn);
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
        public void ReadCats(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT id_kucing, Nama_kucing, Jenis_kucing, Warna_kucing, Jenis_bulukucing FROM Kucing", con);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader.GetString(0)}, Nama: {reader.GetString(1)}, Jenis: {reader.GetString(2)}, Warna: {reader.GetString(3)}, Jenis Bulu: {reader.GetString(4)}");
                }
            }
        }

        public void InsertCat(SqlConnection con)
        {
            Console.WriteLine("Input data Kucing\n");
            Console.WriteLine("Masukkan ID Kucing (4 karakter): ");
            string id = Console.ReadLine();
            Console.WriteLine("Masukkan Nama Kucing: ");
            string nama = Console.ReadLine();
            Console.WriteLine("Masukkan Jenis Kucing: ");
            string jenis = Console.ReadLine();
            Console.WriteLine("Masukkan Warna Kucing: ");
            string warna = Console.ReadLine();
            Console.WriteLine("Masukkan Jenis Bulu Kucing: ");
            string jenisBulu = Console.ReadLine();

            string query = "INSERT INTO Kucing (id_kucing, Nama_kucing, Jenis_kucing, Warna_kucing, Jenis_bulukucing) VALUES (@id, @nama, @jenis, @warna, @jenisBulu)";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@nama", nama);
            cmd.Parameters.AddWithValue("@jenis", jenis);
            cmd.Parameters.AddWithValue("@warna", warna);
            cmd.Parameters.AddWithValue("@jenisBulu", jenisBulu);

            cmd.ExecuteNonQuery();
            Console.WriteLine("Data berhasil ditambahkan");
        }

        public void DeleteCat(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Kucing yang ingin dihapus: ");
            string idToDelete = Console.ReadLine();

            string query = "DELETE FROM Kucing WHERE id_kucing = @id";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", idToDelete);
            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
                Console.WriteLine("Data berhasil dihapus");
            else
                Console.WriteLine("Data dengan ID tersebut tidak ditemukan");
        }

        public void SearchCat(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Kucing yang ingin dicari: ");
            string idToSearch = Console.ReadLine();

            string query = "SELECT id_kucing, Nama_kucing, Jenis_kucing, Warna_kucing, Jenis_bulukucing FROM Kucing WHERE id_kucing = @id";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", idToSearch);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine($"ID: {reader.GetString(0)}, Nama: {reader.GetString(1)}, Jenis: {reader.GetString(2)}, Warna: {reader.GetString(3)}, Jenis Bulu: {reader.GetString(4)}");
                }
                else
                {
                    Console.WriteLine("Data tidak ditemukan");
                }
            }
        }

        public void UpdateCat(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Kucing yang ingin diperbarui: ");
            string idToUpdate = Console.ReadLine();

            string selectQuery = "SELECT Nama_kucing, Jenis_kucing, Warna_kucing, Jenis_bulukucing FROM Kucing WHERE id_kucing = @id";
            SqlCommand selectCmd = new SqlCommand(selectQuery, con);
            selectCmd.Parameters.AddWithValue("@id", idToUpdate);

            using (SqlDataReader reader = selectCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string currentNama = reader.GetString(0);
                    string currentJenis = reader.GetString(1);
                    string currentWarna = reader.GetString(2);
                    string currentJenisBulu = reader.GetString(3);

                    Console.WriteLine($"Data saat ini - Nama: {currentNama}, Jenis: {currentJenis}, Warna: {currentWarna}, Jenis Bulu: {currentJenisBulu}");

                    Console.WriteLine("\nMasukkan informasi baru:");

                    Console.WriteLine("Nama Kucing (kosongkan jika tidak ingin mengubah): ");
                    string newNama = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newNama))
                        newNama = currentNama;

                    Console.WriteLine("Jenis Kucing (kosongkan jika tidak ingin mengubah): ");
                    string newJenis = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newJenis))
                        newJenis = currentJenis;

                    Console.WriteLine("Warna Kucing (kosongkan jika tidak ingin mengubah): ");
                    string newWarna = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newWarna))
                        newWarna = currentWarna;

                    Console.WriteLine("Jenis Bulu Kucing (kosongkan jika tidak ingin mengubah): ");
                    string newJenisBulu = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newJenisBulu))
                        newJenisBulu = currentJenisBulu;

                    string updateQuery = "UPDATE Kucing SET Nama_kucing = @nama, Jenis_kucing = @jenis, Warna_kucing = @warna, Jenis_bulukucing = @jenisBulu WHERE id_kucing = @id";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                    updateCmd.Parameters.AddWithValue("@id", idToUpdate);
                    updateCmd.Parameters.AddWithValue("@nama", newNama);
                    updateCmd.Parameters.AddWithValue("@jenis", newJenis);
                    updateCmd.Parameters.AddWithValue("@warna", newWarna);
                    updateCmd.Parameters.AddWithValue("@jenisBulu", newJenisBulu);

                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        Console.WriteLine("Data berhasil diperbarui");
                    else
                        Console.WriteLine("Data gagal diperbarui");
                }
                else
                {
                    Console.WriteLine("Data tidak ditemukan");
                }
            }
        }
    }
}
