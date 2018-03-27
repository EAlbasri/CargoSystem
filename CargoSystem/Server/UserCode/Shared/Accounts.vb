
Namespace LightSwitchApplication

    Public Class Accounts

        Private Sub Bank_Compute(ByRef result As Decimal)
            ' Set result to the desired field value
            result = ((TaskTransactionsCollection.Where(Function(g) Not g.PaymentType = 1).Sum(Function(t) t.Amount)) + Company.InitialBankAccount) - (ExpenseTransactionsCollection.Where(Function(g) Not g.PaymentType = 1).Sum(Function(t) t.Amount))
        End Sub

        Private Sub Cash_Compute(ByRef result As Decimal)
            ' Set result to the desired field value
            result = ((TaskTransactionsCollection.Where(Function(g) g.PaymentType = 1).Sum(Function(t) t.Amount)) + Company.InitialCashAccount) - (ExpenseTransactionsCollection.Where(Function(g) g.PaymentType = 1).Sum(Function(t) t.Amount))
        End Sub
    End Class

End Namespace
