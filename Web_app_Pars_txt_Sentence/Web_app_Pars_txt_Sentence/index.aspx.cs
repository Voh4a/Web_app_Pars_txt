using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;

namespace Web_app_Pars_txt_Sentence
{
    public partial class index : System.Web.UI.Page
    {
        // екземпляр підключення до бд
        private SqlConnection connectionDB;

        // метод для підрахунку кількості входжень підстроки в строку
        private int CountWords(string str, string substr)
        {
            int count = (str.Length - str.Replace(substr, "").Length) / substr.Length;
            return count;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // підключення до бд
                string connectionString = ConfigurationManager.ConnectionStrings["Connection_SentenceDB"].ConnectionString;
                connectionDB = new SqlConnection(connectionString);
                connectionDB.Open();
            }
            catch(Exception ex)
            {
                Label_Error.Text = "ОЙ, Помилка з базою даних. Покажіть наступний код програмісту: " + ex.Message;
                Label_Error.ForeColor = Color.Red;
                return;
            }


            /* Далі я роблю що коли користувач перший раз заходить на сайт то 
             * за допомогою SEQUENCE Generate_ID_for_Users база генерує йому ID(PK_Sentence)
             * яке записується йому в cookie щоб при наступному відкриванні сайту
             * йому показувались його минулі речення та слова які він шукав
             */

            // Перевіряємо чи не записано вже ID
            if (Request.Cookies["User_ID"] == null)
            {
                SqlDataReader reader = null;
                try
                {
                    // витягуємо з бази згнероване число
                    SqlCommand command = new SqlCommand("SELECT NEXT VALUE FOR Generate_ID_for_Users as Generate_ID", connectionDB);
                    reader = command.ExecuteReader();

                    // читаємо число
                    reader.Read();
                    string User_ID = Convert.ToString(reader["Generate_ID"]);

                    // записуємо в cookie
                    Response.Cookies["User_ID"].Value = Convert.ToString(User_ID);
                    Response.Cookies["User_ID"].Expires = DateTime.Now.AddDays(200);
                }
                catch(Exception ex)
                {
                    Label_Error.Text = "ОЙ, Помилка з базою даних. Покажіть наступний код програмісту: " + ex.Message;
                    Label_Error.ForeColor = Color.Red;
                }
                finally
                {
                    if(reader != null)
                    {
                        reader.Close();
                    }
                }
            }
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            // закриваємо підключення до бд
            if (connectionDB != null && connectionDB.State != System.Data.ConnectionState.Closed)
            {
                connectionDB.Close();
            }
        }

        protected void Button_Import_Click(object sender, EventArgs e)
        {
            // Додаткова перевірка валідатором (RequiredFieldValidator) якщо в клієнта відключений JS 
            if (!Page.IsValid)
            {
                Label_Error.Text = "Введіть слово";
                Label_Error.ForeColor = Color.Red;
                return;
            }

            string txtFile = null;

            try
            {
                // Скачуємо файл від клієнта
                txtFile = Server.MapPath("~/Files/") + Path.GetFileName(FileUpload_Client.PostedFile.FileName);
                FileUpload_Client.SaveAs(txtFile);
            }
            catch(Exception ex)
            {
                Label_Error.Text = "Виберіть файл";
                Label_Error.ForeColor = Color.Red;
                return;

            }

            if (txtFile != null)
            {
                // Читаємо файл
                string txtFileData = File.ReadAllText(txtFile, System.Text.Encoding.GetEncoding(1251));

                int count = 0; // для зберігання кількості входжень підстроки в строку
                string substr = TextBox_Word.Text; // зчитуємо слово для пошуку

                int User_ID = Convert.ToInt32(Request.Cookies["User_ID"].Value);

                // команда для запису в бд
                SqlCommand command = new SqlCommand("INSERT INTO [T_Sentence] ([User_ID], [Word], [Sentence], [Number_of_words]) VALUES(@User_ID, @Word, @Sentence, @Number_of_words)", connectionDB);

                // Розбиваєм текст на речення через '.'
                foreach (string row in txtFileData.Split('.'))
                {
                    // визиваємо метод для підрахунку кількості входжень
                    count = CountWords(row, substr);

                    // якщо кількість входжень не є 0
                    if(count > 0)
                    {
                        // робимо реверс строки (написано в завданні)
                        string revers_str = new string(row.ToCharArray().Reverse().ToArray());

                        try
                        {
                            // додаємо параметри
                            command.Parameters.AddWithValue("User_ID", Convert.ToInt32(User_ID));
                            command.Parameters.AddWithValue("Word", substr);
                            command.Parameters.AddWithValue("Sentence", revers_str);
                            command.Parameters.AddWithValue("Number_of_words", count);
                            
                            // викноуємо команду
                            command.ExecuteNonQuery();
                        }
                        catch(Exception ex)
                        {
                            Label_Error.Text = "ОЙ, Помилка з базою даних. Покажіть наступний код програмісту: " + ex.Message;
                            Label_Error.ForeColor = Color.Red;
                        }

                        // очищаємо від параметрів
                        command.Parameters.Clear();
                    }
                }

                // оновлюємо таблицю на клієнті
                GridView_Sentence.DataBind();
            }
        }
    }
}