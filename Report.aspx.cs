using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Viman
{
    public partial class Report : System.Web.UI.Page

    {
       
      
        int size = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
           
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Id,BookingId,SiteCode,FullName,Email,PhoneNo,DepDate,RetDate,FinalTotalAmt,FromCity,ToCity,Timestamp  FROM FlightsBooking where Status='new'   order by id desc"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            ViewState["Data"] = dt;
                            GridView1.DataSource = dt;
                            GridView1.DataBind();
                        }
                    }
                }
            }
        }



        protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {


            GridView1.PageIndex = e.NewPageIndex;
            BindGrid();

        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            BindGridbookingid();
            Timer1.Enabled = false;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {

            BindGrid();
            txt_bookingid.Text = "";
            txtfdate.Text = "";
            txtscdate.Text = "";
            Timer1.Enabled = true;
        }



        private void BindGridbookingid()
        {

            string bookingid = txt_bookingid.Text;
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Id,BookingId,SiteCode,FullName,Email,PhoneNo,DepDate,RetDate,FinalTotalAmt,FromCity,ToCity,Timestamp  FROM FlightsBooking   where  BookingId='" + bookingid + "' and Status='new'  order by id desc"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);

                            GridView1.DataSource = dt;
                            GridView1.DataBind();
                        }
                    }
                }
            }
        }



        protected void btn_fdate_Click(object sender, EventArgs e)
        {

            string bookingid = txt_bookingid.Text;

            if (bookingid == null || bookingid == "")
            {
                BindGridwithdate();
            }


            else
            {
                BindGridwithdatebookingid();
            }

            Timer1.Enabled = false;

        }



        private void BindGridwithdate()
        {
            DateTime fromDate = DateTime.ParseExact(txtfdate.Text, "dd-MM-yyyy", null);
            DateTime toDate = DateTime.ParseExact(txtscdate.Text, "dd-MM-yyyy", null);

            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(@"
            SELECT Id, BookingId,SiteCode, FullName, Email, PhoneNo, DepDate, RetDate,
                   FinalTotalAmt, FromCity, ToCity, Timestamp
            FROM FlightsBooking
            WHERE Timestamp BETWEEN @fromDate AND @toDateEnd
              AND Status = 'new'
            ORDER BY Id DESC", con))
                {
                    cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                    cmd.Parameters.Add("@toDateEnd", SqlDbType.DateTime).Value = toDate.AddDays(1).AddTicks(-1);

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }
                }
            }
        }


        private void BindGridwithdatebookingid()
        {
            DateTime fromDate = DateTime.ParseExact(txtfdate.Text, "dd-MM-yyyy", null);
            DateTime toDate = DateTime.ParseExact(txtscdate.Text, "dd-MM-yyyy", null);

            string bookingid = txt_bookingid.Text.Trim();
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(@"
            SELECT Id, BookingId,SiteCode, FullName, Email, PhoneNo, DepDate, RetDate,
                   FinalTotalAmt, FromCity, ToCity, Timestamp
            FROM FlightsBooking
            WHERE BookingId = @bookingid
              AND Timestamp BETWEEN @fromDate AND @toDateEnd
              AND Status = 'new'
            ORDER BY Id DESC", con))
                {
                    cmd.Parameters.Add("@bookingid", SqlDbType.VarChar).Value = bookingid;
                    cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = fromDate;
                    cmd.Parameters.Add("@toDateEnd", SqlDbType.DateTime)
                        .Value = toDate.AddDays(1).AddTicks(-1);

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        sda.Fill(dt);

                        GridView1.DataSource = dt;
                        GridView1.DataBind();
                    }
                }
            }
        }


        protected void btnExport_Click(object sender, EventArgs e)
        {

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-excel";
            using (StringWriter sw = new StringWriter())
            {
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                //To Export all pages
                GridView1.AllowPaging = false;

                BindGrid();



                GridView1.HeaderRow.BackColor = Color.White;
                foreach (TableCell cell in GridView1.HeaderRow.Cells)
                {
                    cell.BackColor = GridView1.HeaderStyle.BackColor;
                }
                foreach (GridViewRow row in GridView1.Rows)
                {
                    row.BackColor = Color.White;
                    foreach (TableCell cell in row.Cells)
                    {
                        if (row.RowIndex % 2 == 0)
                        {
                            cell.BackColor = GridView1.AlternatingRowStyle.BackColor;
                        }
                        else
                        {
                            cell.BackColor = GridView1.RowStyle.BackColor;
                        }
                        cell.CssClass = "textmode";
                    }
                }

                GridView1.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();
            }
        }




        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Verifies that the control is rendered */
        }



        protected void Timer1_Tick(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {


            if (DropDownList1.SelectedItem.Text != "0")
            {
                size = int.Parse(DropDownList1.SelectedItem.Value.ToString());
                GridView1.PageSize = size;
                BindGrid();




            }


        }

        protected void ddlwebsite_SelectedIndexChanged(object sender, EventArgs e)
        {
            string website = ddlwebsite.SelectedItem.Value;
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                          
               
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("SELECT Id,BookingId,SiteCode,FullName,Email,PhoneNo,DepDate,RetDate,FinalTotalAmt,FromCity,ToCity,Timestamp  FROM FlightsBooking   where  SiteCode='" + website + "' and Status='new'  order by id desc"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                sda.Fill(dt);

                                GridView1.DataSource = dt;
                                GridView1.DataBind();
                            }
                        }
                    }
                }


            

            


        }
    }
}