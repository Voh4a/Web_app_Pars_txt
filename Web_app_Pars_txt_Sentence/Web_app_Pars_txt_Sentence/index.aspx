<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Web_app_Pars_txt_Sentence.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <title>Find word in file</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:FileUpload ID="FileUpload_Client" runat="server"/> <br />
            <asp:Button ID="Button_Import" runat="server" Text="Завантажити" OnClick="Button_Import_Click"/> <br />

            <asp:Label ID="Label1" runat="server"></asp:Label> 
            <br />
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlSentenceDB">
                <Columns>
                    <asp:BoundField DataField="Word" HeaderText="Слово для пошуку" SortExpression="Word" />
                    <asp:BoundField DataField="Sentence" HeaderText="Речення в якому знайдено слово" SortExpression="Sentence" />
                    <asp:BoundField DataField="Number_of_words" HeaderText="Кількість знайдених слів" SortExpression="Number_of_words" />
                </Columns>
            </asp:GridView>
            <asp:SqlDataSource ID="SqlSentenceDB" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_SentenceDB %>" 
                SelectCommand="SELECT [Word], [Sentence], [Number_of_words] FROM [T_Sentence]">
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
