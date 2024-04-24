using System;
using System.Data.SqlClient;

namespace GroomingPetShop

{
    internal class Pelanggan
    {
        public void Main()
        {
            Pelanggan pr = new Pelanggan();
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
                                            Console.WriteLine("Data Pelanggan\n");
                                            pr.ReadCustomers(conn);
                                            break;
                                        case '2':
                                            Console.Clear();
                                            pr.InsertCustomer(conn);
                                            break;
                                        case '3':
                                            Console.Clear();
                                            pr.DeleteCustomer(conn);
                                            break;
                                        case '4':
                                            Console.Clear();
                                            pr.SearchCustomer(conn);
                                            break;
                                        case '5':
                                            Console.Clear();
                                            pr.UpdateCustomer(conn);
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

        public void ReadCustomers(SqlConnection con)
        {
            SqlCommand cmd = new SqlCommand("SELECT Id_pelanggan, Nama_pelanggan, Alamat, No_telp FROM Pelanggan", con);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Console.WriteLine($"ID: {reader.GetString(0)}, Nama: {reader.GetString(1)}, Alamat: {reader.GetString(2)}, No. Telp: {reader.GetString(3)}");
                }
            }
        }

        public void InsertCustomer(SqlConnection con)
        {
            Console.WriteLine("Input data Pelanggan\n");
            Console.WriteLine("Masukkan ID Pelanggan (8 karakter): ");
            string id = Console.ReadLine();
            Console.WriteLine("Masukkan Nama Pelanggan: ");
            string nama = Console.ReadLine();
            Console.WriteLine("Masukkan Alamat Pelanggan: ");
            string alamat = Console.ReadLine();
            Console.WriteLine("Masukkan No. Telp Pelanggan: ");
            string noTelp = Console.ReadLine();

            string query = "INSERT INTO Pelanggan (Id_pelanggan, Nama_pelanggan, Alamat, No_telp) VALUES (@id, @nama, @alamat, @noTelp)";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", id);
            cmd.Parameters.AddWithValue("@nama", nama);
            cmd.Parameters.AddWithValue("@alamat", alamat);
            cmd.Parameters.AddWithValue("@noTelp", noTelp);

            cmd.ExecuteNonQuery();
            Console.WriteLine("Data berhasil ditambahkan");
        }

        public void DeleteCustomer(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Pelanggan yang ingin dihapus: ");
            string idToDelete = Console.ReadLine();

            string query = "DELETE FROM Pelanggan WHERE Id_pelanggan = @id";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", idToDelete);
            int rowsAffected = cmd.ExecuteNonQuery();

            if (rowsAffected > 0)
                Console.WriteLine("Data berhasil dihapus");
            else
                Console.WriteLine("Data dengan ID tersebut tidak ditemukan");
        }

        public void SearchCustomer(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Pelanggan yang ingin dicari: ");
            string idToSearch = Console.ReadLine();

            string query = "SELECT Id_pelanggan, Nama_pelanggan, Alamat, No_telp FROM Pelanggan WHERE Id_pelanggan = @id";
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@id", idToSearch);

            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Console.WriteLine($"ID: {reader.GetString(0)}, Nama: {reader.GetString(1)}, Alamat: {reader.GetString(2)}, No. Telp: {reader.GetString(3)}");
                }
                else
                {
                    Console.WriteLine("Data tidak ditemukan");
                }
            }
        }

        public void UpdateCustomer(SqlConnection con)
        {
            Console.WriteLine("Masukkan ID Pelanggan yang ingin diperbarui: ");
            string idToUpdate = Console.ReadLine();

            string selectQuery = "SELECT Nama_pelanggan, Alamat, No_telp FROM Pelanggan WHERE Id_pelanggan = @id";
            SqlCommand selectCmd = new SqlCommand(selectQuery, con);
            selectCmd.Parameters.AddWithValue("@id", idToUpdate);

            using (SqlDataReader reader = selectCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    string currentNama = reader.GetString(0);
                    string currentAlamat = reader.GetString(1);
                    string currentNoTelp = reader.GetString(2);

                    Console.WriteLine($"Data saat ini - Nama: {currentNama}, Alamat: {currentAlamat}, No. Telp: {currentNoTelp}");

                    Console.WriteLine("\nMasukkan informasi baru:");

                    Console.WriteLine("Nama Pelanggan (kosongkan jika tidak ingin mengubah): ");
                    string newNama = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newNama))
                        newNama = currentNama;

                    Console.WriteLine("Alamat (kosongkan jika tidak ingin mengubah): ");
                    string newAlamat = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newAlamat))
                        newAlamat = currentAlamat;

                    Console.WriteLine("No. Telp (kosongkan jika tidak ingin mengubah): ");
                    string newNoTelp = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(newNoTelp))
                        newNoTelp = currentNoTelp;

                    string updateQuery = "UPDATE Pelanggan SET Nama_pelanggan = @nama, Alamat = @alamat, No_telp = @noTelp WHERE Id_pelanggan = @id";
                    SqlCommand updateCmd = new SqlCommand(updateQuery, con);
                    updateCmd.Parameters.AddWithValue("@id", idToUpdate);
                    updateCmd.Parameters.AddWithValue("@nama", newNama);
                    updateCmd.Parameters.AddWithValue("@alamat", newAlamat);
                    updateCmd.Parameters.AddWithValue("@noTelp", newNoTelp);

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

        // Fungsi lainnya seperti CreateDatabase, ReadCustomers, InsertCustomer, DeleteCustomer, SearchCustomer
        // Dapat dipertahankan dari kode sebelumnya
    }
}
