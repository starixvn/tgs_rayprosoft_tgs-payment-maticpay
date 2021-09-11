namespace Tgs.Payment.MaticPay
{
    public record MaticPayBankInfo(
        string AccountName,
        string BankCode,
        string BankBranchName,
        string CardNo);
}