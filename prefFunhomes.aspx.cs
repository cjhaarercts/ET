using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class tvp_Customer : Page
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["salespipeline"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GridView1.DataBind();
        }
    }

    protected void OnRowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow)
            return;

        e.Row.Attributes.Add("OnMouseOver", "this.style.backgroundColor = '#ffff00';");
        e.Row.Attributes.Add("OnMouseOut", "this.style.backgroundColor = '" + ((e.Row.RowIndex % 2 == 0) ? "#FFFFFF" : "#EFF3FB") + "';");
    }

    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        var btn = sender as ImageButton;
        var row = (GridViewRow)btn.NamingContainer;
        lblID.Text = GridView1.DataKeys[row.RowIndex].Value.ToString();

        txtName.Text = row.Cells[3].Text;
        txtadd.Text = row.Cells[4].Text;
        txtcity.Text = row.Cells[5].Text;
        txtstate.Text = row.Cells[6].Text;
        txtzip.Text = row.Cells[7].Text;
        txtphone.Text = row.Cells[8].Text;
        txtChapSvc.Text = row.Cells[9].Text;
        txtTradSvc.Text = row.Cells[10].Text;
        txtSameDaySvc.Text = row.Cells[11].Text;
        txtGraveSvc.Text = row.Cells[12].Text;
        txtTradCrem.Text = row.Cells[13].Text;
        txtSameDayCrem.Text = row.Cells[14].Text;
        txtMemCrem.Text = row.Cells[15].Text;
        txtGraveCrem.Text = row.Cells[16].Text;
        txtDirectCrem.Text = row.Cells[17].Text;
        txtLimo.Text = row.Cells[18].Text;
        txtMEscort.Text = row.Cells[19].Text;
        txtAltContainer.Text = row.Cells[20].Text;
        txtClergy.Text = row.Cells[21].Text;
        txtMemPkg.Text = row.Cells[22].Text;
        txtVideo.Text = row.Cells[23].Text;
        txtShroud.Text = row.Cells[24].Text;
        txtShroudMuslin.Text = row.Cells[25].Text;
        txtTahara.Text = row.Cells[26].Text;
        txtShomer.Text = row.Cells[27].Text;
        txtGratuities.Text = row.Cells[28].Text;

        ModalPopupExtender1.Show();
    }

    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        var btn = sender as ImageButton;
        var row = (GridViewRow)btn.NamingContainer;
        int id = Convert.ToInt32(GridView1.DataKeys[row.RowIndex].Value);

        using (var con = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand("DELETE FROM flpreffunhm WHERE Id = @Id", con))
        {
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
            con.Open();
            cmd.ExecuteNonQuery();
        }

        lblresult.Text = "Funeral Home Record Deleted Successfully";
        lblresult.ForeColor = System.Drawing.Color.Green;
        GridView1.DataBind();
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        int id = Convert.ToInt32(lblID.Text);

        using (var con = new SqlConnection(_connectionString))
        using (var cmd = new SqlCommand(@"UPDATE flpreffunhm SET
                                            Name = @Name,
                                            Address = @Address,
                                            City = @City,
                                            State = @State,
                                            Zip = @Zip,
                                            Phone = @Phone,
                                            ChapSvc = @ChapSvc,
                                            TradSvc = @TradSvc,
                                            SameDaySvc = @SameDaySvc,
                                            GraveSvc = @GraveSvc,
                                            TradCrem = @TradCrem,
                                            SameDayCrem = @SameDayCrem,
                                            MemCrem = @MemCrem,
                                            GraveCrem = @GraveCrem,
                                            DirectCrem = @DirectCrem,
                                            Limo = @Limo,
                                            MEscort = @MEscort,
                                            AltContainer = @AltContainer,
                                            Clergy = @Clergy,
                                            MemPkg = @MemPkg,
                                            Video = @Video,
                                            Shroud = @Shroud,
                                            ShroudMuslin = @ShroudMuslin,
                                            Tahara = @Tahara,
                                            Shomer = @Shomer,
                                            Gratuities = @Gratuities
                                         WHERE Id = @Id", con))
        {
            cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@Address", txtadd.Text.Trim());
            cmd.Parameters.AddWithValue("@City", txtcity.Text.Trim());
            cmd.Parameters.AddWithValue("@State", txtstate.Text.Trim());
            cmd.Parameters.AddWithValue("@Zip", txtzip.Text.Trim());
            cmd.Parameters.AddWithValue("@Phone", txtphone.Text.Trim());
            cmd.Parameters.AddWithValue("@ChapSvc", txtChapSvc.Text.Trim());
            cmd.Parameters.AddWithValue("@TradSvc", txtTradSvc.Text.Trim());
            cmd.Parameters.AddWithValue("@SameDaySvc", txtSameDaySvc.Text.Trim());
            cmd.Parameters.AddWithValue("@GraveSvc", txtGraveSvc.Text.Trim());
            cmd.Parameters.AddWithValue("@TradCrem", txtTradCrem.Text.Trim());
            cmd.Parameters.AddWithValue("@SameDayCrem", txtSameDayCrem.Text.Trim());
            cmd.Parameters.AddWithValue("@MemCrem", txtMemCrem.Text.Trim());
            cmd.Parameters.AddWithValue("@GraveCrem", txtGraveCrem.Text.Trim());
            cmd.Parameters.AddWithValue("@DirectCrem", txtDirectCrem.Text.Trim());
            cmd.Parameters.AddWithValue("@Limo", txtLimo.Text.Trim());
            cmd.Parameters.AddWithValue("@MEscort", txtMEscort.Text.Trim());
            cmd.Parameters.AddWithValue("@AltContainer", txtAltContainer.Text.Trim());
            cmd.Parameters.AddWithValue("@Clergy", txtClergy.Text.Trim());
            cmd.Parameters.AddWithValue("@MemPkg", txtMemPkg.Text.Trim());
            cmd.Parameters.AddWithValue("@Video", txtVideo.Text.Trim());
            cmd.Parameters.AddWithValue("@Shroud", txtShroud.Text.Trim());
            cmd.Parameters.AddWithValue("@ShroudMuslin", txtShroudMuslin.Text.Trim());
            cmd.Parameters.AddWithValue("@Tahara", txtTahara.Text.Trim());
            cmd.Parameters.AddWithValue("@Shomer", txtShomer.Text.Trim());
            cmd.Parameters.AddWithValue("@Gratuities", txtGratuities.Text.Trim());

            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;

            con.Open();
            cmd.ExecuteNonQuery();
        }

        lblresult.Text = "Funeral Home Record Details Updated Successfully";
        lblresult.ForeColor = System.Drawing.Color.Green;
        GridView1.DataBind();
    }

    public void Sendmail_With_IcsAttachment(object sender, EventArgs e)
    {
        // Compose message and ICS using disposables
        string toAgentAlias = "rsstangler1";
        string primaryRecipient = toAgentAlias + "@gmail.com";

        string subject = "{txtName.Text} {txtadd.Text} {txtcity.Text} {txtstate.Text} {txtzip.Text} {txtphone.Text}";
        string body = "You have an Appointment with {txtName.Text} ({txtphone.Text})";

        DateTime dt = DateTime.Today; // using today's date by default

        var sb = new StringBuilder();
        sb.AppendLine("BEGIN:VCALENDAR");
        sb.AppendLine("PRODID:-//Schedule a Meeting");
        sb.AppendLine("VERSION:2.0");
        sb.AppendLine("METHOD:REQUEST");
        sb.AppendLine("BEGIN:VEVENT");
        sb.AppendLine("DTSTART:" + dt.ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"));
        sb.AppendLine("DTSTAMP:" + DateTime.UtcNow.ToString("yyyyMMdd\\THHmmss\\Z"));
        sb.AppendLine("DTEND:" + dt.AddHours(6).ToUniversalTime().ToString("yyyyMMdd\\THHmmss\\Z"));
        sb.AppendLine("LOCATION: " + txtadd.Text + " " + txtcity.Text + " " + txtstate.Text + " " + txtzip.Text);
        sb.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
        sb.AppendLine(string.Format("DESCRIPTION:{0}", body));
        sb.AppendLine(string.Format("X-ALT-DESC;FMTTYPE=text/html:{0}", body));
        sb.AppendLine(string.Format("SUMMARY:{0}", subject));
        sb.AppendLine(string.Format("ORGANIZER:MAILTO:{0}", "info@eternalsolutionsllc.com"));
        sb.AppendLine(string.Format("ATTENDEE;CN=\"{0}\";RSVP=TRUE:mailto:{1}", toAgentAlias, primaryRecipient));
        sb.AppendLine("BEGIN:VALARM");
        sb.AppendLine("TRIGGER:-PT15M");
        sb.AppendLine("ACTION:DISPLAY");
        sb.AppendLine("DESCRIPTION:Reminder");
        sb.AppendLine("END:VALARM");
        sb.AppendLine("END:VEVENT");
        sb.AppendLine("END:VCALENDAR");

        using (var msg = new MailMessage())
        {
            msg.From = new MailAddress("info@eternalsolutionsllc.com", "Funeral Home Appointment");
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = false;
            msg.To.Add(new MailAddress(primaryRecipient, toAgentAlias));

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            using (var stream = new MemoryStream(bytes))
            using (var attachment = new Attachment(stream, "appointment.ics", "text/calendar"))
            {
                msg.Attachments.Add(attachment);

                // Use default SmtpClient so web.config mailSettings are honored
                using (var smtpClient = new SmtpClient())
                {
                    try
                    {
                        smtpClient.Send(msg);
                        lblresult.Text = "Email sent successfully.";
                        lblresult.ForeColor = System.Drawing.Color.Green;
                    }
                    catch (Exception ex)
                    {
                        lblresult.Text = "Email error: " + ex.Message;
                        lblresult.ForeColor = System.Drawing.Color.Red;
                    }
                }
            }
        }
    }
}