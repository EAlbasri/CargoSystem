
Namespace LightSwitchApplication

    Public Class ApplicationDataService

        Private Sub UnInvoicedTasks_PreprocessQuery(ByRef query As System.Linq.IQueryable(Of LightSwitchApplication.TaskFiles))
            query = From Tasks As TaskFiles In query Where Tasks.Invoice Is Nothing
        End Sub

    End Class

End Namespace
