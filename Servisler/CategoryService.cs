using B2b_Api.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;

namespace B2b_Api.Servisler
{
    public class CategoryService
    {
        private readonly string _connectionString;
        string hataMesaji = "";
      

        public CategoryService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["hrz_baglanti"].ConnectionString;
        }
        public Sonuc KategorileriOku()
        {
            Sonuc sonuc = new Sonuc();
            sonuc.data = GetAll();
            if (sonuc.data != null)
            {
                sonuc.mesaj = "Başarılı";
                sonuc.ekData = null;
                sonuc.veriOkuBasari = true;
                sonuc.sonuc = true;
            }
            else
            {
                sonuc.mesaj = "Kategori Listesi alınamadı. " + hataMesaji;
                sonuc.ekData = null;
                sonuc.veriOkuBasari = false;
                sonuc.sonuc = false;
            }
            return sonuc;
        }
        public Sonuc KategoriEkle(Category model)
        {
            Sonuc sonuc = new Sonuc();
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                sonuc.mesaj = "Kategori adı zorunludur.";
                sonuc.data = model;
                sonuc.ekData = null;
                sonuc.veriOkuBasari = false;
                sonuc.sonuc = false;
                return sonuc;
            }

            if (string.IsNullOrWhiteSpace(model.Code))
            {
                sonuc.mesaj = "Kategori kodu zorunludur.";
                sonuc.ekData = null;
                sonuc.data = model;
                sonuc.veriOkuBasari = false;
                sonuc.sonuc = false;
                return sonuc;
            }

            if (CodeExists(model.Code))
            {
                sonuc.mesaj = "Bu kategori kodu zaten kullanılıyor.";
                sonuc.ekData = null;
                sonuc.data = model;
                sonuc.veriOkuBasari = false;
                sonuc.sonuc = false;
                return sonuc;
            }

            if (model.ParentId.HasValue && !ParentExists(model.ParentId.Value))
            {
                sonuc.mesaj = "Üst kategori bulunamadı.";
                sonuc.ekData = null;
                sonuc.data = model;
                sonuc.veriOkuBasari = false;
                sonuc.sonuc = false;
                return sonuc;
            }
           
            Category cat = Create(model);
            if (cat != null)
            {
                sonuc.mesaj = "Kategori başarıyla eklendi.";
                sonuc.ekData = null;
                sonuc.data = cat;
                sonuc.veriOkuBasari = true;
                sonuc.sonuc = true;
            }
            else
            {
                sonuc.mesaj = "Kategori eklenemedi. " + hataMesaji;
                sonuc.ekData = null;
                sonuc.data = model;
                sonuc.veriOkuBasari = false;
                sonuc.sonuc = false;
            }
            return sonuc;
        }
        public Sonuc KategoriGuncelle(Category model)
        {
            Sonuc sonuc = new Sonuc();
            if (model.Id <= 0)
            {
                sonuc.mesaj = "Geçersiz kategori ID'si.";
                sonuc.ekData = null;
                sonuc.data = model;
                sonuc.veriOkuBasari = false;
                sonuc.sonuc = false;
                return sonuc;
            }
            var existing = GetById(model.Id);
            if (existing == null)
            {
                sonuc.mesaj = "Kategori tablosu okunamadı.";
                sonuc.ekData = null;
                sonuc.data = model;
                sonuc.veriOkuBasari = false;
                sonuc.sonuc = false;
                return sonuc;
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                sonuc.mesaj = "Kategori adı zorunludur.";
                sonuc.ekData = null;
                sonuc.data = model;
                sonuc.veriOkuBasari = false;
                sonuc.sonuc = false;
                return sonuc;
            }

            if (string.IsNullOrWhiteSpace(model.Code))
            {
                sonuc.mesaj = "Kategori kodu zorunludur.";
                sonuc.ekData = null;
                sonuc.data = model;
                sonuc.veriOkuBasari = false;
                sonuc.sonuc = false;
                return sonuc;
            }

            if (CodeExists(model.Code, model.Id))
            {
                sonuc.mesaj = "Bu kategori kodu zaten kullanılıyor.";
                sonuc.ekData = null;
                sonuc.data = model;
                sonuc.veriOkuBasari = false;
                sonuc.sonuc = false;
                return sonuc;
            }

            if (model.ParentId.HasValue)
            {
                if (model.ParentId.Value == model.Id)
                {
                    sonuc.mesaj = "Kategori kendisinin alt kategorisi olamaz.";
                    sonuc.ekData = null;
                    sonuc.data = model;
                    sonuc.veriOkuBasari = false;
                    sonuc.sonuc = false;
                    return sonuc;
                }

                if (!ParentExists(model.ParentId.Value))
                {
                    sonuc.mesaj = "Üst kategori bulunamadı.";
                    sonuc.ekData = null;
                    sonuc.data = model;
                    sonuc.veriOkuBasari = false;
                    sonuc.sonuc = false;
                    return sonuc;
                }

                if (IsDescendant(model.Id, model.ParentId.Value))
                {
                    sonuc.mesaj = "Döngüsel bağımlılık oluşur.";
                    sonuc.ekData = null;
                    sonuc.data = model;
                    sonuc.veriOkuBasari = false;
                    sonuc.sonuc = false;
                    return sonuc;
                }
            }
            Category cat = Update(model);
            if (cat != null)
            {
                sonuc.mesaj = "Kategori başarıyla güncellendi.";
                sonuc.ekData = null;
                sonuc.data = cat;
                sonuc.veriOkuBasari = true;
                sonuc.sonuc = true;
            }
            else
            {
                sonuc.mesaj = "Kategori güncellenemedi. " + hataMesaji;
                sonuc.ekData = null;
                sonuc.data = model;
                sonuc.veriOkuBasari = false;
                sonuc.sonuc = false;
            }
            return sonuc;
        }
        public Sonuc KategoriSil(int id)
        {
            Sonuc sonuc = new Sonuc();
            if (id <= 0)
            {
                sonuc.mesaj = "Geçersiz kategori ID'si.";
                sonuc.ekData = null;
                sonuc.data = id;
                sonuc.veriOkuBasari = false;
                sonuc.sonuc = false;
                return sonuc;
            }
            var existing = GetById(id);
            if (existing == null)
            {
                sonuc.mesaj = "İlgili kategori id bulunamadı.";
                sonuc.ekData = null;
                sonuc.data = id;
                sonuc.veriOkuBasari = false;
                sonuc.sonuc = false;
                return sonuc;
            }

            if (HasChildren(id))
            {
                sonuc.mesaj = "Alt kategorileri olan bir kategori silinemez.";
                sonuc.ekData = null;
                sonuc.data = id;
                sonuc.veriOkuBasari = false;
                sonuc.sonuc = false;
                return sonuc;
            }
            
            if (Delete(id))
            {
                sonuc.mesaj = "Kategori başarıyla sillindi.";
                sonuc.ekData = null;
                sonuc.data = id;
                sonuc.veriOkuBasari = true;
                sonuc.sonuc = true;
            }
            else
            {
                sonuc.mesaj = "Kategori silinemedi. " + hataMesaji;
                sonuc.ekData = null;
                sonuc.data = id;
                sonuc.veriOkuBasari = false;
                sonuc.sonuc = false;
            }
            return sonuc;
        }
        public Sonuc KategoriAcKapa(int id)
        {
            Sonuc sonuc = new Sonuc();
            if (id <= 0)
            {
                sonuc.mesaj = "Geçersiz kategori ID'si.";
                sonuc.ekData = null;
                sonuc.data = id;
                sonuc.veriOkuBasari = false;
                sonuc.sonuc = false;
                return sonuc;
            }
            var existing = GetById(id);
            if (existing == null)
            {
                sonuc.mesaj = "İlgili kategori id bulunamadı.";
                sonuc.ekData = null;
                sonuc.data = id;
                sonuc.veriOkuBasari = false;
                sonuc.sonuc = false;
                return sonuc;
            }
            Category cat = Toggle(id);
            if (cat != null)
            {
                sonuc.mesaj = "Kategori başarıyla açıldı/Kapandı.";
                sonuc.ekData = null;
                sonuc.data = cat;
                sonuc.veriOkuBasari = true;
                sonuc.sonuc = true;
            }
            else
            {
                sonuc.mesaj = "Kategori açma/kapama işlemi yapılamadı. " + hataMesaji;
                sonuc.ekData = null;
                sonuc.data = id;
                sonuc.veriOkuBasari = false;
                sonuc.sonuc = false;
            }
            return sonuc;
        }

        public List<Category> GetAll()
        {
            try
            {
                var list = new List<Category>();
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand("SELECT Id, Name, Code, ParentId, IsActive, SortOrder, CreatedAt, UpdatedAt FROM Categories ORDER BY SortOrder, Name", conn);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(MapCategory(reader));
                        }
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                hataMesaji = ex.Message;
                return null;
            }
          
        }

        public Category GetById(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand("SELECT Id, Name, Code, ParentId, IsActive, SortOrder, CreatedAt, UpdatedAt FROM Categories WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) return MapCategory(reader);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                hataMesaji = ex.Message;
                return null;
            }
            
        }

        public bool CodeExists(string code, int? excludeId = null)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var sql = "SELECT COUNT(1) FROM Categories WHERE Code = @Code";
                if (excludeId.HasValue) sql += " AND Id <> @ExcludeId";
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Code", code);
                if (excludeId.HasValue) cmd.Parameters.AddWithValue("@ExcludeId", excludeId.Value);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public bool ParentExists(int parentId)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT COUNT(1) FROM Categories WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", parentId);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public bool HasChildren(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT COUNT(1) FROM Categories WHERE ParentId = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        public bool IsDescendant(int categoryId, int potentialParentId)
        {
            var all = new List<Tuple<int, int?>>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT Id, ParentId FROM Categories", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        all.Add(Tuple.Create(reader.GetInt32(0), reader.IsDBNull(1) ? (int?)null : reader.GetInt32(1)));
                    }
                }
            }

            var descendants = new HashSet<int>();
            CollectDescendants(categoryId, all, descendants);
            return descendants.Contains(potentialParentId);
        }

        private void CollectDescendants(int parentId, List<Tuple<int, int?>> all, HashSet<int> result)
        {
            foreach (var item in all)
            {
                if (item.Item2.HasValue && item.Item2.Value == parentId && !result.Contains(item.Item1))
                {
                    result.Add(item.Item1);
                    CollectDescendants(item.Item1, all, result);
                }
            }
        }

        public Category Create(Category model)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand(@"INSERT INTO Categories (Name, Code, ParentId, IsActive, SortOrder, CreatedAt, UpdatedAt)
                    VALUES (@Name, @Code, @ParentId, @IsActive, @SortOrder, GETDATE(), GETDATE());
                    SELECT SCOPE_IDENTITY();", conn);
                    cmd.Parameters.AddWithValue("@Name", model.Name);
                    cmd.Parameters.AddWithValue("@Code", model.Code);
                    cmd.Parameters.AddWithValue("@ParentId", (object)model.ParentId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                    cmd.Parameters.AddWithValue("@SortOrder", model.SortOrder);
                    var id = Convert.ToInt32(cmd.ExecuteScalar());
                    return GetById(id);
                }
            }
            catch (Exception ex)
            {
                hataMesaji = ex.Message;
                return null;
            }
          
        }

        public Category Update(Category model)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand(@"UPDATE Categories SET Name=@Name, Code=@Code, ParentId=@ParentId, 
                    IsActive=@IsActive, SortOrder=@SortOrder, UpdatedAt=GETDATE() WHERE Id=@Id", conn);
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@Code", model.Code);
                cmd.Parameters.AddWithValue("@ParentId", (object)model.ParentId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                cmd.Parameters.AddWithValue("@SortOrder", model.SortOrder);
                cmd.ExecuteNonQuery();
                return GetById(model.Id);
            }
        }

        public bool Delete(int id)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand("DELETE FROM Categories WHERE Id = @Id", conn);
                    cmd.Parameters.AddWithValue("@Id", id);
                    int x = cmd.ExecuteNonQuery();
                    if ( x <= 0)
                    {
                        return false;  
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                hataMesaji = ex.Message;
                return false;
            }
          
        }

        public Category Toggle(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("UPDATE Categories SET IsActive = ~IsActive, UpdatedAt = GETDATE() WHERE Id = @Id", conn);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
                return GetById(id);
            }
        }

        private static Category MapCategory(SqlDataReader reader)
        {
            return new Category
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Code = reader.GetString(2),
                ParentId = reader.IsDBNull(3) ? (int?)null : reader.GetInt32(3),
                IsActive = reader.GetBoolean(4),
                SortOrder = reader.GetInt32(5),
                CreatedAt = reader.GetDateTime(6),
                UpdatedAt = reader.GetDateTime(7)
            };
        }
    }
}
