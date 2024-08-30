namespace RYT.Models.ViewModels;

public class WithdrawVM
{
    public IEnumerable<Bank> Banks { get; set; } = new List<Bank>();
}