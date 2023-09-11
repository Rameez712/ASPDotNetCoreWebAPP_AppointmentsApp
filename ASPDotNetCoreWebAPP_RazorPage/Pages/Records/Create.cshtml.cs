using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Microsoft.Extensions.Primitives;

namespace ASPDotNetCoreWebAPP_RazorPage.Pages.Records
{
    public class CreateModel : PageModel
    {
        public readonly string connectionString;
        public CreateModel(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:SqlServerDb"] ?? "";
        }

        public RecordInfo recordInfo = new RecordInfo();
        public string errorMsg = string.Empty;
        public string successMsg = string.Empty;
        public void OnGet()
        {
        }
        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }
        public void OnPost()
        {
            recordInfo.name = Request.Form["name"];
            recordInfo.appointmentDate = Request.Form["appointmentDate"];
            recordInfo.adminComment = Request.Form["adminComment"];





            if (recordInfo.name.Length == 0 )
            {
                errorMsg = "Name is a required field";
                return;
            }


            //save database
            try
            {
                //string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO Records" +
                        "(name,appointmentDate,admincomment,status) VALUES" +
                         "(@name,@appointmentDate,@admincomment,@status)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.Parameters.AddWithValue("@name", recordInfo.name);
                        command.Parameters.AddWithValue("@appointmentDate", recordInfo.appointmentDate);
                        command.Parameters.AddWithValue("@admincomment", recordInfo.adminComment);
                        command.Parameters.AddWithValue("@status", "Active");
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }

   
            successMsg = "New Record Added Successfully";
            Response.Redirect("/Records/Index");




        }
    }
}
