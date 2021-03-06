﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.296
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.ComponentModel
Imports System.Data
Imports System.Diagnostics
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml.Serialization

'
'This source code was auto-generated by Microsoft.VSDesigner, Version 4.0.30319.296.
'
Namespace CheckVerify
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Web.Services.WebServiceBindingAttribute(Name:="CheckValidatorSoap", [Namespace]:="http://myvalidator.com/validator_dev/")>  _
    Partial Public Class CheckValidator
        Inherits System.Web.Services.Protocols.SoapHttpClientProtocol
        
        Private CheckVerificationOperationCompleted As System.Threading.SendOrPostCallback
        
        Private lookupCompanyOperationCompleted As System.Threading.SendOrPostCallback
        
        Private UpdatePayeeOperationCompleted As System.Threading.SendOrPostCallback
        
        Private UpdateCompanyOperationCompleted As System.Threading.SendOrPostCallback
        
        Private UpdateCheckOperationCompleted As System.Threading.SendOrPostCallback
        
        Private MatrixBusinessRulesOperationCompleted As System.Threading.SendOrPostCallback
        
        Private useDefaultCredentialsSetExplicitly As Boolean
        
        '''<remarks/>
        Public Sub New()
            MyBase.New
            Me.Url = Global.Kiosk.My.MySettings.Default.Kiosk_CheckVerify_CheckValidator
            If (Me.IsLocalFileSystemWebService(Me.Url) = true) Then
                Me.UseDefaultCredentials = true
                Me.useDefaultCredentialsSetExplicitly = false
            Else
                Me.useDefaultCredentialsSetExplicitly = true
            End If
        End Sub
        
        Public Shadows Property Url() As String
            Get
                Return MyBase.Url
            End Get
            Set
                If (((Me.IsLocalFileSystemWebService(MyBase.Url) = true)  _
                            AndAlso (Me.useDefaultCredentialsSetExplicitly = false))  _
                            AndAlso (Me.IsLocalFileSystemWebService(value) = false)) Then
                    MyBase.UseDefaultCredentials = false
                End If
                MyBase.Url = value
            End Set
        End Property
        
        Public Shadows Property UseDefaultCredentials() As Boolean
            Get
                Return MyBase.UseDefaultCredentials
            End Get
            Set
                MyBase.UseDefaultCredentials = value
                Me.useDefaultCredentialsSetExplicitly = true
            End Set
        End Property
        
        '''<remarks/>
        Public Event CheckVerificationCompleted As CheckVerificationCompletedEventHandler
        
        '''<remarks/>
        Public Event lookupCompanyCompleted As lookupCompanyCompletedEventHandler
        
        '''<remarks/>
        Public Event UpdatePayeeCompleted As UpdatePayeeCompletedEventHandler
        
        '''<remarks/>
        Public Event UpdateCompanyCompleted As UpdateCompanyCompletedEventHandler
        
        '''<remarks/>
        Public Event UpdateCheckCompleted As UpdateCheckCompletedEventHandler
        
        '''<remarks/>
        Public Event MatrixBusinessRulesCompleted As MatrixBusinessRulesCompletedEventHandler
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://myvalidator.com/validator_dev/CheckVerification", RequestNamespace:="http://myvalidator.com/validator_dev/", ResponseNamespace:="http://myvalidator.com/validator_dev/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function CheckVerification( _
                    ByVal CheckNumber As String,  _
                    ByVal RoutingNumber As String,  _
                    ByVal AccountNumber As String,  _
                    ByVal PayeeID As String,  _
                    ByVal CheckAmount As String,  _
                    ByVal CheckDate As Date,  _
                    ByVal CompanyName As String,  _
                    ByVal PayeeAuthCode As String,  _
                    ByVal CompanyPhone As String,  _
                    ByVal CompanyID As String,  _
                    ByVal TransactionID As Long,  _
                    ByVal StoreID As String,  _
                    ByVal FirstName As String,  _
                    ByVal LastName As String,  _
                    ByVal BirthDate As String,  _
                    ByVal PayeeZip As String,  _
                    ByRef objBusinessLogic As BusinessLogic) As System.Data.DataSet
            Dim results() As Object = Me.Invoke("CheckVerification", New Object() {CheckNumber, RoutingNumber, AccountNumber, PayeeID, CheckAmount, CheckDate, CompanyName, PayeeAuthCode, CompanyPhone, CompanyID, TransactionID, StoreID, FirstName, LastName, BirthDate, PayeeZip, objBusinessLogic})
            objBusinessLogic = CType(results(1),BusinessLogic)
            Return CType(results(0),System.Data.DataSet)
        End Function
        
        '''<remarks/>
        Public Overloads Sub CheckVerificationAsync( _
                    ByVal CheckNumber As String,  _
                    ByVal RoutingNumber As String,  _
                    ByVal AccountNumber As String,  _
                    ByVal PayeeID As String,  _
                    ByVal CheckAmount As String,  _
                    ByVal CheckDate As Date,  _
                    ByVal CompanyName As String,  _
                    ByVal PayeeAuthCode As String,  _
                    ByVal CompanyPhone As String,  _
                    ByVal CompanyID As String,  _
                    ByVal TransactionID As Long,  _
                    ByVal StoreID As String,  _
                    ByVal FirstName As String,  _
                    ByVal LastName As String,  _
                    ByVal BirthDate As String,  _
                    ByVal PayeeZip As String,  _
                    ByVal objBusinessLogic As BusinessLogic)
            Me.CheckVerificationAsync(CheckNumber, RoutingNumber, AccountNumber, PayeeID, CheckAmount, CheckDate, CompanyName, PayeeAuthCode, CompanyPhone, CompanyID, TransactionID, StoreID, FirstName, LastName, BirthDate, PayeeZip, objBusinessLogic, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub CheckVerificationAsync( _
                    ByVal CheckNumber As String,  _
                    ByVal RoutingNumber As String,  _
                    ByVal AccountNumber As String,  _
                    ByVal PayeeID As String,  _
                    ByVal CheckAmount As String,  _
                    ByVal CheckDate As Date,  _
                    ByVal CompanyName As String,  _
                    ByVal PayeeAuthCode As String,  _
                    ByVal CompanyPhone As String,  _
                    ByVal CompanyID As String,  _
                    ByVal TransactionID As Long,  _
                    ByVal StoreID As String,  _
                    ByVal FirstName As String,  _
                    ByVal LastName As String,  _
                    ByVal BirthDate As String,  _
                    ByVal PayeeZip As String,  _
                    ByVal objBusinessLogic As BusinessLogic,  _
                    ByVal userState As Object)
            If (Me.CheckVerificationOperationCompleted Is Nothing) Then
                Me.CheckVerificationOperationCompleted = AddressOf Me.OnCheckVerificationOperationCompleted
            End If
            Me.InvokeAsync("CheckVerification", New Object() {CheckNumber, RoutingNumber, AccountNumber, PayeeID, CheckAmount, CheckDate, CompanyName, PayeeAuthCode, CompanyPhone, CompanyID, TransactionID, StoreID, FirstName, LastName, BirthDate, PayeeZip, objBusinessLogic}, Me.CheckVerificationOperationCompleted, userState)
        End Sub
        
        Private Sub OnCheckVerificationOperationCompleted(ByVal arg As Object)
            If (Not (Me.CheckVerificationCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent CheckVerificationCompleted(Me, New CheckVerificationCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://myvalidator.com/validator_dev/lookupCompany", RequestNamespace:="http://myvalidator.com/validator_dev/", ResponseNamespace:="http://myvalidator.com/validator_dev/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function lookupCompany(ByVal AccountNumber As String, ByVal RoutingNumber As String) As System.Data.DataSet
            Dim results() As Object = Me.Invoke("lookupCompany", New Object() {AccountNumber, RoutingNumber})
            Return CType(results(0),System.Data.DataSet)
        End Function
        
        '''<remarks/>
        Public Overloads Sub lookupCompanyAsync(ByVal AccountNumber As String, ByVal RoutingNumber As String)
            Me.lookupCompanyAsync(AccountNumber, RoutingNumber, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub lookupCompanyAsync(ByVal AccountNumber As String, ByVal RoutingNumber As String, ByVal userState As Object)
            If (Me.lookupCompanyOperationCompleted Is Nothing) Then
                Me.lookupCompanyOperationCompleted = AddressOf Me.OnlookupCompanyOperationCompleted
            End If
            Me.InvokeAsync("lookupCompany", New Object() {AccountNumber, RoutingNumber}, Me.lookupCompanyOperationCompleted, userState)
        End Sub
        
        Private Sub OnlookupCompanyOperationCompleted(ByVal arg As Object)
            If (Not (Me.lookupCompanyCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent lookupCompanyCompleted(Me, New lookupCompanyCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://myvalidator.com/validator_dev/UpdatePayee", RequestNamespace:="http://myvalidator.com/validator_dev/", ResponseNamespace:="http://myvalidator.com/validator_dev/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function UpdatePayee(ByVal PayeeID As String, ByVal Payee_Status As String, ByVal Velocity_Variance_Threshold As String) As System.Data.DataSet
            Dim results() As Object = Me.Invoke("UpdatePayee", New Object() {PayeeID, Payee_Status, Velocity_Variance_Threshold})
            Return CType(results(0),System.Data.DataSet)
        End Function
        
        '''<remarks/>
        Public Overloads Sub UpdatePayeeAsync(ByVal PayeeID As String, ByVal Payee_Status As String, ByVal Velocity_Variance_Threshold As String)
            Me.UpdatePayeeAsync(PayeeID, Payee_Status, Velocity_Variance_Threshold, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub UpdatePayeeAsync(ByVal PayeeID As String, ByVal Payee_Status As String, ByVal Velocity_Variance_Threshold As String, ByVal userState As Object)
            If (Me.UpdatePayeeOperationCompleted Is Nothing) Then
                Me.UpdatePayeeOperationCompleted = AddressOf Me.OnUpdatePayeeOperationCompleted
            End If
            Me.InvokeAsync("UpdatePayee", New Object() {PayeeID, Payee_Status, Velocity_Variance_Threshold}, Me.UpdatePayeeOperationCompleted, userState)
        End Sub
        
        Private Sub OnUpdatePayeeOperationCompleted(ByVal arg As Object)
            If (Not (Me.UpdatePayeeCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent UpdatePayeeCompleted(Me, New UpdatePayeeCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://myvalidator.com/validator_dev/UpdateCompany", RequestNamespace:="http://myvalidator.com/validator_dev/", ResponseNamespace:="http://myvalidator.com/validator_dev/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function UpdateCompany(ByVal Company_ID As String, ByVal RoutingNumber As String, ByVal Company_Status As String, ByVal Threshold As String, ByVal Velocity As String, ByVal Approve_Date As String, ByVal Company_Phone As String, ByVal Return_Check As String) As System.Data.DataSet
            Dim results() As Object = Me.Invoke("UpdateCompany", New Object() {Company_ID, RoutingNumber, Company_Status, Threshold, Velocity, Approve_Date, Company_Phone, Return_Check})
            Return CType(results(0),System.Data.DataSet)
        End Function
        
        '''<remarks/>
        Public Overloads Sub UpdateCompanyAsync(ByVal Company_ID As String, ByVal RoutingNumber As String, ByVal Company_Status As String, ByVal Threshold As String, ByVal Velocity As String, ByVal Approve_Date As String, ByVal Company_Phone As String, ByVal Return_Check As String)
            Me.UpdateCompanyAsync(Company_ID, RoutingNumber, Company_Status, Threshold, Velocity, Approve_Date, Company_Phone, Return_Check, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub UpdateCompanyAsync(ByVal Company_ID As String, ByVal RoutingNumber As String, ByVal Company_Status As String, ByVal Threshold As String, ByVal Velocity As String, ByVal Approve_Date As String, ByVal Company_Phone As String, ByVal Return_Check As String, ByVal userState As Object)
            If (Me.UpdateCompanyOperationCompleted Is Nothing) Then
                Me.UpdateCompanyOperationCompleted = AddressOf Me.OnUpdateCompanyOperationCompleted
            End If
            Me.InvokeAsync("UpdateCompany", New Object() {Company_ID, RoutingNumber, Company_Status, Threshold, Velocity, Approve_Date, Company_Phone, Return_Check}, Me.UpdateCompanyOperationCompleted, userState)
        End Sub
        
        Private Sub OnUpdateCompanyOperationCompleted(ByVal arg As Object)
            If (Not (Me.UpdateCompanyCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent UpdateCompanyCompleted(Me, New UpdateCompanyCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://myvalidator.com/validator_dev/UpdateCheck", RequestNamespace:="http://myvalidator.com/validator_dev/", ResponseNamespace:="http://myvalidator.com/validator_dev/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function UpdateCheck(ByVal Transaction_ID As String, ByVal Payee_ID As String, ByVal Check_Status As String) As System.Data.DataSet
            Dim results() As Object = Me.Invoke("UpdateCheck", New Object() {Transaction_ID, Payee_ID, Check_Status})
            Return CType(results(0),System.Data.DataSet)
        End Function
        
        '''<remarks/>
        Public Overloads Sub UpdateCheckAsync(ByVal Transaction_ID As String, ByVal Payee_ID As String, ByVal Check_Status As String)
            Me.UpdateCheckAsync(Transaction_ID, Payee_ID, Check_Status, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub UpdateCheckAsync(ByVal Transaction_ID As String, ByVal Payee_ID As String, ByVal Check_Status As String, ByVal userState As Object)
            If (Me.UpdateCheckOperationCompleted Is Nothing) Then
                Me.UpdateCheckOperationCompleted = AddressOf Me.OnUpdateCheckOperationCompleted
            End If
            Me.InvokeAsync("UpdateCheck", New Object() {Transaction_ID, Payee_ID, Check_Status}, Me.UpdateCheckOperationCompleted, userState)
        End Sub
        
        Private Sub OnUpdateCheckOperationCompleted(ByVal arg As Object)
            If (Not (Me.UpdateCheckCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent UpdateCheckCompleted(Me, New UpdateCheckCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        <System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://myvalidator.com/validator_dev/MatrixBusinessRules", RequestNamespace:="http://myvalidator.com/validator_dev/", ResponseNamespace:="http://myvalidator.com/validator_dev/", Use:=System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle:=System.Web.Services.Protocols.SoapParameterStyle.Wrapped)>  _
        Public Function MatrixBusinessRules(ByVal CheckNumber As String, ByVal RoutingNumber As String, ByVal AccountNumber As String, ByVal PayeeID As String, ByVal CheckAmount As String, ByVal CheckDate As Date, ByVal CompanyID As String, ByVal StoreID As String, ByVal payeeZip As String) As BusinessLogic
            Dim results() As Object = Me.Invoke("MatrixBusinessRules", New Object() {CheckNumber, RoutingNumber, AccountNumber, PayeeID, CheckAmount, CheckDate, CompanyID, StoreID, payeeZip})
            Return CType(results(0),BusinessLogic)
        End Function
        
        '''<remarks/>
        Public Overloads Sub MatrixBusinessRulesAsync(ByVal CheckNumber As String, ByVal RoutingNumber As String, ByVal AccountNumber As String, ByVal PayeeID As String, ByVal CheckAmount As String, ByVal CheckDate As Date, ByVal CompanyID As String, ByVal StoreID As String, ByVal payeeZip As String)
            Me.MatrixBusinessRulesAsync(CheckNumber, RoutingNumber, AccountNumber, PayeeID, CheckAmount, CheckDate, CompanyID, StoreID, payeeZip, Nothing)
        End Sub
        
        '''<remarks/>
        Public Overloads Sub MatrixBusinessRulesAsync(ByVal CheckNumber As String, ByVal RoutingNumber As String, ByVal AccountNumber As String, ByVal PayeeID As String, ByVal CheckAmount As String, ByVal CheckDate As Date, ByVal CompanyID As String, ByVal StoreID As String, ByVal payeeZip As String, ByVal userState As Object)
            If (Me.MatrixBusinessRulesOperationCompleted Is Nothing) Then
                Me.MatrixBusinessRulesOperationCompleted = AddressOf Me.OnMatrixBusinessRulesOperationCompleted
            End If
            Me.InvokeAsync("MatrixBusinessRules", New Object() {CheckNumber, RoutingNumber, AccountNumber, PayeeID, CheckAmount, CheckDate, CompanyID, StoreID, payeeZip}, Me.MatrixBusinessRulesOperationCompleted, userState)
        End Sub
        
        Private Sub OnMatrixBusinessRulesOperationCompleted(ByVal arg As Object)
            If (Not (Me.MatrixBusinessRulesCompletedEvent) Is Nothing) Then
                Dim invokeArgs As System.Web.Services.Protocols.InvokeCompletedEventArgs = CType(arg,System.Web.Services.Protocols.InvokeCompletedEventArgs)
                RaiseEvent MatrixBusinessRulesCompleted(Me, New MatrixBusinessRulesCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState))
            End If
        End Sub
        
        '''<remarks/>
        Public Shadows Sub CancelAsync(ByVal userState As Object)
            MyBase.CancelAsync(userState)
        End Sub
        
        Private Function IsLocalFileSystemWebService(ByVal url As String) As Boolean
            If ((url Is Nothing)  _
                        OrElse (url Is String.Empty)) Then
                Return false
            End If
            Dim wsUri As System.Uri = New System.Uri(url)
            If ((wsUri.Port >= 1024) _
                        AndAlso (String.Compare(wsUri.Host, "localhost", System.StringComparison.OrdinalIgnoreCase) = 0)) Then
                Return True
            End If
            Return false
        End Function
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233"),  _
     System.SerializableAttribute(),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code"),  _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://myvalidator.com/validator_dev/")>  _
    Partial Public Class BusinessLogic
        
        Private businessCheckField As Boolean
        
        Private isPayeeZipInRangeField As Boolean
        
        Private registeredCompanyField As Boolean
        
        Private expiredCompanyField As Boolean
        
        Private exceedMaxAmountField As Boolean
        
        Private exceedMaxAgeField As Boolean
        
        Private checkNumUnder300Field As Boolean
        
        Private duplicateCheckField As Boolean
        
        Private companyOnHoldField As Boolean
        
        Private payeeOnHoldField As Boolean
        
        Private underSmallAmountField As Boolean
        
        Private companyFirstCheckField As Boolean
        
        Private exceedFirstCheckMaxAmtField As Boolean
        
        Private newPayeeField As Boolean
        
        Private newPayeeMultiNewField As Boolean
        
        Private firstCheckClearedField As Boolean
        
        Private newCompanyField As Boolean
        
        Private exceedNewCompanyCountField As Boolean
        
        Private reachedVelocityLimitField As Boolean
        
        Private checkNumInRangeField As Boolean
        
        Private companySecondCheckField As Boolean
        
        Private guranteeCodeField As String
        
        Private companyIDField As String
        
        '''<remarks/>
        Public Property BusinessCheck() As Boolean
            Get
                Return Me.businessCheckField
            End Get
            Set
                Me.businessCheckField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property IsPayeeZipInRange() As Boolean
            Get
                Return Me.isPayeeZipInRangeField
            End Get
            Set
                Me.isPayeeZipInRangeField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property RegisteredCompany() As Boolean
            Get
                Return Me.registeredCompanyField
            End Get
            Set
                Me.registeredCompanyField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property ExpiredCompany() As Boolean
            Get
                Return Me.expiredCompanyField
            End Get
            Set
                Me.expiredCompanyField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property ExceedMaxAmount() As Boolean
            Get
                Return Me.exceedMaxAmountField
            End Get
            Set
                Me.exceedMaxAmountField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property ExceedMaxAge() As Boolean
            Get
                Return Me.exceedMaxAgeField
            End Get
            Set
                Me.exceedMaxAgeField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property CheckNumUnder300() As Boolean
            Get
                Return Me.checkNumUnder300Field
            End Get
            Set
                Me.checkNumUnder300Field = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property DuplicateCheck() As Boolean
            Get
                Return Me.duplicateCheckField
            End Get
            Set
                Me.duplicateCheckField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property CompanyOnHold() As Boolean
            Get
                Return Me.companyOnHoldField
            End Get
            Set
                Me.companyOnHoldField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property PayeeOnHold() As Boolean
            Get
                Return Me.payeeOnHoldField
            End Get
            Set
                Me.payeeOnHoldField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property UnderSmallAmount() As Boolean
            Get
                Return Me.underSmallAmountField
            End Get
            Set
                Me.underSmallAmountField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property CompanyFirstCheck() As Boolean
            Get
                Return Me.companyFirstCheckField
            End Get
            Set
                Me.companyFirstCheckField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property ExceedFirstCheckMaxAmt() As Boolean
            Get
                Return Me.exceedFirstCheckMaxAmtField
            End Get
            Set
                Me.exceedFirstCheckMaxAmtField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property NewPayee() As Boolean
            Get
                Return Me.newPayeeField
            End Get
            Set
                Me.newPayeeField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property NewPayeeMultiNew() As Boolean
            Get
                Return Me.newPayeeMultiNewField
            End Get
            Set
                Me.newPayeeMultiNewField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property FirstCheckCleared() As Boolean
            Get
                Return Me.firstCheckClearedField
            End Get
            Set
                Me.firstCheckClearedField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property NewCompany() As Boolean
            Get
                Return Me.newCompanyField
            End Get
            Set
                Me.newCompanyField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property ExceedNewCompanyCount() As Boolean
            Get
                Return Me.exceedNewCompanyCountField
            End Get
            Set
                Me.exceedNewCompanyCountField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property ReachedVelocityLimit() As Boolean
            Get
                Return Me.reachedVelocityLimitField
            End Get
            Set
                Me.reachedVelocityLimitField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property CheckNumInRange() As Boolean
            Get
                Return Me.checkNumInRangeField
            End Get
            Set
                Me.checkNumInRangeField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property CompanySecondCheck() As Boolean
            Get
                Return Me.companySecondCheckField
            End Get
            Set
                Me.companySecondCheckField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property GuranteeCode() As String
            Get
                Return Me.guranteeCodeField
            End Get
            Set
                Me.guranteeCodeField = value
            End Set
        End Property
        
        '''<remarks/>
        Public Property CompanyID() As String
            Get
                Return Me.companyIDField
            End Get
            Set
                Me.companyIDField = value
            End Set
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")>  _
    Public Delegate Sub CheckVerificationCompletedEventHandler(ByVal sender As Object, ByVal e As CheckVerificationCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class CheckVerificationCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As System.Data.DataSet
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),System.Data.DataSet)
            End Get
        End Property
        
        '''<remarks/>
        Public ReadOnly Property objBusinessLogic() As BusinessLogic
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(1),BusinessLogic)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")>  _
    Public Delegate Sub lookupCompanyCompletedEventHandler(ByVal sender As Object, ByVal e As lookupCompanyCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class lookupCompanyCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As System.Data.DataSet
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),System.Data.DataSet)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")>  _
    Public Delegate Sub UpdatePayeeCompletedEventHandler(ByVal sender As Object, ByVal e As UpdatePayeeCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class UpdatePayeeCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As System.Data.DataSet
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),System.Data.DataSet)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")>  _
    Public Delegate Sub UpdateCompanyCompletedEventHandler(ByVal sender As Object, ByVal e As UpdateCompanyCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class UpdateCompanyCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As System.Data.DataSet
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),System.Data.DataSet)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")>  _
    Public Delegate Sub UpdateCheckCompletedEventHandler(ByVal sender As Object, ByVal e As UpdateCheckCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class UpdateCheckCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As System.Data.DataSet
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),System.Data.DataSet)
            End Get
        End Property
    End Class
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1")>  _
    Public Delegate Sub MatrixBusinessRulesCompletedEventHandler(ByVal sender As Object, ByVal e As MatrixBusinessRulesCompletedEventArgs)
    
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Web.Services", "4.0.30319.1"),  _
     System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.ComponentModel.DesignerCategoryAttribute("code")>  _
    Partial Public Class MatrixBusinessRulesCompletedEventArgs
        Inherits System.ComponentModel.AsyncCompletedEventArgs
        
        Private results() As Object
        
        Friend Sub New(ByVal results() As Object, ByVal exception As System.Exception, ByVal cancelled As Boolean, ByVal userState As Object)
            MyBase.New(exception, cancelled, userState)
            Me.results = results
        End Sub
        
        '''<remarks/>
        Public ReadOnly Property Result() As BusinessLogic
            Get
                Me.RaiseExceptionIfNecessary
                Return CType(Me.results(0),BusinessLogic)
            End Get
        End Property
    End Class
End Namespace
