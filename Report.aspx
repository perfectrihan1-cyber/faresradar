<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="Viman.Report" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report</title>

   

   
   

   

    <style>

     body {
    background: #f4f6fb;
    font-family: 'Segoe UI', sans-serif;
}

.content-wrapper {
    padding: 25px;
}

/* Search Cards */
.mobres {
    background: #fff;
    padding: 20px;
    border-radius: 12px;
    box-shadow: 0 2px 10px rgba(0,0,0,0.08);
    height: 100%;
}

.mobres label {
    font-weight: 600;
    color: #444;
    display: block;
    margin-bottom: 10px;
}

.txt_boxcss,
.form-control {
    border: 1px solid #dcdcdc !important;
    border-radius: 8px !important;
    padding: 10px !important;
    height: 45px !important;
}

/* Buttons */
.btn-primary,
.button,
#Button1,
#Button2,
#Button3,
#btnExport {
    background: #4f46e5 !important;
    border: none !important;
    color: #fff !important;
    border-radius: 8px !important;
    padding: 10px 20px !important;
    transition: .3s;
}

.btn-primary:hover,
#btnExport:hover {
    background: #3730a3 !important;
}

/* Main Report Card */
.card {
    border: none;
    border-radius: 15px;
    overflow: hidden;
    box-shadow: 0 4px 15px rgba(0,0,0,.08);
}

.card-body {
    padding: 25px;
}

.card-title {
    color: #222 !important;
    font-size: 28px;
    font-weight: 700;
    margin-bottom: 25px !important;
}

.card-title::after {
    background: #4f46e5 !important;
}

/* Table */
.table {
    margin-bottom: 0;
}

.table thead th {
    background: #4f46e5 !important;
    color: white !important;
    border: none !important;
    text-align: center;
}

.table tbody td {
    vertical-align: middle;
    text-align: center;
}

.table tbody tr:nth-child(even) {
    background: #f8f9ff !important;
}

.table tbody tr:hover {
    background: #eef2ff !important;
}

/* Pager */
.pagination,
table tr td table {
    margin-top: 15px;
}

table tr td table a,
table tr td table span {
    padding: 8px 12px;
    border-radius: 6px;
    margin: 0 3px;
    text-decoration: none;
}

table tr td table span {
    background: #4f46e5;
    color: #fff;
}

/* Export Button */
#btnExport {
    margin-top: 20px;
    min-width: 180px;
}

/* Responsive */
@media(max-width:768px){

    .d-flex{
        flex-direction:column;
    }

    .txt_boxcss,
    .form-control,
    .btn-primary{
        width:100% !important;
    }

    .card-body{
        padding:15px;
    }

    .table-responsive{
        overflow-x:auto;
    }
}

       
    </style>
</head>
<body>
    <form id="form1" runat="server">
          <div class="main-panel">
                <div class="content-wrapper">


                    <asp:ScriptManager ID="ScriptManager1" runat="server" />

                    <div class="row">
                        <div class="col-md-4 grid-margin stretch-card">
                            <div class="mobres">
                                <label for="txt_agent" style="margin-bottom: 10px;">Search By Booking ID:</label>
                                <div class="d-flex" style="flex: 1; gap: 10px;">

                                    <asp:TextBox ID="txt_bookingid" class="form-control form-control-lg txt_boxcss" runat="server" placeholder="Enter Booking ID"></asp:TextBox>

                                    <asp:Button ID="Button3" runat="server" Text="Search" class="btn btn-block btn-primary btn-lg font-weight-medium auth-form-btn" Style="margin-top: auto;" OnClick="Button1_Click" />
                                    
                                </div>
                            </div>
                        </div>

                        <div class="col-md-4 grid-margin stretch-card">
    <div class="mobres">
        <label for="txt_agent" style="margin-bottom: 10px;">Search By WebSite:</label>
        <div class="d-flex" style="flex: 1; gap: 10px;">

           <asp:DropDownList runat="server" ID="ddlwebsite" class="txt_boxcss" AutoPostBack="true" OnSelectedIndexChanged="ddlwebsite_SelectedIndexChanged"  >
                <asp:ListItem Value="0">Select Website</asp:ListItem>
               <asp:ListItem Value="Faresgator">Faresgator</asp:ListItem>
               <asp:ListItem Value="Parallelfares">Parallelfares</asp:ListItem>
               <asp:ListItem Value="Faresbridge">Faresbridge</asp:ListItem>
               <%--<asp:ListItem Value="Faresbundle">Faresbundle</asp:ListItem>--%>

           </asp:DropDownList>

           
           
        </div>
    </div>
</div>

                        <div class="col-md-8 grid-margin stretch-card">
                            <div class="mobres">
                                <label for="txt_pnr" style="margin-bottom: 10px;">Search By Date:</label>
                                <div class="d-flex" style="gap: 10px;">
                                    <ajaxToolkit:CalendarExtender runat="server" TargetControlID="txtfdate" Format="dd-MM-yyyy" ID="CalendarExtender3"></ajaxToolkit:CalendarExtender>
                                    <asp:TextBox ID="txtfdate" runat="server" class="form-control form-control-lg txt_boxcss" placeholder="Enter first date"></asp:TextBox>

                                    <ajaxToolkit:CalendarExtender runat="server" TargetControlID="txtscdate" Format="dd-MM-yyyy" ID="CalendarExtender4"></ajaxToolkit:CalendarExtender>
                                    <asp:TextBox ID="txtscdate" runat="server" class="form-control form-control-lg txt_boxcss" placeholder="Enter second date"></asp:TextBox>

                                    <asp:Button ID="Button1" runat="server" Text="Search" class="btn btn-block btn-primary btn-lg font-weight-medium auth-form-btn" OnClick="btn_fdate_Click" />
                                    <asp:Button ID="Button2" runat="server" Text="Reset" class="btn btn-block btn-primary btn-lg font-weight-medium auth-form-btn" OnClick="Button2_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12 grid-margin stretch-card">
                            <div class="card">
                                <div class="card-body">
                                    <p class="card-title" style="margin-bottom: 5px;">Flight Report</p>
                                    <div class="table-responsive">

                                        <asp:Timer ID="Timer1" runat="server" OnTick="Timer1_Tick" Interval="100000" />
                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" DataKeyNames="id" AllowPaging="true" EnableViewState="false"
                                            OnPageIndexChanging="GridView1_PageIndexChanging" PageSize="10" CssClass="table table-bordered">
                                              <RowStyle BackColor="#bebee9" />
                                       <AlternatingRowStyle BackColor="#7080d8" />
                                            <Columns>
                                                <asp:BoundField DataField="BookingId" HeaderText="BookingId" />
                                                <asp:BoundField DataField="SiteCode" HeaderText="Website Name" />
                                                <asp:BoundField DataField="FullName" HeaderText="Name" />
                                                <asp:BoundField DataField="Email" HeaderText="Email" />
                                                <asp:BoundField DataField="PhoneNo" HeaderText="PhoneNo" />
                                                <asp:BoundField DataField="DepDate" HeaderText="Departure Date" DataFormatString="{0:dd-MM-yyyy}" />
                                                <asp:TemplateField HeaderText="Return Date">
                                                    <ItemTemplate>
                                                        <%# Eval("RetDate") == DBNull.Value || Convert.ToDateTime(Eval("RetDate")) == DateTime.MinValue 
                                                            ? "" 
                                                             : Convert.ToDateTime(Eval("RetDate")).ToString("dd-MM-yyyy") %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:BoundField DataField="RetDate" HeaderText="RetDate" DataFormatString="{0:dd-MM-yyyy}" />--%>
                                                <asp:BoundField DataField="FinalTotalAmt" HeaderText="Amont" />
                                                <asp:BoundField DataField="FromCity" HeaderText="From City" />
                                                <asp:BoundField DataField="ToCity" HeaderText="To City" />
                                                <asp:BoundField DataField="Timestamp" HeaderText="Date" DataFormatString="{0:dd-MM-yyyy}" />
                                            </Columns>
                                            <PagerSettings Mode="NextPreviousFirstLast" FirstPageText="First" PreviousPageText="Previous" NextPageText="Next" LastPageText="Last" />


                                        </asp:GridView>

                                        <asp:Label ID="Label2" runat="server" Text="Choose Pages No." Font-Bold="True"
                                            ForeColor="#7080d8"></asp:Label>
                                        <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">

                                            <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                            <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                            <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                            <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                            <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                        </asp:DropDownList>





                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <br />
                    <asp:Button ID="btnExport" runat="server" Text="Export To Excel" class="btn btn-block btn-primary btn-lg font-weight-medium auth-form-btn" OnClick="btnExport_Click" />



                </div>
                <!--Popup starts-->



            </div>
    </form>
</body>
</html>
