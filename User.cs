public class Employee
{
    public int Id { get; set; }
    public string? fName { get; set; }
    //? not to be nullable
    public string? lName { get; set; }

    public bool IsManager { get; set; }
    public string? title { get; set; }
    public string? email { get; set; }
}