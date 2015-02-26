Imports System.Configuration
Imports System

Public Class clsScreens
    Public Property oPin As clsPinAction
    Dim iWksId As Integer
    Dim iProviderID As Integer
    Dim iCustID As Integer
    Dim oChkService As New CheckService.ICheckServiceservice
    Public Property KSN As String = ""
    Public Property UserInput As String = ""
    Public Property PinBlock As String = ""
    Public Property LangID As Integer = 0
    Public Property ButtonPressed As String = "0"
    Public Property CustomerID As Integer = 0
    Public Property Template As String = ""
    Public Property ProviderID As Integer = 0
    Public Property PAN As String
    Public Property WaitForInput As Integer = 0
    Public Property ReturnToIdleSecs As Integer = 0
    Public Property PinFlag As Integer = 0
    Public Property CardID As Integer = 0
    Public Property WorkstationID As Integer
    Public Property TimeOut As Integer = 0
    Public Property ProcessorID As String
    Public Property Info As String()
    Public Property Info1 As String
    Public Property Info2 As String
    Public Property Info3 As String
    Public Property Info4 As String
    Public Property Info5 As String
    Public Property Info6 As String
    Public Property Info7 As String
    Public Property Info8 As String
    Public Property loaded As Boolean

    Public Property oLog As clsLog
    Public Sub New()

    End Sub
    Public Sub New(ByVal iWorkstationID As Integer, ByVal pErr As clsError, ByVal pCustomer As clsCustomer, ByVal pLog As clsLog)
        oChkService.Url = ConfigurationManager.AppSettings("CheckServiceURL")
        WorkstationID = iWorkstationID
        ProviderID = ConfigurationManager.AppSettings("ProviderID")
        CustomerID = 0
        ProcessorID = ""
        oLog = pLog.Clone()
        oPin = New clsPinAction(iWorkstationID, pErr, pLog, pCustomer)
    End Sub
    Public Property InputScreen As New clsScreens.clsInputScreen
    Public Property OutputScreen As New clsScreens.clsInputScreen

    Public Function PostToQueue() As Boolean
        InputScreen.CustomerID = CustomerID
        InputScreen.ButtonPressed = ButtonPressed

        InputScreen.KSN = KSN
        InputScreen.LangID = LangID
        InputScreen.PinBlock = PinBlock
        InputScreen.UserInput = UserInput
        InputScreen.Template = Template
        InputScreen.WaitForInput = WaitForInput
        InputScreen.ReturnToIdleSecs = ReturnToIdleSecs
        InputScreen.CardID = CardID
        InputScreen.Info = Info
        InputScreen.PAN = PAN
        'oLog.LogMsg("PAN =" + PAN)
        'oLog.LogMsg("PINFlag" + PinFlag.ToString())
        If UserInput = "" Then
            UserInput = "0"
        End If

        Try
            oChkService.LoadEquinoxScreen(WorkstationID, LangID, Template, PinFlag, WaitForInput, TimeOut, ReturnToIdleSecs, PAN, Info, PinBlock, KSN, UserInput, ButtonPressed)
        Catch e As System.Threading.ThreadAbortException
            Console.WriteLine("Thread - caught ThreadAbortException - resetting.")
            Console.WriteLine("Exception message: {0}", e.Message)
            System.Threading.Thread.ResetAbort()
        Catch e As Exception
            Console.WriteLine("Exception message: {0}", e.Message)
        End Try
        If ButtonPressed = "" Then
            ButtonPressed = "0"
        End If
        OutputScreen.CustomerID = CustomerID
        OutputScreen.ButtonPressed = ButtonPressed
        OutputScreen.KSN = KSN
        OutputScreen.LangID = LangID
        OutputScreen.PinBlock = PinBlock
        OutputScreen.UserInput = UserInput
        OutputScreen.Template = Template
        OutputScreen.WaitForInput = WaitForInput
        OutputScreen.ReturnToIdleSecs = ReturnToIdleSecs
        OutputScreen.CardID = CardID
        OutputScreen.Info = Info
        OutputScreen.Info1 = Info1
        OutputScreen.Info2 = Info2
        OutputScreen.Info3 = Info3
        OutputScreen.Info4 = Info4
        OutputScreen.Info5 = Info5
        OutputScreen.Info6 = Info6
        OutputScreen.Info7 = Info7
        OutputScreen.PAN = PAN
        oPin.oErr.ErrCode = 0

        Info1 = ""
        Info2 = ""
        Info3 = ""
        Info4 = ""
        Info5 = ""
        Info6 = ""
        Info7 = ""
    End Function
    Public Sub PlaceID()
        Template = "PlaceID"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        'oChkService.Trace("oScreens.PlaceID")
        PostToQueue()
    End Sub
    Public Sub InvalidCard()
        Template = "Invalid"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 8
        'oChkService.Trace("oScreens.InvalidCard")
        PostToQueue()
    End Sub
    Public Sub CaptureSSN()
        Template = "SSN"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        'oChkService.Trace("oScreens.SSN")
        PostToQueue()
    End Sub
    Public Sub ChoosePayee()
        Template = "Payees"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        'oChkService.Trace("oScreens.ChoosePayee")
        PostToQueue()
    End Sub
    Public Sub AddPayeeSuccess()
        Template = "AddPayeeSuccess"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 10
        'oChkService.Trace("oScreens.AddPayeeSuccess")
        PostToQueue()
    End Sub
    Public Sub BillPaySuccess()
        Template = "BillPaySuccess"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 10
        'oChkService.Trace("oScreens.BillPaySuccess")
        PostToQueue()
    End Sub
    Public Sub HardwareError()
        Template = "HardwareError"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 10
        'oChkService.Trace("oScreens.HardwareError")
        PostToQueue()
    End Sub
    Public Sub InsertBill()
        Template = "InsertBill"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        'oChkService.Trace("oScreens.InsertBill")
        PostToQueue()
    End Sub
    Public Sub MakePayment()
        Template = "MakePayment"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        'oChkService.Trace("oScreens.MakePayment")
        PostToQueue()
    End Sub
    Public Sub BillPay()
        Template = "BillPay"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        'oChkService.Trace("oScreens.BillPay")
        PostToQueue()
    End Sub
    Public Sub CaptureMobileNum()
        Template = "MobPhone"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        'oChkService.Trace("oScreens.CaptureMobileNum")
        PostToQueue()
    End Sub
    Public Sub CaptureCoPhone()
        Template = "CoPhone"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        'oChkService.Trace("oScreens.CoPhone")
        PostToQueue()
    End Sub
    Public Sub CheckNotSigned()
        Template = "CheckNotSigned"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 20
        ReturnToIdleSecs = 8
        'oChkService.Trace("oScreens.CheckNotSigned")
        PostToQueue()
    End Sub
    Public Sub CheckAmount()
        Template = "CheckAmount"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        ' oChkService.Trace("oScreens.CheckAmount")
        PostToQueue()
    End Sub
    Public Sub PaymentAmount()
        Template = "PaymentAmount"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        'oChkService.Trace("oScreens.PaymentAmount")
        PostToQueue()
    End Sub
    Public Sub CaptureDOB()
        Template = "EnterDOB"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        'oChkService.Trace("oScreens.CaptureDOB")
        PostToQueue()
    End Sub
    Public Sub CapturePAN()
        Template = "Idle"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        'oChkService.Trace("oScreens.CapturePAN")
        PostToQueue()
    End Sub
    Public Sub SignCheck()
        Template = "SignCheck"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 300
        'oChkService.Trace("oScreens.SignCheck")
        PostToQueue()
    End Sub

    Public Sub LaunchWaitScreen()
        Template = "Wait"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 10
        ReturnToIdleSecs = 500
        'oChkService.Trace("oScreens.LaunchWaitScreen")
        PostToQueue()
    End Sub

    Public Sub LaunchRegWaitScreen()
        Template = "RegWait"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 10
        ReturnToIdleSecs = 500
        'oChkService.Trace("oScreens.LaunchRegWaitScreen")
        PostToQueue()
    End Sub

    Public Sub ErrScreen(ByVal strErrMsg As String())
        Info = {}
        Template = "Error"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 8
        Info = strErrMsg
        'oChkService.Trace("oScreens.ErrScreen")
        PostToQueue()
    End Sub
    Public Sub CannotCash()
        Template = "CannotCash"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 8
        'oChkService.Trace("oScreens.CannotCash")
        PostToQueue()
    End Sub
    Public Sub CheckTimeout()
        Template = "Timeout"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 8
        'oChkService.Trace("oScreens.CheckTimeout")
        PostToQueue()
    End Sub
    Public Sub WillText()
        Template = "WillText"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 8
        ' oChkService.Trace("oScreens.WillText")
        PostToQueue()
    End Sub
    Public Sub CheckInfo(ByVal strVals As String())
        Template = "CheckInfo"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 300
        ReturnToIdleSecs = 0
        Info = strVals
        'oChkService.Trace("oScreens.CheckInfo")
        PostToQueue()
    End Sub
    Public Sub BillPayReceipt(ByVal strVals As String())
        Template = "BillPayReceipt"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 300
        ReturnToIdleSecs = 0
        Info = strVals
        'oChkService.Trace("oScreens.BillPayReceipt")
        PostToQueue()
    End Sub
    Public Sub FinalBalance(ByVal strVals As String())
        Template = "Balance"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 30
        ReturnToIdleSecs = 0
        Info = strVals
        'oChkService.Trace("oScreens.FinalBalance")
        PostToQueue()
    End Sub
    Public Sub EndRegistration()
        Template = "FinishReg"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 4
        'oChkService.Trace("oScreens.FinishReg")
        PostToQueue()
    End Sub
    Public Sub EndCheckCash()
        Template = "FinishCheck"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 4
        'oChkService.Trace("oScreens.EndCheckCash")
        PostToQueue()
    End Sub
    Public Sub CardReset(ByVal encryptPan As String)
        Template = "CardReset"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 8
        PAN = encryptPan
        'oChkService.Trace("oScreens.CardReset")
        PostToQueue()
    End Sub
    Public Sub GetPINForVerify(ByVal encryptPan As String)
        Template = "EnterPIN"
        PinFlag = 1
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        PAN = encryptPan
        'oChkService.Trace("oScreens.EnterPIN - " + encryptPan)
        PostToQueue()
    End Sub

    Public Sub GetNewPIN(ByVal encryptPan As String)
        Template = "EnterNewPIN"
        PinFlag = 1
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        PAN = encryptPan
        'oChkService.Trace("oScreens.EnterNewPIN")
        PostToQueue()
    End Sub
    Public Sub ReenterNewPIN(ByVal encryptPan As String)
        Template = "ReenterNewPIN"
        PinFlag = 1
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        PAN = encryptPan
        'oChkService.Trace("oScreens.ReenterNewPIN")
        PostToQueue()
    End Sub
    Public Sub ReenterPIN(ByVal encryptPan As String)
        Template = "ReenterPIN"
        PinFlag = 1
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        PAN = encryptPan
        'oChkService.Trace("oScreens.ReenterPIN")
        PostToQueue()
    End Sub
    Public Sub IssuingCard()
        Template = "IssuingCard"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 10
        'oChkService.Trace("oScreens.IssuingCard")
        PostToQueue()

    End Sub
    Public Sub IssuingPIN()
        Template = "IssuingPIN"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 10
        'oChkService.Trace("oScreens.IssuingPIN")

        PostToQueue()
    End Sub
    Public Sub IssuingVerify()
        Template = "IssuingVerify"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 10
        'oChkService.Trace("oScreens.IssuingVerify")
        PostToQueue()

    End Sub
    Public Sub MaybeCashCheck()
        Template = "MaybeCashCheck"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 10
        'oChkService.Trace("oScreens.MaybeCashCheck")
        PostToQueue()
    End Sub
    Public Sub CheckAlreadyCashed()
        Template = "CheckAlreadyCashed"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 10
        'oChkService.Trace("oScreens.CheckAlreadyCashed")
        PostToQueue()
    End Sub
    Public Sub CustOptions(CDType As String)
        Template = "Options" + CDType
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        'oChkService.Trace("oScreens.CustOptions")
        PostToQueue()
    End Sub

    Public Sub CashCheckOrNewPhotoID()
        Template = "CheckOrReg"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        'oChkService.Trace("oScreens.CashCheckOrNewPhotoID")
        PostToQueue()
    End Sub
    Public Sub RegistrationSuccessful()
        Template = "RegSuccess"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 30
        ReturnToIdleSecs = 8
        'oChkService.Trace("oScreens.RegSuccess")
        PostToQueue()
    End Sub
    Public Sub InvalidCardForComp()
        Template = "InvalidComp"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 10
        'oChkService.Trace("oScreens.InvalidCardForComp")
        PostToQueue()
    End Sub
    Public Sub ReplaceCard()
        Template = "ReplaceCard"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        'oChkService.Trace("oScreens.ReplaceCard")
        PostToQueue()
    End Sub
    Public Sub TransactionCancelled()
        Template = "Cancel"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 10
        'oChkService.Trace("oScreens.TransactionCancelled")
        PostToQueue()
    End Sub
    Public Sub NewCardReady()
        Template = "UseNewCard"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 15
        'oChkService.Trace("oScreens.NewCardReady")
        PostToQueue()
    End Sub
    Public Sub LanguageOptions()
        Template = "LanguageOptions"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        'oChkService.Trace("oScreens.LanguageOptions")
        PostToQueue()
    End Sub
    Public Sub Exiting()
        Template = "Exiting"
        Info = {}
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 8
        'oChkService.Trace("oScreens.Exiting")
        PostToQueue()
    End Sub

    Public Sub PrepareChecks()
        Template = "PrepareChecks"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        'oChkService.Trace("oScreens.PrepareChecks")
        PostToQueue()
    End Sub
    Public Sub AcceptFee(ByVal strVals())
        Template = "AcceptFee"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        Info = strVals
        'oChkService.Trace("oScreens.AcceptFee")
        PostToQueue()
    End Sub
    Public Sub DoingBGCheck()
        Template = "BackgroundCheck"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 10
        ReturnToIdleSecs = 8
        'oChkService.Trace("oScreens.DoingBGCheck")
        PostToQueue()
    End Sub
    Public Sub BackGroundCheckResults(ByVal strVals())
        Template = "BackgroundCheck"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 10
        ReturnToIdleSecs = 8
        Info = strVals
        'oChkService.Trace("oScreens.BackgroundCheckResults")

        PostToQueue()
    End Sub
    Public Sub ScannerFailedToInit()
        Template = "ScannerNotInit"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 15
        ReturnToIdleSecs = 20
        'oChkService.Trace("oScreens.ScannerFailedToInit")
        PostToQueue()
    End Sub
    Public Sub TooLargeAmount()
        Template = "TooLargeAmount"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 15
        ReturnToIdleSecs = 20
        'oChkService.Trace("oScreens.TooLargeAmount")
        PostToQueue()
    End Sub
    Public Sub WaitForIDScanner()
        Template = "ScanningID"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 60

        'oChkService.Trace("oScreens.ScanningID")
        PostToQueue()
    End Sub
    Public Sub WaitForCheckScanner(ByVal numTries As Integer)
        Template = "ScanningCheck"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 20
        Info5 = numTries.ToString()
        ' oChkService.Trace("oScreens.WaitForCheckScanner")
        PostToQueue()
    End Sub
    Public Sub NewCardStart()
        Template = "NewCardStart"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        'oChkService.Trace("oScreens.NewCardStart")
        PostToQueue()
    End Sub
    Public Sub RegOrNewCard()
        Template = "RegOrNewCard"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        'oChkService.Trace("oScreens.RegOrNewCard")
        PostToQueue()
    End Sub
    Public Sub PinUpdated()
        Template = "PinUpdated"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 7
        'oChkService.Trace("oScreens.PINUpdated")
        PostToQueue()
    End Sub
    Public Sub PhotoIDUpdated()
        Template = "PhotoIDUpdated"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 15
        ReturnToIdleSecs = 7
        'oChkService.Trace("oScreens.PhotoIDUpdated")
        PostToQueue()
    End Sub
    Public Sub TryCheckAgain()
        Template = "TryCheckAgain"
        PinFlag = 0
        WaitForInput = 0
        TimeOut = 60
        ReturnToIdleSecs = 5
        'oChkService.Trace("oScreens.TryCheckAgain")
        PostToQueue()
    End Sub
    Public Sub ContWaiting()
        Template = "ContWaiting"
        PinFlag = 0
        WaitForInput = 1
        TimeOut = 60
        ReturnToIdleSecs = 0
        'oChkService.Trace("oScreens.ContWaiting")
        PostToQueue()
    End Sub
    Public Class clsInputScreen
        Public Property KSN As String = ""
        Public Property UserInput As String = ""
        Public Property PinBlock As String = ""
        Public Property LangID As Integer
        Public Property ButtonPressed As String = ""
        Public Property CustomerID As Integer
        Public Property Template As String = ""
        Public Property WaitForInput As Integer = 0
        Public Property ReturnToIdleSecs As Integer = 0
        Public Property PinFlag As Integer = 0
        Public Property CardID As Integer = 0
        Public Property Info As String()
        Public Property Info1 As String = ""
        Public Property Info2 As String = ""
        Public Property Info3 As String = ""
        Public Property Info4 As String = ""
        Public Property Info5 As String = ""
        Public Property Info6 As String = ""
        Public Property Info7 As String = ""
        Public Property PAN As String = ""

        Public Sub New()

        End Sub
    End Class
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
        Public Property oCheckCashing As New clsDataModule.clsInterface
        Public Property oCustomer As clsCustomer
        Public Property TransactionType As String = ""
        Public Property PaymentNumber As Integer = 0
        Public Function IssuingCardToSwitch()
            oQData.CustomerID = CustomerID
            oQData.WorkstationID = WorkstationID
            oQData.QueueCode = "IC"
            oQData.Data1 = String.Empty
            oQData.Data2 = CardID
            oQData.Data3 = CardType
            oQData.Data4 = "0"
            oQData.Data5 = String.Empty
            oQData.PostToQueue()
            If CType(oQData.ReturnCode2, Integer) > 0 Then
                Return False
            End If
            ProcessQueue()
            If (oErr.ErrCode > 0) Then
                Return False
            End If
            Return True
        End Function
        Public Function IssuingPinToSwitch() As Boolean
            oQData.CustomerID = CustomerID
            oQData.WorkstationID = WorkstationID
            oQData.QueueCode = "IP"
            oQData.Data1 = PinBlock
            oQData.Data2 = CardID
            oQData.Data3 = KSN
            oQData.Data4 = String.Empty
            oQData.Data5 = String.Empty
            oQData.PostToQueue()
            If CType(oQData.ReturnCode2, Integer) > 0 Then
                Return False
            End If
            ProcessQueue()
            If (oErr.ErrCode > 0) Then
                Return False
            End If
            Return True
        End Function
        Public Function VerifyPinAtSwitch() As Boolean
            oQData.CustomerID = CustomerID
            oQData.WorkstationID = WorkstationID
            oQData.QueueCode = "VP"
            oQData.Data1 = PinBlock
            oQData.Data2 = CardID
            oQData.Data3 = KSN
            oQData.Data4 = String.Empty
            oQData.Data5 = String.Empty
            oQData.PostToQueue()
            If oQData.ReturnCode2 Is Nothing Then
                Return False
            End If
            If CType(oQData.ReturnCode2, Integer) > 0 Then
                Return False
            End If
            VerifyPinReturnCode = CType(oQData.ReturnCode2, Integer)
            If VerifyPinReturnCode = INVALIDPIN Then
                Return False
            End If
            ProcessQueue()
            If (oErr.ErrCode > 0) Then
                Return False
            End If
            Return True
        End Function
        Public Function AddMoneyAtSwitch() As Integer
            oQData = New Kiosk.clsQueue(WorkstationID)
            oQData.CustomerID = CustomerID
            oQData.QueueCode = "PP"
            oQData.Data1 = CashAmt.ToString()
            oQData.Data2 = CardID
            oQData.Data3 = TransactionType
            oQData.Data4 = BlockID.ToString()
            oQData.Data5 = TransactionNumber.ToString()
            oQData.Data6 = PaymentNumber.ToString()
            oQData.PostToQueue()
            If CType(oQData.ReturnCode2, Integer) = 0 Then
                oQData.ReturnCode2 = "0"
            End If

            If IsDBNull(oQData.ReturnCode2) Then
                Return 1
            End If
            If CType(oQData.ReturnCode2, Integer) > 0 Then
                Return oQData.ReturnCode2
            End If
            ProcessQueue()
            If (oErr.ErrCode > 0) Then
                Return 1
            End If
            Return oQData.ReturnCode2
        End Function
        Public Function App_Path() As String
            Return System.AppDomain.CurrentDomain.BaseDirectory()
        End Function
        Public Sub New(ByVal iWorkstationID As Integer, ByRef pErr As clsError, ByRef pLog As clsLog, ByVal pCustomer As clsCustomer)
            oQData = New clsQueue(iWorkstationID)
            WorkstationID = iWorkstationID
            oErr = pErr.Clone()
            oLog = pLog.Clone()
            oFlags = New clsPinActionFlags(iWorkstationID)

            oCustomer = pCustomer.Clone()
        End Sub
        Public Sub New()

        End Sub
        Public Function ProcessQueue() As Boolean
            Dim oScreen As New clsScreens(WorkstationID, oErr, oCustomer, oLog)
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
                    If (oQData.ReturnCode2 = "0") Then
                        oFlags.PinOK = True
                    Else
                        oFlags.PinOK = False
                    End If

                    If (oFlags.PinOK) Then
                        oFlags.VPSecondTry = False
                        Dim ppBal, ppAvail, bBal As Double

                        'PrePaidBalance = Double.Parse(oQData.Data1)
                        If Double.TryParse(oQData.Data1, ppBal) Then
                            PrePaidBalance = ppBal
                        End If
                        If Double.TryParse(oQData.Data2, ppAvail) Then
                            PrePaidAvailable = ppAvail
                        End If
                        If Decimal.TryParse(oQData.Data1, bBal) Then
                            BeginningBal = bBal
                        End If

                        If oFlags.IssueCard Then
                            oFlags.EPSecondTry = False
                            'Dim seg1 As New clsDataModule.clsLabel
                            'seg1.strLabel = "CMReg1"
                            'seg1.iTranslate = 1
                            'Dim seg2 As New clsDataModule.clsLabel
                            'seg2.strLabel = "PasswordEntry"
                            'seg2.iTranslate = 1
                            'Dim seg3 As New clsDataModule.clsLabel
                            'seg3.strLabel = oCheckCashing.GetRandomNum(3, 999).ToString().PadRight(3, "0") + oCheckCashing.GetRandomNum(3, 999).ToString().PadRight(3, "0") + oCheckCashing.GetRandomNum(2, 99).ToString().PadRight(2, "0")
                            'seg3.iTranslate = 0
                            'oCustomer.Password = seg3.strLabel
                            'Dim arrSeg(2) As clsDataModule.clsLabel
                            'arrSeg(0) = seg1
                            'arrSeg(1) = seg2
                            'arrSeg(2) = seg3
                            'oCheckCashing.UpdateCustomer(oCheckCashing.CreatePasswordHash(seg3.strLabel, oCustomer.UserSalt), oCustomer.CustomerID)

                            'oCheckCashing.COMMS(oCustomer.CustomerID, arrSeg)
                        End If
                    ElseIf (oFlags.VPSecondTry) Then
                        oFlags.VPSecondTry = False
                        oErr.ErrCode = 30
                    Else
                        oFlags.VPSecondTry = True
                        oErr.ErrCode = 18
                    End If
                ElseIf (oQData.InputQueue.QueueCode = "IP") Then
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
                    If (oQData.ReturnCode2 = "0") Then
                        oFlags.IssueCard = True
                    Else
                        oFlags.IssueCard = False
                    End If
                ElseIf (oQData.InputQueue.QueueCode = "PP") Then
                    If (oQData.StatusCode = "Processing") Then
                        oErr.ErrCode = 21
                    End If
                    If (oQData.ReturnCode2 = "0") Then

                    Else
                        oErr.ErrCode = 21
                    End If
                End If

            Catch ex As Exception
                oLog.LogMsg(ex.ToString())
                oErr.ErrDetail = ex.ToString()
                oErr.ErrText = ex.Message
                Return False
            End Try
            Return True
        End Function
        'Public Sub SendComms()
        '    If oFlags.IssueCard Then
        '        oFlags.EPSecondTry = False
        '        Dim seg1 As New clsDataModule.clsLabel
        '        seg1.strLabel = "CMReg1"
        '        seg1.iTranslate = 1
        '        Dim seg2 As New clsDataModule.clsLabel
        '        seg2.strLabel = "PasswordEntry"
        '        seg2.iTranslate = 1
        '        Dim seg3 As New clsDataModule.clsLabel
        '        seg3.strLabel = oCheckCashing.GetRandomNum(3, 999).ToString().PadRight(3, "0") + oCheckCashing.GetRandomNum(3, 999).ToString().PadRight(3, "0") + oCheckCashing.GetRandomNum(2, 99).ToString().PadRight(2, "0")
        '        seg3.iTranslate = 0
        '        oCustomer.Password = seg3.strLabel
        '        Dim arrSeg(2) As clsDataModule.clsLabel
        '        arrSeg(0) = seg1
        '        arrSeg(1) = seg2
        '        arrSeg(2) = seg3
        '        oCheckCashing.UpdateCustomer(oCheckCashing.CreatePasswordHash(seg3.strLabel, oCustomer.UserSalt), oCustomer.CustomerID)

        '        oCheckCashing.COMMS(oCustomer.CustomerID, arrSeg)
        '    End If
        'End Sub
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
            Dim oCheckCashing = New clsDataModule.clsInterface
            Public Property WorkstationID As Integer = 0
            Public Sub New()

            End Sub
            Public Sub New(ByVal pWorkstationID As Integer)
                WorkstationID = pWorkstationID

                SimPinPad = CType(oCheckCashing.GetKioskSettings("SimPinPad", WorkstationID), Integer)

            End Sub
        End Class
    End Class

End Class

