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
    public class IndexModel : PageModel

    {
        SqlConnection sqlConnection;

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=kino;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            SqlCommand command = new SqlCommand("SELECT * FROM [afisha]", sqlConnection);
            
            using (DbDataReader reader = command.ExecuteReader())
            {
                int i = 1;
                while (reader.Read())
                {   
                    string small_box = "<div class='small_container box['><a href='tickets.html' class='box_link'><span class='box_title_type_s'>!</span><div class='box_title_s'><h2 class='box_title_big'>&</h2></div></a><video class='videos' autoplay='' loop='' muted='' poster=']'><source src='|'></video></div>";

                    string big_box = "<div class='big_container box['><form method='post' asp-page='Index'><a href='tickets.html' class='box_link' ><span class='box_title_type_b'>!</span><div class='box_title_b'><h2 class='box_title_big'>&</h2><p class='box_title_description'>^</p></div></a></form><video class='videos' autoplay='' loop='' muted='' poster=']'><source src='|'></video></div>";

                    string tmp = null;

                    
                    int index = reader.GetOrdinal("name");
                    string name = Convert.ToString(reader.GetValue(index));
                    Afisha_name = name;

                    index = reader.GetOrdinal("genre");
                    string genre = Convert.ToString(reader.GetValue(index));
                    Afisha_genre = genre;

                    index = reader.GetOrdinal("description");
                    string des = Convert.ToString(reader.GetValue(index));
                    Afisha_description = des;

                    index = reader.GetOrdinal("img");
                    string img = Convert.ToString(reader.GetValue(index));
                    Afisha_img = img;

                    index = reader.GetOrdinal("type");
                    int type = Convert.ToInt32(reader.GetValue(index));

                    index = reader.GetOrdinal("video");
                    string video = Convert.ToString(reader.GetValue(index));

                    if (type == 1) {
                        tmp = big_box;

                    }
                        if (type == 2)
                    {
                        tmp = small_box;

                    }
                    if (Afisha_box != null)
                    {

                        var tmpp = tmp.Replace("!", genre);
                        var tmppp = tmpp.Replace("&", name);
                        var tmpppp = tmppp.Replace("^", des);
                        var tmppppp = tmpppp.Replace("|", img);
                        var tmpppppp = tmppppp.Replace("]", video);
                        var final = tmppppp.Replace("[", Convert.ToString(i) );
                        Afisha_box = Afisha_box.Trim('"') + final.Trim('"');

                    }
                    else
                    {
                        var tmpp = tmp.Replace("!", genre);
                        var tmppp = tmpp.Replace("&", name);
                        var tmpppp = tmppp.Replace("^", des);
                        var tmppppp = tmpppp.Replace("|", img);
                        var tmpppppp = tmppppp.Replace("]", video);
                        var final = tmppppp.Replace("[", Convert.ToString(i));
                        Afisha_box = final.Trim('"');

                    }

                    i = i + 1;
                }
                
            }
            
        }

        public string Afisha_name { get; set; }
        public string Afisha_genre { get; set; }
        public string Afisha_description { get; set; }
        public string Afisha_img { get; set; }
        public string Afisha_box { get; set; }
        public void OnPost()
        {
           
        
        }
        
    }
}
