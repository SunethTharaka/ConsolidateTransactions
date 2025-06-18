using FluentAssertions;

namespace Transactions.Test
{
    public class UnitTest1
    {
        [Fact]
        public void ConsolidateTransactions_ShouldReturnValidTransactions_TransactionsWithHistoricalRecords()
        {
            // Arrange
            var input = new List<Transaction>
        {
            new("Account1", new DateTime(2024, 1, 1), 100),
            new("Account2", new DateTime(2024, 1, 1), 200),
            new("Account3", new DateTime(2024, 1, 2), 300),
            new("Account4", new DateTime(2024, 1, 2), 100),
            new("Account5", new DateTime(2024, 1, 3), 50),
            new("OtherAccount", new DateTime(2024, 1, 1), 999)
        };
            var consolidator = new TransactionConsolidator();

            // Act
            var result = consolidator.ConsolidateTransactions(input);

            // Assert
            result.Should().HaveCount(4);

            // Verify consolidated entries
            result.Should().ContainEquivalentOf(new Transaction("ConsolidatedAccount", new DateTime(2024, 1, 1), 300));
            result.Should().ContainEquivalentOf(new Transaction("ConsolidatedAccount", new DateTime(2024, 1, 2), 400));
            result.Should().ContainEquivalentOf(new Transaction("ConsolidatedAccount", new DateTime(2024, 1, 3), 50));

            // Verify retained original
            result.Should().ContainEquivalentOf(new Transaction("OtherAccount", new DateTime(2024, 1, 1), 999));
        }

        [Fact]
        public void ConsolidateTransactions_ThrowException_InvalidTransactionsList()
        {
            // Arrange
            List<Transaction> input = null;
            var consolidator = new TransactionConsolidator();

            using var errorOutput = new StringWriter();
            Console.SetError(errorOutput);

            // Act
            var result = consolidator.ConsolidateTransactions(input);

            // Assert
            result.Should().BeEmpty("null input should be caught and return an empty list");

            var errorMessage = errorOutput.ToString();
            errorMessage.Should().Contain("Error consolidating transactions: Value cannot be null. (Parameter 'source')");
        }
    }
}