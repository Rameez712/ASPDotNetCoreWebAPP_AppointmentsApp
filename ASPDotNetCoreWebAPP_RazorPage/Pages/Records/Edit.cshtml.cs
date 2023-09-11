using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Linq;

namespace ASPDotNetCoreWebAPP_RazorPage.Pages.Records
{
    public class EditModel : PageModel
    {
        public RecordInfo recordInfo = new RecordInfo();
        public string errorMsg = string.Empty;
        public string successMsg = string.Empty;
        public void OnGet()
        {
            String id = Request.Query["id"];
            try
            {
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Records WHERE id=@id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                recordInfo.id = "" + reader.GetInt32(0);
                                recordInfo.name = reader.GetString(1);
                                recordInfo.appointmentDate = reader.GetString(2);
                                recordInfo.status = reader.GetString(3);
                                recordInfo.adminComment = reader.GetString(4);

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;

            }
        }
        public bool IsNumeric(string value)
        {
            return value.All(char.IsNumber);
        }
        public void OnPost()
        {
            recordInfo.id = Request.Query["id"];
            recordInfo.name = Request.Form["name"];
            recordInfo.appointmentDate = Request.Form["appointmentDate"];
            recordInfo.adminComment = Request.Form["adminComment"];
            recordInfo.status = Request.Form["status"];



            if (recordInfo.name.Length == 0)
            {
                errorMsg = "Name is a required field";
                return;
            }
    
            


            //save database
            try
            {
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=TestDB;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Records SET name=@name, appointmentDate=@appointmentDate ,status=@status, adminComment=@adminComment  WHERE id=@id";


                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        command.Parameters.AddWithValue("@name", recordInfo.name);
                        command.Parameters.AddWithValue("@appointmentDate", recordInfo.appointmentDate);
                        command.Parameters.AddWithValue("@adminComment", recordInfo.adminComment);
                        command.Parameters.AddWithValue("@status", recordInfo.status);
                        command.Parameters.AddWithValue("@id", recordInfo.id);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return;
            }
          
            successMsg = "New Record Edited Successfully";

            Response.Redirect("/Records/Index");




        }
    }
}
