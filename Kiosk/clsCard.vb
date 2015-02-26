Imports System.Globalization
Imports System
Imports System.Xml

Imports System.Configuration

Public Class clsCard
    Public Property oFlags As clsCard.clsCardFlags
    Public Property oErr As clsError
    Public Property KSN As String
    Public Property KSN2 As String
    Dim strType As String = ""
    Dim strTrack1 As String = ""
    Dim strTrack2 As String = ""
    Dim strTrack3 As String = ""
    Dim strTrack3Rem As String = ""
    Dim strPin As String = ""
    Dim strPan As String = ""
    Dim oCheckCashing As New clsDataModule.clsInterface
    Dim strPrevCustomerID As String = ""
    Dim strPIN2 As String = ""
    Dim dExpire As Date
    Dim oLang As New clsLanguage()
    Public oCustomer As clsCustomer
    Public Property ActID As Integer = 0
    Public Property AccountNumber As String = ""
    Public Property CardID As Integer = 0
    Public Property PANNotEncrypted As String = ""
    Public Property Barcode As Integer = 0
    Public Property ExpDate As Date
        Get
            Return dExpire
        End Get
        Set(ByVal value As Date)
            dExpire = value
        End Set
    End Property
    Public Property PIN2 As String
        Get
            Return strPIN2
        End Get
        Set(ByVal value As String)
            strPIN2 = value
        End Set
    End Property
    Public Property TransactCardType As String
        Get
            Return strType
        End Get
        Set(ByVal value As String)
            strType = value
        End Set
    End Property
    Public Property PreviousCustID As String
        Get
            Return strPrevCustomerID
        End Get
        Set(ByVal value As String)
            strPrevCustomerID = value
        End Set
    End Property
    Public Property PAN As String
        Get
            Return strPan
        End Get
        Set(ByVal value As String)
            strPan = value
        End Set
    End Property
    Public Property PIN As String
        Get
            Return strPin
        End Get
        Set(ByVal value As String)
            strPin = value
        End Set
    End Property
    Public Property Track3 As String
        Get
            Return strTrack3
        End Get
        Set(ByVal value As String)
            strTrack3 = value
        End Set
    End Property
    Public Property Track2 As String
        Get
            Return strTrack2
        End Get
        Set(ByVal value As String)
            strTrack2 = value
        End Set
    End Property
    Public Property Track1 As String
        Get
            Return strTrack1
        End Get
        Set(ByVal value As String)
            strTrack1 = value

        End Set
    End Property
    Public Property Track3Rem As String
        Get
            Return strTrack3Rem
        End Get
        Set(ByVal value As String)
            strTrack3Rem = value
        End Set
    End Property
    Public Property CompanyNbr As Integer = 0
    Public Function ProcessCard(ByRef oCustomer As clsCustomer) As Integer

        If oFlags.SimCardReader Then oFlags.TestType = oCheckCashing.GetTestData("TransactionType", 0)
        If oFlags.SimCardReader And oFlags.TestType = "" Then
            Return 0
        End If
        While oFlags.TestType = "SDR"
            Dim i As Integer = 0
            Dim out1, out2 As Integer
            Dim rc As Integer = 0
            Dim rcBackGrd As String = ""
            Dim rcCard As Integer = 0
            Dim ocheckService As New CheckServices.CheckServices()
            Dim netCard As Decimal = Decimal.Parse(oCheckCashing.GetTestData("NetCard", 0))
            Dim syTranID As Integer = oCheckCashing.GetRandomNumber(3, 999)
            Dim strPhoto As String = oCheckCashing.GetTestData("PhotoIDNumber", 0)
            Dim strState As String = oCheckCashing.GetTestData("StateCode", 0)
            Dim dDob As Date = oCheckCashing.GetTestData("DOB", 0)
            Dim strPan As String = Left(oCheckCashing.GetTestData("PAN", 0), 16)
            Dim iType As Integer = Int32.Parse(oCheckCashing.GetTestData("SDIDType", 0))
            Dim strPrefix As String = oCheckCashing.GetTestData("Surname", 0)
            Dim strCounty As String = oCheckCashing.GetTestData("County", 0)
            Dim strIssueDate As String = oCheckCashing.GetTestData("IssueDate", 0)
            Dim strExpirationDate As String = oCheckCashing.GetTestData("ExpirationDate", 0)
            Dim strEye As String = oCheckCashing.GetTestData("Eye", 0)
            Dim strHair As String = oCheckCashing.GetTestData("Hair", 0)
            Dim strFirst As String = oCheckCashing.GetTestData("First", 0)
            Dim strLast As String = oCheckCashing.GetTestData("Last", 0)
            Dim strMiddle As String = oCheckCashing.GetTestData("Middle", 0)
            Dim strAddr As String = oCheckCashing.GetTestData("Address1", 0)
            Dim strAddr2 As String = oCheckCashing.GetTestData("Address2", 0)
            Dim strCity As String = oCheckCashing.GetTestData("City", 0)
            Dim strZip As String = oCheckCashing.GetTestData("Zip", 0)
            Dim strHeight As String = oCheckCashing.GetTestData("Height", 0)
            Dim strweight As String = oCheckCashing.GetTestData("Weight", 0)
            Dim strSex As String = oCheckCashing.GetTestData("Sex", 0)
            Dim strSSN As String = oCheckCashing.GetTestData("SSN", 0)
            Dim strMobile As String = oCheckCashing.GetTestData("Mobile", 0)
            Dim strPin1 As String = oCheckCashing.GetTestData("PinBlock1", 0)
            Dim strPin2 As String = oCheckCashing.GetTestData("PinBlock2", 0)
            Dim Password As String = oCheckCashing.GetTestData("Password", 0)
            ocheckService.Timeout = System.Configuration.ConfigurationManager.AppSettings("WebServiceTimeout")
            'ocheckService.HMRHeavyMetalRegistration(strPhoto, dDob, strFirst, strMiddle, strLast, strAddr, strAddr2, strCity, strState, strZip, oCustomer.EmailAddress, 4566, "", "", 0, "", rcCard)
            'ocheckService.HMCHeavyMetalCheckCashing(PAN, "x")
           oCheckCashing.UpdateCaseNum()
            oFlags.TestType = oCheckCashing.GetTestData("TransactionType", 0)
        End While
        While oFlags.TestType = "SDC"
            Dim i As Integer = 0
            Dim out1, out2 As Double
            Dim rc As Integer = 0
            Dim rc1 As String = ""
            Dim rc2 As String = ""
            Dim rc3 As Integer = 0
            Dim ocheckService As New CheckServices.CheckServices()
            Dim netCard As Decimal = Decimal.Parse(oCheckCashing.GetTestData("NetCheck", 0))
            Dim syTranID As Integer = oCheckCashing.GetRandomNumber(3, 999)
            Dim strPhoto As String = oCheckCashing.GetTestData("PhotoID", 0)
            Dim strState As String = oCheckCashing.GetTestData("State", 0)
            Dim dDob As Date = oCheckCashing.GetTestData("DOB", 0)
            Dim strPan As String = Left(oCheckCashing.GetTestData("PAN", 0), 16)
            Dim strType As String = oCheckCashing.GetTestData("Type", 0)
            Dim strRoute As String = oCheckCashing.GetTestData("RouteNum", 0)
            Dim strCheck As String = oCheckCashing.GetTestData("CheckNum", 0)
            Dim strAccount As String = oCheckCashing.GetTestData("Account", 0)
            Dim dCheck As String = oCheckCashing.GetTestData("CheckDate", 0)
            Dim cPrint As Char = oCheckCashing.GetTestData("PrintCode", 0)
            'ocheckService.HeavyMetalPositiveCheck(strType, syTranID, netCard, strRoute, strAccount, strCheck, dCheck, cPrint.ToString(), strPhoto, dDob, strState, System.Configuration.ConfigurationManager.AppSettings("ProviderID"), rc, rc1, rc2)
            'oJourn.SaveJournal(oCustomer.CustomerID, Date.Now.ToString, oCustomer.oCheck.BlockID, oCustomer.oCheck.TransactionNumber, "K0", "KIOSK", "Scrap Pos Check", 0, "rc=" + rc.ToString() + " rc1=" + rc1.ToString() + " rc2=" + rc2.ToString())
            oCheckCashing.UpdateCaseNum()
            oFlags.TestType = oCheckCashing.GetTestData("TransactionType", 0)
        End While
        While oFlags.TestType = "SDP"
            Dim i As Integer = 0
            Dim out1 As Double = 0
            Dim out2 As Double = 0
            Dim rc1 As Integer = 0
            Dim rc2 As String = ""
            Dim rc3 As String = ""
            Dim rcCard As String = ""
            Dim ocheckService As New CheckServices.CheckServices()
            Dim netCard As Decimal = Decimal.Parse(oCheckCashing.GetTestData("NetCard", 0))
            Dim syTranID As Integer = oCheckCashing.GetRandomNumber(3, 999)
            Dim strPhoto As String = oCheckCashing.GetTestData("PhotoIDNumber", 0)
            Dim strState As String = oCheckCashing.GetTestData("State", 0)
            Dim dDob As Date = oCheckCashing.GetTestData("DOB", 0)
            Dim strPan As String = Left(oCheckCashing.GetTestData("PAN", 0), 16)

            Dim strType As String = oCheckCashing.GetTestData("Type", 0)
            Dim strRoute As String = oCheckCashing.GetTestData("RouteNum", 0)
            Dim strCheck As String = oCheckCashing.GetTestData("CheckNum", 0)
            Dim strAccount As String = oCheckCashing.GetTestData("Account", 0)
            Dim dCheck As String = oCheckCashing.GetTestData("CheckDate", 0)
            Dim cPrint As Char = oCheckCashing.GetTestData("PrintCode", 0)
            Dim iTest As Integer = 1
            Dim OutCardBal As Decimal = 0
            Dim OutCardAvailable As Decimal = 0
            'ocheckService.HeavyMetalPrePaid("P", syTranID.ToString(), netCard, strPhoto,
            'strState,
            'dDob,
            'strPan,
            'ConfigurationManager.AppSettings("ProviderID"),
            'OutCardBal,
            'OutCardAvailable,
            '0,
            'rc1,
            'rcCard)

            oCheckCashing.UpdateCaseNum()
            oFlags.TestType = oCheckCashing.GetTestData("TransactionType", 0)
        End While
        If oFlags.SimCardReader Then
            If PAN = "" Then
                PAN = oCheckCashing.GetTestData("PAN", 0)
            End If
        End If

        If PAN = "" Then
            If Not oFlags.TestMode Then
                oErr.ErrCode = 1
            Else
                Return 10
            End If
            Return -1
        End If

        AccountNumber = PANNotEncrypted.Substring(8, PANNotEncrypted.Length - 8)
        AccountNumber = AccountNumber.PadLeft(18, "0")



        If oCustomer.GetCustomerDataFromPAN(PAN) Then
            If TransactCardType = "XXX" Then
                oErr.ErrCode = 113
                oErr.GetErrorData()
            ElseIf CardID > 0 Then
                oFlags.Register = False
                oFlags.ScrapDragon = False
                Return 1
            ElseIf (oCustomer.RegSource = "SD" And oCustomer.RegFlag = "PR") Then
                oFlags.Register = True
                oFlags.CardManagement = False
                oFlags.ScrapDragon = True
                Return 1
            ElseIf (oCustomer.RegSource = "CM" And oCustomer.RegFlag = "PR") Then
                oFlags.Register = True
                oFlags.ScrapDragon = False
                oFlags.CardManagement = True
                Return 1
            Else
                oFlags.Register = False
                oFlags.ScrapDragon = False
                Return 1
            End If
        Else
            oFlags.ReRegister = False
            oFlags.Register = True
            If oCustomer.GetRegistrationDataFromPAN(PAN) Then
                oFlags.ResumeReg = True
                Return 2
            Else
                Return 2
            End If
        End If

    End Function
    Public Function ValidateBin() As Integer
        Dim strType As String = ""
        Dim iBin As Integer = Int32.Parse(PAN.Substring(0, 7))
        Return oCheckCashing.ValidateBin(iBin, oCustomer.CustomerID)

    End Function

    Public Sub New()
        ' oJourn.Url = ConfigurationManager.AppSettings("SaveJournalURL")


    End Sub
    Public Sub New(ByVal pWorkstationID As Integer, ByVal CustomerExists As Boolean, ByRef pErr As clsError)
        'oJourn.Url = ConfigurationManager.AppSettings("SaveJournalURL")

        oFlags = New clsCard.clsCardFlags(pWorkstationID)
        oErr = pErr.Clone()
    End Sub
    Public Class clsCardFlags
        Public Property SimCardReader As Boolean = False
        Public Property TestType As String = ""
        Public Property TestMode As Boolean = False
        Public Property Register As Boolean = False
        Public Property ScrapDragon As Boolean = False
        Public Property CardManagement As Boolean = False
        Public Property ReRegister As Boolean = False
        Public Property ResumeReg As Boolean = False
        Public Property WorkstationID As Integer = 0
        Public Sub New(ByVal pWorkstationID As Integer)
            WorkstationID = pWorkstationID
            Dim oCheckCashing As New clsDataModule.clsInterface()

            TestMode = CType(oCheckCashing.GetKioskSettings("TestMode", WorkstationID), Integer)
            SimCardReader = CType(oCheckCashing.GetKioskSettings("SimCardReader", WorkstationID), Integer)
        End Sub
        Public Sub New()

        End Sub
    End Class
End Class
