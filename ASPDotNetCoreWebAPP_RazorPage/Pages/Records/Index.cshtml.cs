using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data;
using System.Data.SqlClient;

namespace ASPDotNetCoreWebAPP_RazorPage.Pages.Records
{
    public class IndexModel : PageModel
    {
        public List<RecordInfo> recordInfos = new List<RecordInfo>();
        public readonly string connectionString;
        public IndexModel(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:SqlServerDb"] ?? "";
        }
        public void OnGet()
        {
             DataTable dataTable = new DataTable();
            try
            {
               
               // string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string todayDate = DateTime.Now.ToString("yyyy-MM-dd");
                    string sql = "SELECT * FROM Records where appointmentDate='"+ todayDate+"'";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {


                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            
                            while (reader.Read())
                            {
                                RecordInfo recordInfo = new RecordInfo();
                                recordInfo.id = "" + reader.GetInt32(0);
                                recordInfo.name = reader.GetString(1);
                                recordInfo.appointmentDate = reader.GetString(2);
                                recordInfo.status = reader.GetString(3);
                                recordInfo.adminComment  = reader.GetString(4);

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
   
    public class RecordInfo
    {
        public string id;

        public string name;
        public string status;
        public string appointmentDate;
        public string adminComment;
    }
}
