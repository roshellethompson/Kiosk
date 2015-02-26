Imports System.Configuration

Public Class clsJournal
    Dim strIncident As String = ""
    Dim strApp As String = ""
    Dim strMsg As String = ""
    Dim strDesc As String = ""
    Dim mAmt As Decimal = 0.0
    Dim iCustID As Integer = 0
    Dim strData As String = ""
    Dim iBlockID As Integer
    Dim iTransID As Integer
    Public Property Data As String
        Get
            Return strData
        End Get
        Set(value As String)
            strData = value
        End Set
    End Property
    Public Property Amount As Decimal
        Get
            Return mAmt
        End Get
        Set(value As Decimal)
            mAmt = value
        End Set
    End Property
    Public Property CustID As Integer
        Get
            Return iCustID
        End Get
        Set(value As Integer)
            iCustID = value
        End Set
    End Property
    Public Property Incident As String
        Get
            Return strIncident
        End Get
        Set(value As String)
            strIncident = value
        End Set
    End Property
    Public Property App As String
        Get
            Return strApp
        End Get
        Set(value As String)
            strApp = value
        End Set
    End Property
    Public Property Msg As String
        Get
            Return strMsg
        End Get
        Set(value As String)
            strMsg = value
        End Set
    End Property
    Public Property Description As String
        Get
            Return strDesc
        End Get
        Set(value As String)
            strDesc = value
        End Set
    End Property
    Public Sub Save()
        '.SaveJournal(iCustID, Date.Now, iBlockID.ToString, iTransID.ToString, Incident, App, Description, Amount, Data)
    End Sub

    Public Sub New()
        '  oJourn.Url = ConfigurationManager.AppSettings("SaveJournalURL")
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
