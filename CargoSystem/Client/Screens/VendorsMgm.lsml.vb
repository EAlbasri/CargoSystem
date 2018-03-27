
Namespace LightSwitchApplication

    Public Class VendorsMgm
        Private ExpenseDialogHelper As DialogHelper
        Private ExpenseTransactionDialogHelper As DialogHelper

        Dim EditFlag As Boolean = False

        Private Sub VendorsMgm_InitializeDataWorkspace(saveChangesTo As List(Of IDataService))
            ExpenseDialogHelper = New DialogHelper(Me.ExpensesCollection, "AddEditExpense")
            ExpenseTransactionDialogHelper = New DialogHelper(Me.ExpenseTransactionsCollection, "AddEditExpensePayment")
        End Sub

        Private Sub VendorsMgm_Created()
            ExpenseDialogHelper.InitializeUI()
            ExpenseTransactionDialogHelper.InitializeUI()
        End Sub

#Region "Add Edit Expense"

        Private Sub OperationalExpensesCollectionAddAndEditNew_CanExecute(ByRef result As Boolean)
            ' Write your code here.
            If Not IsNothing(VendorsList.SelectedItem) Then
                result = ExpenseDialogHelper.CanAdd()
            Else
                result = False
            End If
        End Sub

        Private Sub OperationalExpensesCollectionAddAndEditNew_Execute()
            ' Write your code here.
            EditFlag = False
            ExpenseDialogHelper.AddEntity()
            ExpensesCollection.SelectedItem.Vendor = VendorsList.SelectedItem
            ExpensesCollection.SelectedItem.Quantity = 1
            ServicePrp = Nothing
            IsService = True
        End Sub

        Private Sub ServicePrp_Changed()
            If EditFlag = False And Not IsNothing(ServicePrp) Then
                ExpensesCollection.SelectedItem.Service = ServicePrp
                ExpensesCollection.SelectedItem.UnitPrice = ServicePrp.DefaultFees
                ExpensesCollection.SelectedItem.Note = ServicePrp.DefaultNote
                ExpensesCollection.SelectedItem.Name = ServicePrp.Name
            End If
        End Sub

        Private Sub OperationalExpensesCollectionEditSelected_CanExecute(ByRef result As Boolean)
            ' Write your code here.
            If Not IsNothing(VendorsList.SelectedItem) Then
                result = ExpenseDialogHelper.CanEditSelected
            Else
                result = False
            End If
        End Sub

        Private Sub OperationalExpensesCollectionEditSelected_Execute()
            ' Write your code here.
            EditFlag = True
            ExpenseDialogHelper.EditSelectedEntity()
            ServicePrp = ExpensesCollection.SelectedItem.Service
            IsService = ExpensesCollection.SelectedItem.IsService
            EditFlag = False
        End Sub

        Private Sub ExpenseOk_CanExecute(ByRef result As Boolean)
            ' Write your code here.
            If Not IsNothing(ExpensesCollection.SelectedItem) Then
                If ExpensesCollection.SelectedItem.Details.ValidationResults.HasErrors = False Then
                    result = True
                Else
                    result = False
                End If
            End If
        End Sub

        Private Sub ExpenseOk_Execute()
            ' Write your code here.
            ExpenseDialogHelper.DialogOk()
            Save()
        End Sub

        Private Sub ExpenseCancel_Execute()
            ' Write your code here.
            ExpenseDialogHelper.DialogCancel()
        End Sub

        Private Sub IsService_Changed()
            If IsService = True Then
                Me.FindControl("ServicePrp").IsVisible = True
                Me.FindControl("Name").IsVisible = False
                ExpensesCollection.SelectedItem.IsService = True
            Else
                Me.FindControl("ServicePrp").IsVisible = False
                Me.FindControl("Name").IsVisible = True
                ExpensesCollection.SelectedItem.IsService = False
            End If
        End Sub

#End Region

#Region "Expense Payment"

        Private Sub ExpensePayment_CanExecute(ByRef result As Boolean)
            ' Write your code here.
            If Not IsNothing(VendorsList) And Not IsNothing(ExpensesCollection) Then
                If Not IsNothing(VendorsList.SelectedItem) Then
                    If Not IsNothing(ExpensesCollection.SelectedItem) Then
                        If ExpensesCollection.SelectedItem.Balance > 0 Then
                            result = ExpensesCollection.CanAddNew
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

        Private Sub ExpenseTransactionCancel_Execute()
            ' Write your code here.
            ExpenseTransactionDialogHelper.DialogCancel()
        End Sub

        Private Sub ExpensePayment_Execute()
            ' Write your code here.
            ExpenseTransactionDialogHelper.AddEntity()
            ExpenseTransactionsCollection.SelectedItem.PaymentType = 1
            ExpenseTransactionsCollection.SelectedItem.Account = AccountsSet.FirstOrDefault
        End Sub

        Private Sub ExpenseTransactionOk_Execute()
            ' Write your code here.
            ExpenseTransactionDialogHelper.DialogOk()
            Save()
        End Sub

        Private Sub ExpenseTransactionOk_CanExecute(ByRef result As Boolean)
            ' Write your code here.
            If Not IsNothing(ExpensesCollection) Then
                If Not IsNothing(ExpensesCollection.SelectedItem) Then
                    If Not IsNothing(ExpenseTransactionsCollection) Then
                        If Not IsNothing(ExpenseTransactionsCollection.SelectedItem) Then
                            If ExpensesCollection.SelectedItem.Balance >= 0 And ExpenseTransactionsCollection.SelectedItem.Amount > 0 And Not IsNothing(ExpenseTransactionsCollection.SelectedItem.PaymentType) Then
                                If Not ExpenseTransactionsCollection.SelectedItem.Details.ValidationResults.HasErrors = True Then
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
            Else
                result = False
            End If
        End Sub

#End Region

    End Class

End Namespace
