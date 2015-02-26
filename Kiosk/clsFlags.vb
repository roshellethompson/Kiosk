Imports System.Configuration
Imports System.Net
Imports System

Public Class clsFlags
    Public bFpSensorMatch As Boolean = False
    Public bSecondPin As Boolean = False
    Public bReviewed As Boolean = False
    Public bEnter As Boolean = False
    Public bSecondPass As Boolean = False
    Public bSim As Boolean = False
    Public bSimSnapShell As Boolean = False
    Public bSimCheckScanner As Boolean = False
    Public bSimCardRW As Boolean = False
    Public bSimFPReader As Boolean = False
    Public bSimPinPad As Boolean = False
    Public bDataOnly As Boolean = False
    Public bPayeeVerified As Boolean = False
    Public bCheckSigned As Boolean = False
    Public bCheckAltered As Boolean = False
    Public bCheckReviewed As Boolean = False
    Public bPrePaidBalUpdated As Boolean
    Public bImageVerify As Boolean = False
    Public bInPositiveFile As Boolean = False
    Public bPhotoIDMatches As Boolean = False
    Public iRegister As Boolean = False
    Public bRereg As Boolean = False
    Public strTestType As String = ""
    Public iWKSID As Integer
    Public iTime As Integer = 0
    Public bCancel As Boolean = False
    Public bClear As Boolean = False
    Public bTimeOut As Boolean = False
    Public bEOT As Boolean = False
    Public bActivity As Boolean = False
    Public bAck As Boolean = False
    Public iCurrLen As Integer = 0
    Public strTestCode As String = ""
    Public oCheckCashing As New clsDataModule.clsInterface
    Public bTestReview As Boolean = False
    Public bBackGroundOK As Boolean = False
    Public iShortTimeOut As Integer = 0
    Public iLongTimeOut As Integer = 0
    Public iFPTimeOut As Integer = 0
    Public bFPWaiting As Boolean = False
    Public iTimeout As Integer = 9
    Public iAttemptCount As Integer = 0
    Public bSimQueue As Boolean = False
    Public bIssueCard As Boolean = False
    Public bIssuePin As Boolean = False
    Public strActiveTimer As String = ""
    Public bShowOCRError As Boolean = False
    Public bShowCheckScanError As Boolean = False
    Public bShowPinMismatchError As Boolean = False
    Public bPrePaidOnly As Boolean = False
    Public bCheckCaptured As Boolean = False
    Public bSimCheckVerify As Boolean = False
    Public bLost As Boolean = False
    Public bWrongCard As Boolean = False
    Public bCardManagement As Boolean = False
    Public bPrintedReg As Boolean = False
    Public bResumeReg As Boolean = False
    Public bCustExists As Boolean = False
    Public bVPSecondTry As Boolean = False
    Public bScrapDragon As Boolean = False
    Public bPinOK As Boolean = False
    Public bCheckCash As Boolean = False
    Public bEnteringPin As Boolean = False
    Public bEPSecondTry As Boolean = False
    Public Property Reloadable = False
    Public Property CustACK As Boolean = False
    Public Property EPSecondTry As Boolean = False
    Public Property EnteringPin As Boolean = False
    Public Property CheckCash As Boolean = False
    Public Property PinOK As Boolean = False
    Public Property ScrapDragon As Boolean = False
    Public Property VPSecondTry As Boolean = False
    Public Property CustomerExists As Boolean = False

    Public Property ResumeReg As Boolean
        
    Public Property PrintReg As Boolean
       

    Public Property CardManagement As Boolean
       
    Public Property WrongCard As Boolean
       
    Public Property Lost As Boolean
        

    Public Property SimCheckVerify As Boolean
       
    Public Property CheckCaptured As Boolean

    Public Property FPWaiting As Boolean

    Public Property FPTimeOut As Integer

    Public Property PrePaidOnly As Boolean

    Public Property ShowPinMismatchError As Boolean

    Public Property ActiveTimer As String = ""

    Public Property IssueCard As Boolean

    Public Property IssuePin As Boolean
 
    Public Property SimQueue As Boolean

    Public Property AttemptCount As Integer

    Public Property CheckReviewed As Boolean
 
    Public Property TimeOutSecs As Integer

    Public Property LongTimeOut As Integer

    Public Property ShortTimeOut As Integer

    Public Property BackGroundCheck As Boolean

    Public Property TestCode As String

    Public Property SimCardReader As Boolean

    Public Property SimCheckScanner As Boolean
 

    Public Property SimSnapShell As Boolean
    
    Public Property SimFPReader As Boolean
  
    Public Property SimPinPad As Boolean
   
    Public Property TestType As String
      
    Public Property TestReview As Boolean
 
    Public Property Ack As Boolean
 
    Public Property Activity As Boolean
  
    Public Property TimeOut As Boolean

    Public Property Clear As Boolean
   
    Public Property Cancel As Boolean

    Public Property EOT As Boolean

    Public Property TimeLeft As Integer
  
    Public Property WorkstationID As Integer

    Public Property PhotoIDMatches As Boolean

    Public Property TestMode As Boolean

    Public Property SimulateAll As Boolean

    Public Property OnSecondPass As Boolean

    Public Property Enter As Boolean
 
    Public Property Reviewed As Boolean

    Public Property OnSecondPin As Boolean
     
    Public Property FingerPrintMatch As Boolean
     
    Public Property ReRegister As Boolean

    Public Property Register As Boolean
   
    Public Property ImageVerify As Boolean

    Public Property PrePaidBalUpdated As Boolean


    Public Property CheckAltered As Boolean
  
    Public Property CheckSigned As Boolean
      
    Public Property PayeeVerified As Boolean
        
    Public Property InPositiveFile As Boolean
       
    Public Property CurrentInputLength As Integer
       
    Public Property ShowOCRError As Boolean
        
    Public Property ShowCheckScanError As Boolean
      

    Public bWaitingScreen As Boolean = False
    Public bBuildSelectLang As Boolean = False
    Public bSelectPayment As Boolean = False
    Public bSignCheck As Boolean = False
    Public bScanningID As Boolean = False

    Public Property ScanningID As Boolean
      
    Public Property SignCheck As Boolean
       
    Public Property SelectPayment As Boolean
       
    Public Property BuildSelectLang As Boolean
       
    Public Property WaitingScreen As Boolean
   
    Public Property MultiCheck As Boolean = False
    Public Property StartPhoto As Boolean = False
    Public Property EndCheckCash As Boolean = False
    Public Property QueueTimeOut As Integer = 0
    Public Property HMILoadable As String = ""
    Public Property HMINonloadable As String = ""
    Public Property CheckScanErrCount As Integer = 0
    Public Property CheckReviewedTimeOut As Boolean = False
    Public Property SimScrapYard As Integer = False
    Public Property bOnFirstPin As Boolean = False
    Public Sub NewScreenFlags()
        bScanningID = False
        bWaitingScreen = False
        bBuildSelectLang = False
        bSelectPayment = False
        bSignCheck = False
        StartPhoto = False
    End Sub
    Public Sub New()

    End Sub
    Public Sub New(ByVal oLog As clsLog)
        Try
            NewScreenFlags()
            HMILoadable = oCheckCashing.GetSystemSettings("HMILoadable")
            HMINonloadable = oCheckCashing.GetSystemSettings("HMINonloadable")

            BackGroundCheck = False
            EndCheckCash = False
            MultiCheck = False
            bFpSensorMatch = False
            bSecondPin = False
            bReviewed = False
            bEnter = False
            bSecondPass = False
            bSim = False
            bSimSnapShell = False
            bSimCheckScanner = False
            bSimCardRW = False
            bSimFPReader = False
            bSimPinPad = False
            bDataOnly = False
            bOnFirstPin = False
            bPayeeVerified = False
            bCheckSigned = False
            bCheckAltered = False
            bCheckReviewed = False
            bPrePaidBalUpdated = False
            bImageVerify = False
            bInPositiveFile = False
            bPhotoIDMatches = False
            iRegister = False
            bRereg = False
            strTestType = ""
            iWKSID = 0
            iTime = 0
            bCancel = False
            bClear = False
            bTimeOut = False
            bEOT = False
            bActivity = False
            bAck = False
            iCurrLen = 0
            strTestCode = ""
            bTestReview = False
            bBackGroundOK = False
            iShortTimeOut = 0
            iLongTimeOut = 0
            iFPTimeOut = 0
            bFPWaiting = False
            iTimeout = 9
            iAttemptCount = 0
            bSimQueue = False
            bIssueCard = False
            bIssuePin = False
            strActiveTimer = ""
            bShowOCRError = False
            bShowCheckScanError = False
            bShowPinMismatchError = False
            bPrePaidOnly = False
            bCheckCaptured = False
            bSimCheckVerify = False
            bLost = False
            bWrongCard = False
            bCardManagement = False
            bPrintedReg = False
            bResumeReg = False
            bCustExists = False
            bVPSecondTry = False
            bScrapDragon = False
            bPinOK = False
            bCheckCash = False
            bEnteringPin = False
            bEPSecondTry = False
            WorkstationID = oCheckCashing.GetWorkstationID(GetIP)
            If WorkstationID = 0 Then
                oLog.LogMsg("Workstation not found in Workstation_Device")
                Throw New Exception("Workstation not found in Workstation_Device for IP" + GetIP())
            End If
            oLog.LogMsg("Workstation=" + WorkstationID.ToString)
            oLog.LogMsg("Workstation IP=" + GetIP())
            
        Catch ex As Exception
            Throw (ex)
        End Try
    End Sub
    Public Function GetIP() As String
        Dim ipAddresses() As IPAddress = Dns.GetHostAddresses(Dns.GetHostName)
        Dim strIP As String = ""
        For Each ip In ipAddresses
            If ip.AddressFamily = Sockets.AddressFamily.InterNetwork Then
                For Each b As Byte In ip.GetAddressBytes()
                    strIP += b.ToString()
                Next
            End If
            If strIP <> "" Then
                Return strIP
            End If
        Next
    End Function
    Public Sub SetDataReceivedFlags()
        Enter = False
        Activity = True
    End Sub

    Public Sub SetCancelFlags()
        Cancel = False
        TimeOut = False
    End Sub

    Public Sub SetEnterFlags()
        Enter = True

        Activity = False
    End Sub
    Public Sub SetAfterEnterFlags()
        Enter = False
        CurrentInputLength = 0
        Activity = False
    End Sub
    Public Function Clone() As clsFlags
        Return Me
    End Function
    Public Property WaitForOCR
    Public Property OnIdle = True
    'Build Screens Flags
    Public Property BuildClearTextScreen = False
    Public Property BUildPinScreen = False
    Public Property BuildCheckInfoScreen = False
    Public Property BuildCannotCash = False
    Public Property BuildChooseCashCheckorReRegister = False
    Public Property BuildErrScreen = False
    Public Property BuildStartScreen = False
    Public Property BuildFinScreen = False
    Public Property BuildComeBackLater = False
    Public Property BuildEnterDOB = False
    Public Property BuildPhotoIDScreen = False
    Public Property BuildSelectLangScreen = False
    Public Property BuildIDCard = False
    Public Property BuildPleaseWait = False
    Public Property BuildSelectPayment = False
    Public Property BuildSelectREgistration = False
    Public Property BuildSignCheck = False
    Public Property BuildSTartCheckCash = False
    Public Property BUildSwipeCard = False
    Public Property BuildEnterMobileNum = False
    Public Property BuildEnterCoPhone = False
    Public Property BuildEnterSSN = False
    Public Property BuildHaveSSN = False
    Public Property BuildGetCash = False
    Public Property BuildTestScreen = False
    Public Property BuildTestHeavyMetalRegistration = False



End Class
