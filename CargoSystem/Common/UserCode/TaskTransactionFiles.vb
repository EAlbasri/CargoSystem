
Namespace LightSwitchApplication

    Public Class TaskTransactionFiles

        Private Sub Amount_Validate(results As EntityValidationResultsBuilder)
            ' results.AddPropertyError("<Error-Message>")

            If Not IsNothing(Task) Then
                If Not IsNothing(Amount) Then
                    If Task.Balance < 0 Or Amount = 0 Then
                        results.AddPropertyError("Wrong Amount")
                    End If
                End If
            End If
        End Sub
    End Class

End Namespace
