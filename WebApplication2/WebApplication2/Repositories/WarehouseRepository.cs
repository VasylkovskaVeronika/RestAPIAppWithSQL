using Microsoft.Data.SqlClient;

namespace WebApplication2.Repositories;

public interface IWarehouseRepository
{
    public bool ProductWithIDExists(int id);
    public bool WarehouseWithIDExists(int id);
    public bool AddProductRequirementsCompleted(int pId, int amount, DateTime createdAt);
    public bool IsOrderCompleted(int pId, int a);
    public void UpdateFullfilledAt(int pId, int a);
    public int InsertProductWarehouse(int pId, int wId, int a);
}
public class WarehouseRepository: IWarehouseRepository
{
    private readonly IConfiguration _configuration;

    public WarehouseRepository(IConfiguration c)
    {
        _configuration = c;
    }
    
    public bool ProductWithIDExists(int id)
    {
        using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
        {
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            con.Open();
            com.CommandText = "SELECT COUNT(IdProduct) FROM Product WHERE IdProduct =@idProduct";
            com.Parameters.AddWithValue("@idProduct", id);
            int resultSet = (Int32)com.ExecuteScalar();
            if ((int)resultSet == 0) 
                return false; 
            return true;
        }
    }

    public bool WarehouseWithIDExists(int id)
    {
        using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
        {
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            con.Open();
            com.CommandText = "SELECT COUNT(IdWarehouse) FROM Warehouse WHERE IdWarehouse =@idWarehouse";
            com.Parameters.AddWithValue("idWarehouse", id);
            int resultSet = (Int32)com.ExecuteScalar();
            if ((int)resultSet == 0) 
                return false;
            return true;
        }
    }

    public bool AddProductRequirementsCompleted(int pId, int amount, DateTime createdAt)
    {
        using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
        {
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            con.Open();
            com.CommandText = "SELECT COUNT(IdOrder) FROM [Order] WHERE IdProduct =@IdProduct AND Amount >=@Amount AND CreatedAt>@CreatedAt";
            com.Parameters.AddWithValue("IdProduct", pId);
            com.Parameters.AddWithValue("Amount", amount);
            com.Parameters.AddWithValue("CreatedAt", createdAt);
            int resultSet = (Int32)com.ExecuteScalar();
            if (resultSet == 0) 
                return false; 
            //com.CommandText = "SELECT CreatedAt FROM [Order] WHERE IdProduct =@IdProduct AND Amount =@Amount";
            //com.Parameters.AddWithValue("IdProduct", pId);
            //com.Parameters.AddWithValue("Amount", amount);
            //if ((DateTime)com.ExecuteScalar() > createdAt) 
                //return false;
            return true;
        }
    }

    public bool IsOrderCompleted(int pId, int a)
    {
        using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
        {
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            con.Open();
            com.CommandText = "SELECT count(IdProductWarehouse) from Product_Warehouse Where IdOrder = (SELECT IdOrder FROM [Order] WHERE IdProduct =@IdProduct AND Amount =@Amount)";
            com.Parameters.AddWithValue("IdProduct", pId);
            com.Parameters.AddWithValue("Amount", a);
            int exists = (int)com.ExecuteScalar();
            if (exists == 0) 
                return true;
            return false;
        }
    }

    public void UpdateFullfilledAt(int pId, int a)
    {
        using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
        {
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            con.Open();
            com.CommandText = "UPDATE [Order] SET FulfilledAt = GETDATE() WHERE IdOrder = (SELECT idOrder FROM [Order] WHERE IdProduct =@IdProduct AND Amount =@Amount)";
            com.Parameters.AddWithValue("IdProduct", pId);
            com.Parameters.AddWithValue("Amount", a);
            com.ExecuteNonQuery();
        }
    }

    public int InsertProductWarehouse(int pId, int wId, int a)
    {
        using (SqlConnection con = new SqlConnection(_configuration.GetConnectionString("ProductionDb")))
        {
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            con.Open();
            com.CommandText = "SET IDENTITY_INSERT Product_Warehouse ON;INSERT INTO Product_Warehouse (IdProductWarehouse, IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt)VALUES (((SELECT ISNULL(Max(IdProductWarehouse),0) FROM Product_Warehouse) + 1), @IdWarehouse, @IdProduct,(SELECT idOrder FROM [Order] WHERE IdProduct =@IdProduct AND Amount =@Amount),@Amount, ((SELECT Price FROM Product WHERE IdProduct = @IdProduct) * @Amount), GETDATE());SET IDENTITY_INSERT Product_Warehouse OFF";
            com.Parameters.AddWithValue("IdProduct", pId);
            com.Parameters.AddWithValue("Amount", a);
            com.Parameters.AddWithValue("IdWarehouse", wId);
            com.ExecuteNonQuery();
            com.CommandText = "SELECT IdProductWarehouse from Product_Warehouse Where IdProduct = @IdProduct and IdWarehouse = @IdWarehouse and Amount = @Amount and Price = ((SELECT Price FROM Product WHERE IdProduct = @IdProduct) * @Amount)";
            int id = (int)com.ExecuteScalar();
            return id;
        }
    }
}