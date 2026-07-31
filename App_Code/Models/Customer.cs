using System;

/// <summary>
/// Customer entity representing a lead/customer record
/// </summary>
public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZIP { get; set; }
    public string HomePhone { get; set; }
    public string MobilePhone { get; set; }
    public string EmailAddress { get; set; }
    public string Branch { get; set; }
    public string HDischarge { get; set; }
    public string Ages { get; set; }
    public string Notes { get; set; }
    public DateTime? AppointmentSet { get; set; }
    public string Agent { get; set; }
    public string ApptSetter { get; set; }
    public string Status { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? LDate { get; set; }
    public DateTime? CDate { get; set; }

    public string FullName 
    { 
        get { return string.Format("{0} {1}", FirstName, LastName); }
    }

    public string FullAddress 
    { 
        get { return string.Format("{0} {1} {2} {3}", Address, City, State, ZIP).Trim(); }
    }
}
