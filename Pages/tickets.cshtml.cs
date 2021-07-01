using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace kino.Pages
{
    public class TicketsModel : PageModel

    {
        SqlConnection sqlConnection;

        private readonly ILogger<IndexModel> _logger;

        public TicketsModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=kino;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("SELECT * FROM [nzal]", sqlConnection);
            
            using (DbDataReader reader = command.ExecuteReader())
            {
            
                while (reader.Read())
                {

                    
                    string time_box = "< div class='zal_info'><a class='zal_info_number'>Зал [</a><a class='zal_info_time'>&</a></div>";


                    string tmp = null;


                    int indexx = reader.GetOrdinal("name");
                    string name = Convert.ToString(reader.GetValue(indexx));
                    zal_name = name;

                    indexx = reader.GetOrdinal("time");
                    string time = Convert.ToString(reader.GetValue(indexx));
                    zal_time = time;

                    indexx = reader.GetOrdinal("number");
                    string number = Convert.ToString(reader.GetValue(indexx));
                    zal_number = number;


                    if (zal_box != null)
                    {
                        tmp = time_box;
                        var tmpp = tmp.Replace("[", number);
                        var final = tmpp.Replace("&", time);
                        zal_box = zal_box.Trim('"') + final.Trim('"');

                    }
                    else
                    {
                        tmp = time_box;
                        var tmpp = tmp.Replace("[", number);
                        var final = tmpp.Replace("&", time);
                        zal_box = final.Trim('"');
                    }

                }
                
            }
            
        }

        public string zal_name { get; set; }
        public string zal_time { get; set; }
        public string zal_number { get; set; }
        public string zal_box { get; set; }
        public void OnPost()
        {
        }
        
    }
}
