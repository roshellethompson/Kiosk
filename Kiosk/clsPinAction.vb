Imports System.IO
Imports System

Public Class clsPinAction
    Public Const INVALIDPIN As Integer = 131
    Public Property oFlags As clsPinAction.clsPinActionFlags
    Public VerifyPinReturnCode As Integer = 0
    Public Property oQData As clsQueue
    Public Property CustomerID As Integer = 0
    Public Property WorkstationID As Integer = 0
    Public Property CardID As Integer = 0
    Public Property CardType As String = ""
    Public Property PinBlock As String = ""
    Public Property KSN As String = ""
    Public Property CashAmt As Decimal = 0
    Public Property BlockID As Integer = 0
    Public Property TransactionNumber As Integer = 0
    Public Property PrePaidBalance As Decimal = 0
    Public Property PrePaidAvailable As Decimal = 0
    Public Property BeginningBal As Decimal = 0
    Public Property oLog As clsLog
    Public Property oErr As clsError
    Public Property EncryptPan As String = ""
   
    Public Function IssuingCardToSwitch()
        oQData.CustomerID = CustomerID
        oQData.WorkstationID = WorkstationID
        oQData.QueueCode = "IC"
        oQData.Data1 = String.Empty
        oQData.Data2 = CardID.ToString()
        oQData.Data3 = CardType
        oQData.Data4 = "0"
        oQData.Data5 = String.Empty
        oQData.PostToQueue()
        If oQData.StatusCode.ToLower <> "finished" Then
            Return False
        End If
        If CType(oQData.ReturnCode2, Integer) > 0 Then
            Return False
        End If
        ProcessQueue()
        Return True
    End Function
    Public Function IssuingPinToSwitch()
        oQData.CustomerID = CustomerID
        oQData.WorkstationID = WorkstationID
        oQData.QueueCode = "IP"
        oQData.Data1 = PinBlock
        oQData.Data2 = CardID.ToString()
        oQData.Data3 = KSN
        oQData.Data4 = String.Empty
        oQData.Data5 = String.Empty
        oQData.PostToQueue()
        If oQData.StatusCode.ToLower <> "finished" Then
            Return False
        End If
        If CType(oQData.ReturnCode2, Integer) > 0 Then
            Return False
        End If
        ProcessQueue()
        Return True
    End Function
    Public Function VerifyPinAtSwitch() As Boolean
        oQData.CustomerID = CustomerID
        oQData.WorkstationID = WorkstationID
        oQData.QueueCode = "VP"
        oQData.Data2 = CardID.ToString()
        oQData.Data1 = PinBlock
        oQData.Data3 = KSN
        oQData.Data4 = String.Empty
        oQData.Data5 = String.Empty
        oQData.PostToQueue()
        If oQData.StatusCode.ToLower <> "finished" Then
            Return False
        End If
        If oQData.ReturnCode2 Is Nothing Then
            Return False
        End If
        VerifyPinReturnCode = CType(oQData.ReturnCode2, Integer)
        If VerifyPinReturnCode = INVALIDPIN Then
            Return False
        End If
        ProcessQueue()
        Return True
    End Function
    Public Function AddMoneyAtSwitch()
        oQData = New Kiosk.clsQueue(WorkstationID)
        oQData.CustomerID = CustomerID
        oQData.QueueCode = "PP"
        oQData.Data1 = CashAmt.ToString()
        oQData.Data2 = CardID
        oQData.Data3 = "WDL"
        oQData.Data4 = BlockID.ToString()
        oQData.Data5 = TransactionNumber.ToString()
        oQData.PostToQueue()
        If oQData.StatusCode.ToLower <> "finished" Then
            Return False
        End If
        If CType(oQData.ReturnCode2, Integer) > 0 Then
            Return False
        End If
        ProcessQueue()
    End Function
    Public Function App_Path() As String
        Return System.AppDomain.CurrentDomain.BaseDirectory()
    End Function
    Public Sub New(ByVal iWorkstationID As Integer, ByRef pErr As clsError, ByRef pLog As clsLog)
        oQData = New clsQueue(iWorkstationID)
        WorkstationID = iWorkstationID
        oErr = pErr.Clone()
        oLog = pLog.Clone()
        oFlags = New clsPinActionFlags(iWorkstationID)
    End Sub
    Public Function ProcessQueue() As Boolean
        Dim oScreen As New clsScreens(WorkstationID, oLog)
        Try
            oLog.LogMsg("QueueCode=" + oQData.InputQueue.QueueCode + "\r\n")
            oLog.LogMsg("Status=" + oQData.InputQueue.StatusCode + "\r\n")
            If oQData.InputQueue.ReturnCode2 Is Nothing Then
                oLog.LogMsg("ReturnCode2=nothing" + "\r\n")
            Else
                oLog.LogMsg("ReturnCode2=" + oQData.InputQueue.ReturnCode2 + "\r\n")
            End If
            oLog.LogMsg("Data1=" + oQData.InputQueue.Data1)
            oLog.LogMsg("Data2=" + oQData.InputQueue.Data2)
            oLog.LogMsg("Data3=" + oQData.InputQueue.Data3)
            If (oFlags.SimPinPad) Then
                oQData.ReturnCode2 = "0"
                oQData.Data1 = "0"
                oQData.Data2 = "0"
            End If
            If oQData.InputQueue.QueueCode = "VP" Then
                If (oQData.StatusCode = "Processing") Then
                    oFlags.PinOK = False
                Else
                    If (oQData.ReturnCode2 = "0") Then
                        oFlags.PinOK = True
                    Else
                        oFlags.PinOK = False
                    End If
                End If
                If (oFlags.PinOK) Then
                    oFlags.VPSecondTry = False
                    PrePaidBalance = Double.Parse(oQData.Data1)
                    PrePaidAvailable = Double.Parse(oQData.Data2)
                    BeginningBal = Decimal.Parse(oQData.Data1)
                ElseIf (oFlags.VPSecondTry) Then
                    oFlags.VPSecondTry = False
                    oErr.ErrCode = 30
                Else
                    oFlags.VPSecondTry = True
                    oErr.ErrCode = 18
                End If
            ElseIf (oQData.InputQueue.QueueCode = "IP") Then
                If (oQData.StatusCode = "Processing") Then
                    oFlags.IssuePIN = False
                End If
                If (oQData.ReturnCode2 = "0") Then
                    oFlags.IssuePIN = True
                Else
                    oFlags.IssuePIN = False
                End If
                oFlags.EnteringPin = False
                If (oFlags.IssuePIN) Then
                    Me.IssuingCardToSwitch()
                Else
                    oErr.ErrCode = 19
                End If
            ElseIf (oQData.InputQueue.QueueCode = "IC") Then
                If (oQData.StatusCode = "Processing") Then
                    oFlags.IssueCard = False
                End If
                If (oQData.ReturnCode2 = "0") Then
                    oFlags.IssueCard = True
                Else
                    oFlags.IssueCard = False
                End If
                If (oFlags.IssueCard) Then
                    oScreen.CapturePIN(EncryptPan)
                ElseIf (oQData.InputQueue.QueueCode = "PP") Then
                    If (oQData.StatusCode = "Processing") Then
                        oErr.ErrCode = 21
                    End If
                    If (oQData.ReturnCode2 = "0") Then
                        PrePaidBalance = Double.Parse(oQData.Data1)
                        PrePaidAvailable = Double.Parse(oQData.Data2)
                        If (PrePaidBalance < PrePaidAvailable) Then
                            PrePaidAvailable = PrePaidBalance
                        End If
                    Else
                        oErr.ErrCode = 21
                    End If
                End If
            End If

            oErr.GetErrorData()
        Catch ex As Exception
            oLog.LogMsg(ex.ToString())
            oErr.ErrDetail = ex.ToString()
            oErr.ErrText = ex.Message
            Return False
        End Try
        Return True
    End Function
    Public Class clsPinActionFlags
        Public Property VPSecondTry As Boolean = False
        Public Property IssueCard As Boolean = False
        Public Property IssuePIN As Boolean = False
        Public Property EPSecondTry As Boolean = False
        Public Property EnteringPin As Boolean = False
        Public Property OnFirstPin As Boolean = False
        Public Property OnSecondPin As Boolean = False
        Public Property PinOK As Boolean = False
        Public Property SimPinPad As Boolean = False
        Public Property ShowPinMismatchError As Boolean = False
        Dim oCheckCashing = New CheckCashing.CheckCashing
        Public Property WorkstationID As Integer = 0

        Public Sub New(ByVal pWorkstationID As Integer)
            WorkstationID = pWorkstationID
            oCheckCashing.URL = System.Configuration.ConfigurationManager.AppSettings("CheckCashingURL")
            SimPinPad = CType(oCheckCashing.GetKioskSettings("SimPinPad", WorkstationID), Integer)

        End Sub
    End Class
End Class
