using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Data.SqlClient;
using MvcSegundaPracticaACL.Models;
using System.Data;

namespace MvcSegundaPracticaACL.Repositories
{
    public class RepositoryComics
    {
        DataTable tablaComics;
        SqlConnection cn;
        SqlCommand com;
        public RepositoryComics()
        {
            string connectionString = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=COMICS;User ID=sa;Trust Server Certificate=True";
            string sql = "select * from COMICS";
            SqlDataAdapter ad = new SqlDataAdapter(sql, connectionString);
            this.tablaComics = new DataTable();
            ad.Fill(this.tablaComics);
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;

            List<Comic> comics = new List<Comic>();

            foreach (var row in consulta)
            {
                Comic comic = new Comic();
                comic.IdComic = row.Field<int>("IDCOMIC");
                comic.Nombre = row.Field<string>("NOMBRE");
                comic.Imagen = row.Field<string>("IMAGEN");
                comic.Descripcion = row.Field<string>("DESCRIPCION");
                comics.Add(comic);
            }
            return comics;
        }

        public Comic DetallesComic(int id)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           where datos.Field<int>("IDCOMIC") == id
                           select datos;

            var row = consulta.First();
            Comic comic = new Comic();
            comic.IdComic = row.Field<int>("IDCOMIC");
            comic.Nombre = row.Field<string>("NOMBRE");
            comic.Imagen = row.Field<string>("IMAGEN");
            comic.Descripcion = row.Field<string>("DESCRIPCION");
            return comic;
        }

        public async Task CreateComicAsync(string nombre, string img, string descripcion)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           select datos;
            int maxID = consulta.Max(x => x.Field<int>("IDCOMIC"));

            string sql = "INSERT INTO COMICS VALUES(@id, @nombre, @imagen, @descripcion)";
            this.com.Parameters.AddWithValue("@id", maxID+1);
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@imagen", img);
            this.com.Parameters.AddWithValue("@descripcion", descripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }
    }
}
