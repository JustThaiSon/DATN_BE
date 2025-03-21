namespace DATN_Models.DTOS.Employee.Res
{
    public class EmployeeRes
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public DateTime Dob { get; set; }
        public int Sex { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
