<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="log.aspx.cs" Inherits="Go3Interration.log" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <table style="width:100%; font-family:Calibri; font-size:12px;" border="1">
            <asp:Repeater runat="server" ID="reper">
                <ItemTemplate>
                    <tr>
                     <td style="width:200px;"><%# Eval("Date") %></td>
                     <td style="width:200px; font-style:oblique; font-size:12px;"><%# Eval("MetodName") %></td>
                     <td><%# Eval("ReqModel") %></td>

                    </tr>
                
                </ItemTemplate>
            </asp:Repeater>

            </table>
        </div>
    </form>
</body>
</html>
