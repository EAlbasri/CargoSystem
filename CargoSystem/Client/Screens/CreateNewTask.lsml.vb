
Namespace LightSwitchApplication

    Public Class CreateNewTask

        Private Sub CreateNewTask_InitializeDataWorkspace(ByVal saveChangesTo As Global.System.Collections.Generic.List(Of Global.Microsoft.LightSwitch.IDataService))
            ' Write your code here.
            Me.TaskFilesProperty = New TaskFiles()
            Me.TaskFilesProperty.Quantity = 1
        End Sub

        Private Sub CreateNewTask_Saved()
            ' Write your code here.
            Dim CurrentCust = Me.TaskFilesProperty.Customer
            Dim currentDriver = Me.TaskFilesProperty.Driver

            Me.TaskFilesProperty = New TaskFiles()
            Me.TaskFilesProperty.Customer = CurrentCust
            Me.TaskFilesProperty.Service = ServicePrp
            Me.TaskFilesProperty.Quantity = 1
            Me.TaskFilesProperty.UnitPrice = ServicePrp.DefaultFees
            Me.TaskFilesProperty.Note = ServicePrp.DefaultNote
            Me.TaskFilesProperty.Driver = currentDriver
            'Me.Close(False)
            Application.Current.ShowDefaultScreen(Me.TaskFilesProperty)
        End Sub

        Private Sub ServicePrp_Changed()
            If Not IsNothing(ServicePrp) Then
                Me.TaskFilesProperty.Service = ServicePrp
                Me.TaskFilesProperty.UnitPrice = ServicePrp.DefaultFees
                Me.TaskFilesProperty.Note = ServicePrp.DefaultNote
            End If
        End Sub
    End Class

End Namespace