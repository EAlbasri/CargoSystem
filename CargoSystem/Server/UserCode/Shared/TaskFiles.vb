
Namespace LightSwitchApplication

    Public Class TaskFiles

        Private Sub Discount_Validate(results As EntityValidationResultsBuilder)
            ' results.AddPropertyError("<Error-Message>")
            If Discount > Total Or Discount < 0 Then
                results.AddPropertyError("Wrong discount value")
            End If
        End Sub

        Private Sub Total_Compute(ByRef result As Decimal)
            ' Set result to the desired field value
            result = (UnitPrice * Quantity) - Discount
        End Sub

        Private Sub Balance_Compute(ByRef result As Decimal)
            ' Set result to the desired field value
            result = Total - TaskTransactionsCollection.Sum(Function(g) g.Amount)
        End Sub

        Private Sub Quantity_Validate(results As EntityValidationResultsBuilder)
            ' results.AddPropertyError("<Error-Message>")
            If Quantity < 1 Then
                results.AddPropertyError("Quantit must be > 1")
            End If
        End Sub
    End Class

End Namespace
