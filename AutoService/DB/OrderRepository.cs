using System;
using System.Collections.Generic;
using AutoService.Models;
using Microsoft.Extensions.Options;
using MySqlConnector;

namespace AutoService.DB;

public class OrderRepository
{
    MySqlConnection connection;
    public OrderRepository(IOptions<DatabaseConnection> conString)
    {
        connection = new MySqlConnection(conString.Value.ConnectionString);
    }

    public void InsertOrder(Order order, List<Work> works)
    {
        connection.Open();
        int maxOrderId = 0;
        
        string sql1 =
            "INSERT INTO auto_service_db.orders (id, client_name, car_model, service_id, total_amount, discount_percent, order_date) VALUES(0, @client_name, @car_model, @service_id, @total_amount, @discount_percent, @order_date);";

        string sql2 = 
            "select max(id) as id from auto_service_db.orders;";
        
        string sql3 =
            "INSERT INTO auto_service_db.order_items (id, order_id, work_id, work_price) VALUES(0, @order_id, @work_id, @work_price);";

        using var transaction = connection.BeginTransaction();
        try
        {
            using (var mc = new MySqlCommand(sql1, connection, transaction))
            {
                mc.Parameters.AddWithValue("@client_name", order.ClientName);
                mc.Parameters.AddWithValue("@car_model", order.CarModel);
                mc.Parameters.AddWithValue("@service_id", order.ServiceId);
                mc.Parameters.AddWithValue("@total_amount", order.TotalAmount);
                mc.Parameters.AddWithValue("@discount_percent", order.DiscountPercent);
                mc.Parameters.AddWithValue("@order_date", order.OrderDate);
                mc.ExecuteNonQuery();
            }

            using (var mc = new MySqlCommand(sql2, connection, transaction))
            {
                using (var dr = mc.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        maxOrderId = dr.GetInt32("id");
                    }
                }
            }

            //не работает
            foreach (var work in works)
            {
                using var mc = new MySqlCommand(sql3, connection, transaction);
                mc.Parameters.AddWithValue("@order_id", maxOrderId);
                mc.Parameters.AddWithValue("@WorkPrice", work.Price);
                mc.Parameters.AddWithValue("@WorkId", work.Id);
            }
            
            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
        }
        connection.Close();
    }
}