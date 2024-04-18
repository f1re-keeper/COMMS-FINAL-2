namespace COMMSFinalProject.Models
{
    public class Loan
    {
        public string LoanType { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
        public int LoanPeriod { get; set; }
        public string Status { get; set; }
        //public User User { get; set; }
        public int Id { get; set; }
        public int UserId { get; set; }

        public Loan()
        {
            Status = "Processing";
        }
        public void clearLoan()
        {
            LoanType = "";
            Amount = 0;
            Currency = "";
            LoanPeriod = 0;
            Status = "";
        }
    }
}
