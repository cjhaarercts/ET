using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

/// <summary>
/// Data access layer for Customer records
/// Separates SQL queries from UI code for better maintainability and testing
/// </summary>
public class CustomerRepository
{
    private readonly string _connectionString;

    public CustomerRepository()
    {
        _connectionString = ConfigurationManager.ConnectionStrings["salespipeline"].ConnectionString;
    }

    /// <summary>
    /// Updates a customer record in the database
    /// </summary>
    public void Update(Customer customer)
    {
        using (SqlConnection con = new SqlConnection(_connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(
                @"UPDATE Customers SET 
                    FirstName=@FirstName, 
                    LastName=@LastName, 
                    Address=@Address, 
                    City=@City, 
                    State=@State, 
                    ZIP=@ZIP, 
                    Status=@Status, 
                    HomePhone=@HomePhone, 
                    MobilePhone=@MobilePhone, 
                    EmailAddress=@EmailAddress, 
                    Branch=@Branch, 
                    HDischarge=@HDischarge, 
                    Ages=@Ages, 
                    Notes=@Notes, 
                    AppointmentSet=@AppointmentSet, 
                    Agent=@Agent, 
                    ApptSetter=@ApptSetter, 
                    LDate=@LDate, 
                    CDate=@CDate 
                WHERE Id=@Id", con))
            {
                AddCustomerParameters(cmd, customer);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    /// <summary>
    /// Deletes a customer record by ID
    /// </summary>
    public void Delete(int id)
    {
        using (SqlConnection con = new SqlConnection(_connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("DELETE FROM Customers WHERE Id = @Id", con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    private void AddCustomerParameters(SqlCommand cmd, Customer customer)
    {
        cmd.Parameters.AddWithValue("@FirstName", customer.FirstName ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@LastName", customer.LastName ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Address", customer.Address ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@City", customer.City ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@State", customer.State ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@ZIP", customer.ZIP ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@HomePhone", customer.HomePhone ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@MobilePhone", customer.MobilePhone ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@EmailAddress", customer.EmailAddress ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Branch", customer.Branch ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@HDischarge", customer.HDischarge ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Ages", customer.Ages ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Notes", customer.Notes ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Agent", customer.Agent ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@ApptSetter", customer.ApptSetter ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@AppointmentSet", customer.AppointmentSet ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Status", customer.Status ?? (object)DBNull.Value);

        // Business logic: set LDate when status is "Left Message"
        cmd.Parameters.AddWithValue("@LDate", 
            customer.Status == "Left Message" ? (object)DateTime.Today : DBNull.Value);

        // Business logic: set CDate when status contains "Call Back"
        cmd.Parameters.AddWithValue("@CDate", 
            customer.Status?.Contains("Call Back") == true ? (object)DateTime.Today : DBNull.Value);

        cmd.Parameters.AddWithValue("@Id", customer.Id);
    }
}
