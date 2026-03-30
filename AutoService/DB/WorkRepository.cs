using System;
using System.Collections.Generic;
using System.Data;
using AutoService.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace AutoService.DB;

public class WorkRepository
{
    MySqlConnection connection;
    public WorkRepository(IOptions<DatabaseConnection> conString)
    {
        connection = new MySqlConnection(conString.Value.ConnectionString);
    }

    public List<Work> GetAll()
    {
        List<Work> works = new List<Work>();
        try
        {
            connection.Open();
            string sql = "select * from works w;";
            using (var mc = new MySqlCommand(sql, connection))
            using (var dr = mc.ExecuteReader())
            {
                while (dr.Read())
                {
                    works.Add(new Work
                    {
                        Id =  dr.GetInt32("id"),
                        ServiceId = dr.GetInt32("service_id"),
                        WorkName = dr.GetString("work_name"),
                        Price = dr.GetDecimal("price")
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
        return works;
    }
}
