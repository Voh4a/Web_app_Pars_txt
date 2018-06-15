using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;

namespace Web_app_Pars_txt_Sentence
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button_Import_Click(object sender, EventArgs e)
        {
            // Скачуємо файл від клієнта
            string txtFile = Server.MapPath("~/Files/") + Path.GetFileName(FileUpload_Client.PostedFile.FileName);
            FileUpload_Client.SaveAs(txtFile);

            // Читаємо файл
            string txtFileData = File.ReadAllText(txtFile, System.Text.Encoding.GetEncoding(1251));

            foreach(string row in txtFileData.Split('.'))
            {
                Label1.Text += row + " New ";
            }
        }
    }
}