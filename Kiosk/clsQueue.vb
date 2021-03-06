﻿Imports System.Configuration
Public Class clsQueue
    Dim strRet1 As String
    Dim strRet2 As String
    Dim strRet3 As String
    Dim strRet4 As String
    Dim strData1 As String
    Dim strData2 As String
    Dim strData3 As String
    Dim strData4 As String
    Dim strData5 As String
    Dim iWksId As Integer
    Dim iProviderID As Integer
    Dim strQueueCode As String = ""
    Dim iCustID As Integer
    Dim iQueueId As Integer = 0
    Dim strStatusCode As String
    Dim strTranCode As String
    Dim oChkService As New CheckService.ICheckServiceservice
    Public Property QueueTimeOut As Integer
    Public Property Data6 As String
    Public Property TranCode As String
        Get
            Return strTranCode
        End Get
        Set(value As String)
            strTranCode = value
        End Set
    End Property

    Public Property StatusCode As String
        Get
            Return strStatusCode
        End Get
        Set(value As String)
            strStatusCode = value
        End Set
    End Property
    Public Property QueueID As Integer
        Get
            Return iQueueId
        End Get
        Set(value As Integer)
            iQueueId = value
        End Set
    End Property
    Public Property CustomerID As Integer
        Get
            Return iCustID
        End Get
        Set(value As Integer)
            iCustID = value
        End Set
    End Property
    Public Property Data3 As String
        Get
            Return strData3
        End Get
        Set(value As String)
            strData3 = value
        End Set
    End Property
    Public Property Data4 As String
        Get
            Return strData4
        End Get
        Set(value As String)
            strData4 = value
        End Set
    End Property
    Public Property Data5 As String
        Get
            Return strData5
        End Get
        Set(value As String)
            strData5 = value
        End Set
    End Property
    Public Property QueueCode As String
        Get
            Return strQueueCode
        End Get
        Set(value As String)
            strQueueCode = value
        End Set
    End Property
    Public Property ProviderID As String
        Get
            Return iProviderID
        End Get
        Set(value As String)
            iProviderID = value
        End Set
    End Property
    Public Property WorkstationID As Integer
        Get
            Return iWksId
        End Get
        Set(value As Integer)
            iWksId = value
        End Set
    End Property
    Public Property Data2 As String
        Get
            Return strData2
        End Get
        Set(value As String)
            strData2 = value
        End Set
    End Property
    Public Property Data1 As String
        Get
            Return strData1
        End Get
        Set(value As String)
            strData1 = value
        End Set
    End Property
    Public Property ReturnCode4 As String
        Get
            Return strRet4
        End Get
        Set(value As String)
            strRet4 = value
        End Set
    End Property
    Public Property ReturnCode3 As String
        Get
            Return strRet3
        End Get
        Set(value As String)
            strRet3 = value
        End Set
    End Property
    Public Property ReturnCode2 As String
        Get
            Return strRet2
        End Get
        Set(value As String)
            strRet2 = value
        End Set
    End Property
    Public Property ReturnCode1 As String
        Get
            Return strRet1
        End Get
        Set(value As String)
            strRet1 = value
        End Set
    End Property
    Public Property ProcessorID As String
    Public Sub New()

    End Sub
    Public Sub New(ByVal iWorkstationID As Integer)
        Dim oCheckCashing = New clsDataModule.clsInterface()

        oChkService.Url = ConfigurationManager.AppSettings("CheckServiceURL")
        WorkstationID = iWorkstationID
        QueueTimeOut = oCheckCashing.GetKioskSettings("QueueTimeOut", WorkstationID)
        ProviderID = ConfigurationManager.AppSettings("ProviderID")
        CustomerID = 0
        TranCode = ""
        ProcessorID = ""
        Data1 = ""
        Data2 = ""
        Data3 = ""
        Data4 = ""
        Data5 = ""
        Data6 = ""
        ReturnCode1 = ""
        ReturnCode2 = ""
        ReturnCode3 = ""
        ReturnCode4 = ""
    End Sub
    Public Property InputQueue As New clsQueue.clsInputQueue

    Public Function PostToQueue() As Boolean
        InputQueue.CustomerID = CustomerID
        InputQueue.QueueCode = QueueCode
        InputQueue.Data1 = Data1
        InputQueue.Data2 = Data2
        InputQueue.Data3 = Data3
        InputQueue.Data4 = Data4
        InputQueue.Data5 = Data5
        InputQueue.Data6 = Data6
        InputQueue.ReturnCode1 = ReturnCode1
        InputQueue.ReturnCode2 = ReturnCode2
        InputQueue.ReturnCode3 = ReturnCode3
        InputQueue.ReturnCode4 = ReturnCode4

        oChkService.PostAndCheckEZCashQ(iQueueId, ProviderID, WorkstationID, CustomerID, QueueCode, StatusCode, Data1, Data2, Data3, Data4, Data5, Data6, ReturnCode1, ReturnCode2, ReturnCode3, ReturnCode4, QueueTimeOut)
    End Function


    Public Class clsInputQueue
        Dim strRet1 As String
        Dim strRet2 As String
        Dim strRet3 As String
        Dim strRet4 As String
        Dim strData1 As String
        Dim strData2 As String
        Dim strData3 As String
        Dim strData4 As String
        Dim strData5 As String
        Dim iWksId As Integer
        Dim iProviderID As Integer
        Dim strQueueCode As String = ""
        Dim iCustID As Integer
        Dim iQueueId As Integer = 0
        Dim strStatusCode As String
        Dim strTranCode As String
        Dim oChkService As New CheckService.ICheckServiceservice
        Public Property Data6 As String
        Public Property TranCode As String
            Get
                Return strTranCode
            End Get
            Set(value As String)
                strTranCode = value
            End Set
        End Property

        Public Property StatusCode As String
            Get
                Return strStatusCode
            End Get
            Set(value As String)
                strStatusCode = value
            End Set
        End Property
        Public Property QueueID As Integer
            Get
                Return iQueueId
            End Get
            Set(value As Integer)
                iQueueId = value
            End Set
        End Property
        Public Property CustomerID As Integer
            Get
                Return iCustID
            End Get
            Set(value As Integer)
                iCustID = value
            End Set
        End Property
        Public Property Data3 As String
            Get
                Return strData3
            End Get
            Set(value As String)
                strData3 = value
            End Set
        End Property
        Public Property Data4 As String
            Get
                Return strData4
            End Get
            Set(value As String)
                strData4 = value
            End Set
        End Property
        Public Property Data5 As String
            Get
                Return strData5
            End Get
            Set(value As String)
                strData5 = value
            End Set
        End Property
        Public Property QueueCode As String
            Get
                Return strQueueCode
            End Get
            Set(value As String)
                strQueueCode = value
            End Set
        End Property
        Public Property ProviderID As Integer
            Get
                Return iProviderID
            End Get
            Set(value As Integer)
                iProviderID = value
            End Set
        End Property
        Public Property WorkstationID As Integer
            Get
                Return iWksId
            End Get
            Set(value As Integer)
                iWksId = value
            End Set
        End Property
        Public Property Data2 As String
            Get
                Return strData2
            End Get
            Set(value As String)
                strData2 = value
            End Set
        End Property
        Public Property Data1 As String
            Get
                Return strData1
            End Get
            Set(value As String)
                strData1 = value
            End Set
        End Property
        Public Property ReturnCode4 As String
            Get
                Return strRet4
            End Get
            Set(value As String)
                strRet4 = value
            End Set
        End Property
        Public Property ReturnCode3 As String
            Get
                Return strRet3
            End Get
            Set(value As String)
                strRet3 = value
            End Set
        End Property
        Public Property ReturnCode2 As String
            Get
                Return strRet2
            End Get
            Set(value As String)
                strRet2 = value
            End Set
        End Property
        Public Property ReturnCode1 As String
            Get
                Return strRet1
            End Get
            Set(value As String)
                strRet1 = value
            End Set
        End Property
        Public Property ProcessorID As String

        Public Sub New()

        End Sub
    End Class
End Class
