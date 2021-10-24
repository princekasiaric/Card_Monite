namespace CardMon.Core.DTOs.ThirdPartAPIDto.Request
{
    public class TransactionsRequestDto
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public long transactionId { get; set; }
        public string accountNumber { get; set; }
        public double amount { get; set; }
    }
}
