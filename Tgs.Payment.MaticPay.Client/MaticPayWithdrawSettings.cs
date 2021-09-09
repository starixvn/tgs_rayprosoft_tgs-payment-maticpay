namespace Tgs.Payment.MaticPay
{
    public record MaticPayWithdrawSettings(
        string MerchantNo,
        string ApiKey,
        string OrderNo,
        string AccountName,
        string BankCode,
        string BankBranchName,
        string CardNo);
}