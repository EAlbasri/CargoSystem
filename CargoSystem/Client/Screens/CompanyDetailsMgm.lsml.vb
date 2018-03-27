
Namespace LightSwitchApplication

    Public Class CompanyDetailsMgm

        Private Sub CompanyDetailsMgm_InitializeDataWorkspace(saveChangesTo As List(Of Microsoft.LightSwitch.IDataService))
            ' Write your code here.
            ' Me.CompanyDetailsProperty = New CompanyDetails()

            Dim MyComp = CompanyDetailsSet.FirstOrDefault

            If IsNothing(MyComp) Then
                Me.CompanyDetailsProperty = New CompanyDetails()
            Else
                Me.CompanyDetailsProperty = MyComp
            End If

            Dim MyAcc = AccountsSet.FirstOrDefault

            If IsNothing(MyAcc) Then
                Me.AccountsProperty = New Accounts
            End If

        End Sub

        Private Sub CompanyDetailsMgm_Saved()
            ' Write your code here.
            'Me.Close(False)
            'Application.Current.ShowDefaultScreen(Me.CompanyDetailsProperty)
        End Sub

    End Class

End Namespace