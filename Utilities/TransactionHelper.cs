namespace RYT.Utilities;

public static class TransactionHelper
{
    public static string GenerateTransRef()
    {
        return "RYT-" + Guid.NewGuid();
    }
}