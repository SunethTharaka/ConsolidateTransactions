public record Transaction(string Account, DateTime Date, decimal Amount);

public class TransactionConsolidator
{
    private void Main()
    {
        var transactions = new List<Transaction>
            {
                new("Account1", new DateTime(2024, 1, 10), 100),
                new("Account2", new DateTime(2024, 1, 10), 200),
                new("Account3", new DateTime(2024, 2, 15), 150),
                new("Account1", new DateTime(2024, 1, 10), 50),
                new("Account4", new DateTime(2024, 3, 5), 300),
                new("Account5", new DateTime(2024, 3, 5), 100),
                new("OtherAccount", new DateTime(2024, 4, 1), 500)
            };

        var newTransactions = ConsolidateTransactions(transactions);
    }

    public List<Transaction> ConsolidateTransactions(List<Transaction> transactions)
    {
        var result = new List<Transaction>();
        try
        {
            var historicalAccounts = new List<string>
        {
          "Account1", "Account2", "Account3", "Account4", "Account5"
        };

            // Task 1 and 2:
            var consolidatedTransactions = transactions.Where(x => historicalAccounts.Contains(x.Account))
                .GroupBy(x => x.Date)
                .Select(g => new Transaction("ConsolidatedAccount", g.Key, g.Sum(x => x.Amount)))
                .ToList();

            //Task 3
            result = transactions.Where(x => !historicalAccounts.Contains(x.Account)).ToList();

            //Task 4
            result.AddRange(consolidatedTransactions);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error consolidating transactions: {ex.Message}");
        }

        return result;
    }
}