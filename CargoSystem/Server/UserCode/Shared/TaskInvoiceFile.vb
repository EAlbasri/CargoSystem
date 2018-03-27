
Namespace LightSwitchApplication

    Public Class TaskInvoiceFile

        Private Sub InvoiceTotal_Compute(ByRef result As Decimal)
            ' Set result to the desired field value
            result = TasksCollection.Sum(Function(g) g.Total)
        End Sub

        Private Sub InvoiceBalance_Compute(ByRef result As Decimal)
            ' Set result to the desired field value
            result = TasksCollection.Sum(Function(g) g.Balance)
        End Sub
    End Class

End Namespace
