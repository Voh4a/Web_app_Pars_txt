<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Web_app_Pars_txt_Sentence.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
    <title>Find word in file</title>

    <style type="text/css">
        #body{
            display: flex;
            flex-direction: row;
            justify-content: center;
        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div id="body">
                <div id="main">
                    <asp:FileUpload ID="FileUpload_Client" runat="server"/> <br /> <br />

                    <span>Ввдеіть слово для пошуку</span>
                    <br />
                    <asp:TextBox ID="TextBox_Word" runat="server" Width="152px"></asp:TextBox>
                    <asp:RequiredFieldValidator 
                        ID="RequiredFieldValidatorName" 
                        runat="server" 
                        ControltoValidate="TextBox_Word"
                        ErrorMessage="Введіть слово"
                        Text="*" 
                        ForeColor="Red">
                    </asp:RequiredFieldValidator>
                    <br />

                    <asp:Label ID="Label_Error" runat="server"></asp:Label> 
                    <br />
                    
                    <asp:Button ID="Button_Import" runat="server" Text="Опрацювати" OnClick="Button_Import_Click"/> 
                    
                </div>
            <div id="output_table">
                <asp:GridView ID="GridView_Sentence" runat="server" AutoGenerateColumns="False" DataSourceID="SqlSentenceDB" CellPadding="4" ForeColor="#333333" GridLines="None">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="Word" HeaderText="Слово для пошуку" SortExpression="Word" />
                        <asp:BoundField DataField="Sentence" HeaderText="Речення(в зворотньому порядку)" SortExpression="Sentence" />
                        <asp:BoundField DataField="Number_of_words" HeaderText="Кількість знайдених слів в реченні" SortExpression="Number_of_words" />
                    </Columns>
                    <EditRowStyle BackColor="#7C6F57" />
                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#E3EAEB" />
                    <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                </asp:GridView>
                <asp:SqlDataSource ID="SqlSentenceDB" runat="server" ConnectionString="<%$ ConnectionStrings:Connection_SentenceDB %>" 
                    SelectCommand="SELECT [Word], [Sentence], [Number_of_words] FROM [T_Sentence] WHERE ([User_ID] = @User_ID)">
                    <SelectParameters>
                        <asp:CookieParameter CookieName="User_ID" DefaultValue="0" Name="User_ID" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
            </div>

        </div>
    </form>
</body>
</html>
