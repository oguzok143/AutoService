using System;
using System.Collections.Generic;
using System.Data;
using AutoService.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace AutoService.DB;

public class ServiceRepository
{
    MySqlConnection connection;
    public ServiceRepository(IOptions<DatabaseConnection> conString)
    {
        connection = new MySqlConnection(conString.Value.ConnectionString);
    }

    public List<Service> GetAll()
    {
        List<Service> services = new List<Service>();
        try
        {
            connection.Open();
            string sql = "select * from services s;";
            using (var mc = new MySqlCommand(sql, connection))
            using (var dr = mc.ExecuteReader())
            {
                while (dr.Read())
                {
                    services.Add(new Service
                    {
                        Id =  dr.GetInt32("id"),
                        Title = dr.GetString("title"),
                        Description = dr.GetString("description")
                    });
                }
            }
        }
        catch (MySqlException ex)
        {
            Console.WriteLine(ex);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();
        }
        return services;
    }
}
