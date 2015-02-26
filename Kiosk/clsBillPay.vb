Imports System.Xml
Imports System.Configuration
Imports System
Public Class clsBillPay
    Public Property oFlags As clsCheck.clsCheckFlags
    Public Property oLog As New clsLog
    Dim mBillAmount As Decimal = 0
    Dim iBlockID As Integer = 0
    Dim iTransID As Integer = 0
    Dim iCustomerID As Integer = 0
    Dim mFee As Decimal = 0
    Dim iStatus As Integer = 0


    Public Property BillAmount As Double
        Get
            Return mBillAmount
        End Get
        Set(value As Double)
            mBillAmount = value
        End Set
    End Property
    Public Property Fee As Double
        Get
            Return mFee
        End Get
        Set(value As Double)
            mFee = value
        End Set
    End Property
    Public Property BlockID As Integer
        Get
            Return iBlockID
        End Get
        Set(value As Integer)
            iBlockID = value
        End Set
    End Property
    Public Property TransID As Integer
        Get
            Return iTransID
        End Get
        Set(value As Integer)
            iTransID = value
        End Set
    End Property
    Public Property CompanyID As Integer
        Get
            Return iCustomerID
        End Get
        Set(value As Integer)
            iCustomerID = value
        End Set
    End Property
    Public Property Status As Integer
        Get
            Return iStatus
        End Get
        Set(value As Integer)
            iStatus = value
        End Set
    End Property
End Class
