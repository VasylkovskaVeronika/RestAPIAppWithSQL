using Microsoft.Data.SqlClient;
using WebApplication2.Models;
using WebApplication2.Models.DTOs;

namespace WebApplication2.Repositories;

public interface IAnimalsRepository
{
    List<Animal> GetAnimals();
    bool UpdateAnimal(Animal animal);
    bool AddAnimal(AddAnimal animal);
    bool DeleteAnimal(int id);
   }

public class AnimalsRepository : IAnimalsRepository
{
        private readonly IConfiguration _configuration;

        public AnimalsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Animal> GetAnimals()
        {
            //string connStr = "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=apbd;Integrated Security=True;Connect" +
            // " Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi" +
            //" Subnet Failover=False";
            using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Default")); 
            conn.Open();

            using SqlCommand command = new SqlCommand("SELECT * FROM ANIMAL", conn);

            var reader = command.ExecuteReader();

            List<Animal> res = new List<Animal>();
            int idOrdinal = reader.GetOrdinal(("AnimalId"));
            int nameOfOrdinal = reader.GetOrdinal(("Name"));
            int descOrdinal = reader.GetOrdinal(("Description"));
            int catOrdinal = reader.GetOrdinal(("Category"));
            int areaOrdinal = reader.GetOrdinal(("Area"));
        
            while (reader.Read())
            {
                res.Add(new Animal(
                    reader.GetInt32(idOrdinal),
                    reader.GetString(nameOfOrdinal),
                    reader.GetString(descOrdinal),
                    reader.GetString(catOrdinal),
                    reader.GetString(areaOrdinal)
                ));
            }

            return res;
        }

        public bool UpdateAnimal(Animal updated)
        {
            using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Default"));
            conn.Open();

            using SqlCommand command = new SqlCommand();
            command.Connection = conn;
            command.CommandText= "UPDATE ANIMAL SET Name=@aname, Description=@ad, " +
                                 "Category=@ac, Area=@aa WHERE AnimalId=@ai";
            command.Parameters.AddWithValue("@aname", updated.Name);
            command.Parameters.AddWithValue("@ad", updated.Desc);
            command.Parameters.AddWithValue("@ac", updated.Category);
            command.Parameters.AddWithValue("@aa", updated.Area);
            command.Parameters.AddWithValue("@ai", updated.Id);
            if (command.ExecuteNonQuery() > 0) //checking number of rows affected by query
            {
                return true;
            }

            return false;
            
        }

        public bool AddAnimal(AddAnimal animal)
        {
            using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Default"));
            conn.Open();

            using SqlCommand command = new SqlCommand();
            command.Connection = conn;
            command.CommandText= "INSERT INTO ANIMAL VALUES(@aname, @ad, @ac, @aa)";
            command.Parameters.AddWithValue("@aname", animal.Name);
            command.Parameters.AddWithValue("@ad", animal.Desc);
            command.Parameters.AddWithValue("@ac", animal.Category);
            command.Parameters.AddWithValue("@aa", animal.Area);
            if (command.ExecuteNonQuery() > 0) //checking number of rows affected by query
            {
                return true;
            }

            return false;
        }

        public bool DeleteAnimal(int id)
        {
            using SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("Default"));
            conn.Open();

            using SqlCommand command = new SqlCommand();
            command.Connection = conn;
            command.CommandText= "DELETE FROM ANIMAL WHERE AnimalId=@ai";
            command.Parameters.AddWithValue("@ai", id);
            if (command.ExecuteNonQuery() > 0) //checking number of rows affected by query
            {
                return true;
            }

            return false;
        }
}