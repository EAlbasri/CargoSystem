
Namespace LightSwitchApplication

    Public Class ExpenseTransactionFiles

        Private Sub Amount_Validate(results As EntityValidationResultsBuilder)
            ' results.AddPropertyError("<Error-Message>")
            If Not IsNothing(Expense) Then
                If Not IsNothing(Amount) Then
                    If Expense.Balance < 0 Or Amount = 0 Then
                        results.AddPropertyError("Wrong Amount")
                    End If
                End If
            End If
        End Sub
    End Class

End Namespace
