
Namespace LightSwitchApplication

    Public Class CustomersMgm
        Private TasksDialogHelper As DialogHelper
        Private InvoiceDialogHelper As DialogHelper
        Private InvoicePaymentDialogHelper As DialogHelper

        Private EditFlag As Boolean = False

        Private Sub CustomersMgm_InitializeDataWorkspace(saveChangesTo As List(Of IDataService))
            TasksDialogHelper = New DialogHelper(Me.UnInvoicedTasks, "TaskAddEdit")
            InvoiceDialogHelper = New DialogHelper(Me.InvoiceTasksCollection, "AddEditInvoice")
            InvoicePaymentDialogHelper = New DialogHelper(Me.InvoiceTaskTransactionsCollection, "AddInvoicePayment")

            AddHandler Me.FindControl(TasksGrid_CONTROL).ControlAvailable, AddressOf TasksGrid_ControlAvailable
            AddHandler Me.FindControl(Tab_CONTROL).ControlAvailable, AddressOf TabItems_ControlAvailable
        End Sub

        Private Sub CustomersMgm_Created()
            TasksDialogHelper.InitializeUI()
            InvoiceDialogHelper.InitializeUI()
            InvoicePaymentDialogHelper.InitializeUI()
        End Sub

#Region "Task Add & Edit"

        Private Sub UnInvoicedTasksAddAndEditNew_CanExecute(ByRef result As Boolean)
            ' Write your code here.
            If Not IsNothing(CustomerFilesSet.SelectedItem) Then
                result = TasksDialogHelper.CanAdd()
            Else
                result = False
            End If
        End Sub

        Private Sub UnInvoicedTasksAddAndEditNew_Execute()
            ' Write your code here.
            EditFlag = False
            TasksDialogHelper.AddEntity()
            UnInvoicedTasks.SelectedItem.Customer = CustomerFilesSet.SelectedItem
            UnInvoicedTasks.SelectedItem.Quantity = 1
            ServicesPrp = Nothing
            'UnInvoicedTasks.SelectedItem.Service = Nothing
        End Sub

        Private Sub ServicesPrp_Changed()
            If EditFlag = False And Not IsNothing(ServicesPrp) Then
                UnInvoicedTasks.SelectedItem.Service = ServicesPrp
                UnInvoicedTasks.SelectedItem.UnitPrice = ServicesPrp.DefaultFees
                UnInvoicedTasks.SelectedItem.Note = ServicesPrp.DefaultNote
            End If
        End Sub

        Private Sub UnInvoicedTasksEditSelected_CanExecute(ByRef result As Boolean)
            ' Write your code here.
            If Not IsNothing(CustomerFilesSet.SelectedItem) Then
                result = TasksDialogHelper.CanEditSelected
            Else
                result = False
            End If
        End Sub

        Private Sub UnInvoicedTasksEditSelected_Execute()
            ' Write your code here.
            EditFlag = True
            TasksDialogHelper.EditSelectedEntity()
            ServicesPrp = UnInvoicedTasks.SelectedItem.Service
            EditFlag = False
        End Sub

        Private Sub TaskOk_CanExecute(ByRef result As Boolean)
            ' Write your code here.
            If Not IsNothing(UnInvoicedTasks.SelectedItem) Then
                If UnInvoicedTasks.SelectedItem.Details.ValidationResults.HasErrors = False Then
                    result = True
                Else
                    result = False
                End If
            End If
        End Sub

        Private Sub TaskOk_Execute()
            TasksDialogHelper.DialogOk()
            'Save()
        End Sub

        Private Sub TaskCancel_Execute()
            ' Write your code here.
            TasksDialogHelper.DialogCancel()
        End Sub

#End Region

#Region "Invoices Grid & Creation"
        'this has to match the actual name of the grid control 
        '(by default it gets called "grid") 
        Private Const TasksGrid_CONTROL As String = "UnInvoicedTasks"

        'this is somewhere to store a reference to the grid control 
        Private WithEvents _TasksGridControl As DataGrid = Nothing

        Private Sub CreateInvoice_Execute()
            ' Write your code here.
            Dim NewInvoice As New TaskInvoiceFile
            NewInvoice.InvoiceDate = Today.Date
            NewInvoice.Customer = CustomerFilesSet.SelectedItem
            For Each task As TaskFiles In _TasksGridControl.SelectedItems
                NewInvoice.TasksCollection.Add(task)
            Next
            Save()
            Refresh()
        End Sub

        Private Sub TasksGrid_ControlAvailable(send As Object, e As ControlAvailableEventArgs)
            'we know that the control is a grid, but we use TryCast, just in case 
            _TasksGridControl = TryCast(e.Control, DataGrid)

            'if the cast failed, just leave, there's nothing more we can do here 
            If (_TasksGridControl Is Nothing) Then Return

            'set the property on the grid that allows multiple selection 
            _TasksGridControl.SelectionMode = DataGridSelectionMode.Extended
        End Sub

#End Region

#Region "Invoice Create,Edit & Delete"

        Private Sub InvoiceCancel_Execute()
            ' Write your code here.
            InvoiceDialogHelper.DialogCancel()
        End Sub

        Private Sub InvoiceOk_Execute()
            ' Write your code here.
            InvoiceDialogHelper.DialogCancel()
        End Sub


        Private Sub CreateInvoice_CanExecute(ByRef result As Boolean)
            ' Write your code here.
            If Not IsNothing(CustomerFilesSet.SelectedItem) Then
                If Not IsNothing(UnInvoicedTasks.SelectedItem) Then
                    result = True
                Else
                    result = False
                End If
            Else
                result = False
            End If
        End Sub

        Private Sub EditInvoice_CanExecute(ByRef result As Boolean)
            ' Write your code here.
            If Not IsNothing(CustomerFilesSet) And Not IsNothing(InvoicesCollection) Then
                If Not IsNothing(CustomerFilesSet.SelectedItem) And Not IsNothing(InvoicesCollection.SelectedItem) Then
                    result = InvoiceDialogHelper.CanEditSelected
                Else
                    result = False
                End If
            Else
                result = False
            End If
        End Sub

        Private Sub EditInvoice_Execute()
            ' Write your code here.
            InvoiceDialogHelper.EditSelectedEntity()
        End Sub

        Private Sub DeleteInvoice_CanExecute(ByRef result As Boolean)
            ' Write your code here.
            If Not IsNothing(CustomerFilesSet) And Not IsNothing(InvoicesCollection) Then
                If Not IsNothing(CustomerFilesSet.SelectedItem) And Not IsNothing(InvoicesCollection.SelectedItem) Then
                    result = InvoiceDialogHelper.CanEditSelected
                Else
                    result = False
                End If
            Else
                result = False
            End If
        End Sub

        Private Sub DeleteInvoice_Execute()
            ' Write your code here.
            InvoicesCollection.SelectedItem.Delete()
            Save()
            Refresh()
        End Sub

#End Region

#Region "Invoice Payment"

        Private Sub InvoicePayment_CanExecute(ByRef result As Boolean)
            ' Write your code here.
            If Not IsNothing(InvoicesCollection) Then
                If Not IsNothing(InvoicesCollection.SelectedItem) Then
                    If InvoicesCollection.SelectedItem.InvoiceBalance > 0 Then
                        result = InvoicePaymentDialogHelper.CanAdd
                    Else
                        result = False
                    End If
                Else
                    result = False
                End If
            Else
                result = False
            End If
        End Sub

        Private Sub InvoicePayment_Execute()
            ' Write your code here.
            InvoicePaymentDialogHelper.AddEntity()
            InvoiceTaskTransactionsCollection.SelectedItem.PaymentType = 1
            AmountPrp = 0
        End Sub

        Private Sub InvoicePaymentCancel_Execute()
            ' Write your code here.
            InvoicePaymentDialogHelper.DialogCancel()
        End Sub

        Private Sub AmountPrp_Validate(results As ScreenValidationResultsBuilder)
            If Not IsNothing(InvoicesCollection) Then
                If Not IsNothing(InvoicesCollection.SelectedItem) Then
                    If InvoicesCollection.SelectedItem.InvoiceBalance < 0 Or AmountPrp = 0 Then
                        'results.AddPropertyError("Wrong Amount")
                    End If
                End If
            End If
        End Sub

        Private Sub InvoicePaymentOk_CanExecute(ByRef result As Boolean)
            ' Write your code here.
            If Not IsNothing(InvoicesCollection) Then
                If Not IsNothing(InvoicesCollection.SelectedItem) Then
                    If Not IsNothing(InvoiceTaskTransactionsCollection) Then
                        If Not IsNothing(InvoiceTaskTransactionsCollection.SelectedItem) Then
                            If AmountPrp <= InvoicesCollection.SelectedItem.InvoiceBalance And AmountPrp > 0 And Not IsNothing(InvoiceTaskTransactionsCollection.SelectedItem.PaymentType) Then
                                result = True
                            Else
                                result = False
                            End If
                        Else
                            result = False
                        End If
                    Else
                        result = False
                    End If

                Else
                    result = False
                End If
            Else
                result = False
            End If

        End Sub

        Private Sub InvoicePaymentOk_Execute()
            ' Write your code here.
            Dim AmountBalance As Double = AmountPrp
            For Each tsk As TaskFiles In InvoiceTasksCollection
                'Dim TransactionProperty As New TransactionFiles()
                If tsk.Balance > 0 Then
                    If tsk.Balance < AmountBalance Then
                        Dim tr As New TaskTransactionFiles
                        tr.Amount = tsk.Balance
                        tr.TransactionDate = Now.Date
                        tr.Task = tsk
                        tr.PaymentType = InvoiceTaskTransactionsCollection.SelectedItem.PaymentType
                        tr.Account = AccountsSet.FirstOrDefault

                        AmountBalance -= tr.Amount
                    ElseIf tsk.Balance >= AmountBalance Then
                        Dim tr As New TaskTransactionFiles
                        tr.Amount = AmountBalance
                        tr.TransactionDate = Now
                        tr.Task = tsk
                        tr.PaymentType = InvoiceTaskTransactionsCollection.SelectedItem.PaymentType
                        tr.Account = AccountsSet.FirstOrDefault

                        AmountBalance = 0
                        Exit For
                    End If
                End If
            Next

            InvoicePaymentDialogHelper.DialogCancel()
            Save()
        End Sub

#End Region

#Region "Tab Control"
        'TabControl 
        Private Const Tab_CONTROL As String = "TasksTabControl"
        'this is somewhere to store a reference to the grid control 
        Private WithEvents _TabControl As TabControl = Nothing

        Private Sub TabItems_ControlAvailable(send As Object, e As ControlAvailableEventArgs)
            'we know that the control is a grid, but we use TryCast, just in case 
            _TabControl = TryCast(e.Control, TabControl)
            _TabControl.SelectedIndex = 1
            'if the cast failed, just leave, there's nothing more we can do here 
            If (_TabControl Is Nothing) Then Return
        End Sub

        Private Sub _TabControl_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles _TabControl.SelectionChanged
            If _TabControl.SelectedIndex = 0 Then
                Me.FindControl("CreateInvoice").IsVisible = True
            Else
                Me.FindControl("CreateInvoice").IsVisible = False
            End If

            If _TabControl.SelectedIndex = 1 Then
                Me.FindControl("EditInvoice").IsVisible = True
                Me.FindControl("DeleteInvoice").IsVisible = True
                Me.FindControl("InvoicePayment").IsVisible = True

            Else
                Me.FindControl("EditInvoice").IsVisible = False
                Me.FindControl("DeleteInvoice").IsVisible = False
                Me.FindControl("InvoicePayment").IsVisible = False
            End If
        End Sub

#End Region

    End Class

End Namespace
