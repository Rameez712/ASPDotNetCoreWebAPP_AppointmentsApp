using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace ASPDotNetCoreWebAPP_RazorPage.Pages.Records
{
    public class searchModel : PageModel
    {
        public List<RecordInfo> recordInfos = new List<RecordInfo>();
        public RecordInfo recordInfo = new RecordInfo();
        public string errorMsg = string.Empty;
        public string successMsg = string.Empty;
        public readonly string connectionString;
        public searchModel(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:SqlServerDb"] ?? "";
        }
        public void OnPost()
        {

            DataTable dataTable = new DataTable();
            try
            {
                
                recordInfo.name = Request.Form["name"];
                recordInfo.appointmentDate = Request.Form["appointmentDate"];

                //string connectionString ="Data Source=.\\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = string.Empty;
                    if (recordInfo.name != "" && recordInfo.appointmentDate != "")
                    {
                         sql = "SELECT * FROM Records where name='" + recordInfo.name + "' and appointmentDate='" + recordInfo.appointmentDate + "'";
                    }
                    else if(recordInfo.name == "" && recordInfo.appointmentDate != "")
                    {
                        sql = "SELECT * FROM Records where appointmentDate='" + recordInfo.appointmentDate + "'";
                    }
                    else if (recordInfo.name != "" && recordInfo.appointmentDate == "")
                    {
                        sql = "SELECT * FROM Records where name='" + recordInfo.name + "'";
                    }
                    else
                    {
                        sql = "SELECT * FROM Records";
                    }
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            command.Parameters.AddWithValue("@name", recordInfo.name);
                            command.Parameters.AddWithValue("@appointmentDate", recordInfo.appointmentDate);
                            while (reader.Read())
                            {
                                RecordInfo recordInfo = new RecordInfo();
                                recordInfo.id = "" + reader.GetInt32(0);
                                recordInfo.name = reader.GetString(1);
                                recordInfo.appointmentDate = reader.GetString(2);
                                recordInfo.status = reader.GetString(3);
                                recordInfo.adminComment = reader.GetString(4);
                                recordInfos.Add(recordInfo);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
}
