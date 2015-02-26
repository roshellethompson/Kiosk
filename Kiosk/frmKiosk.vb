Imports System.Configuration
Imports System.Xml
Imports System.Diagnostics.Process
Imports System.Reflection
Imports System.IO
Imports System



Public Class frmKiosk


    Dim oFlags As New clsFlags
    Public Shared oPrint As clsPrinter
    Dim oLog As New clsLog("TransLog.txt")
    Public oQData As clsQueue
    Public oErr As clsError
    Dim oJourn As New SaveJournal.Journal
    Public Property oAffiliate As New clsAffiliate
    Dim oOCR As New OCRScan.Service
    Public oCheckCashing As New clsDataModule.clsInterface
    Public oChkService As New CheckService.ICheckServiceservice
    Public oCheckService As New CheckServices.CheckServices
    Public iInfo As New CheckService.TLangResult
    Public Shared WithEvents lblErr As New Label
    Dim lblFinish As New Label
    Friend WithEvents tmrReg As New System.Windows.Forms.Timer
    Friend WithEvents tmrTimeOut As New System.Windows.Forms.Timer
    Friend WithEvents tmrCheckReviewed As New System.Windows.Forms.Timer
    Dim oCustomer As clsCustomer

    ''' <summary>
    ''' Select the english language
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ChooseEnglish()
        Try
            oLog.LogMsg("Button English clicked on select lang screen...")
            oCustomer.oLang.LanguageName = "English"



            oCustomer.FillCaptions()
            oFlags.TimeLeft = oFlags.TimeOutSecs
            oFlags.BuildPhotoIDScreen = True

        Catch ex As Exception
            oLog.LogMsg(ex.ToString)
            frmKiosk.lblErr.Text = ex.Message
            ProcessCancel()
        End Try
    End Sub
    ''' <summary>
    ''' Select French
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ChooseFrench()
        Try
            oLog.LogMsg("French is language")
            oCustomer.oLang.LanguageName = "French"
            oCustomer.FillCaptions()
            oFlags.TimeLeft = oFlags.TimeOutSecs
            oFlags.BuildPhotoIDScreen = True
        Catch ex As Exception
            oLog.LogMsg(ex.ToString)
            frmKiosk.lblErr.Text = ex.Message
            ProcessCancel()
        End Try
    End Sub
    ''' <summary>
    ''' Select Spanish
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ChooseSpanish()
        Try
            oLog.LogMsg("Language is Spanish")
            oCustomer.oLang.LanguageName = "Spanish"
            oCustomer.FillCaptions()
            oFlags.BuildPhotoIDScreen = True
        Catch ex As Exception
            oLog.LogMsg(ex.ToString)
            frmKiosk.lblErr.Text = ex.Message
            ProcessCancel()
        End Try
    End Sub


    ''' <summary>
    ''' Capture the photo and id scan, build the mobile phone capture
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CapturePhotoID()
        Try
            If oFlags.ReRegister Then
                oCustomer.RegType = "N"
            ElseIf oFlags.Lost Then
                oCustomer.RegType = "L"
            ElseIf oFlags.CardManagement Then
                oCustomer.RegType = "C"
            ElseIf oFlags.ScrapDragon Then
                oCustomer.RegType = "S"
            Else
                oCustomer.RegType = "R"
            End If
            If Not oFlags.SimSnapShell Then
                Dim oRet As Integer = oChkService.CaptureRegIDData(oFlags.WorkstationID, oCustomer.RegType.ToString, oCustomer.oCard.PAN, oCustomer.ScanID)
                oLog.LogMsg("Button Start Scan clicked to scan id and take photo, ScanID=" + oCustomer.ScanID.ToString)
                If oRet = 1 Then
                    oFlags.TimeLeft = oFlags.ShortTimeOut
                    oErr.ErrCode = 32
                    ProcessCancel()
                    Exit Sub
                End If
                oRet = oChkService.CaptureRegIDImage(oFlags.WorkstationID, oCustomer.ScanID)
                If oRet = 1 Then
                    oFlags.TimeLeft = oFlags.ShortTimeOut
                    oErr.ErrCode = 32
                    ProcessCancel()
                    Exit Sub
                End If
                oRet = oChkService.CaptureRegCustImage(oFlags.WorkstationID, oCustomer.ScanID)
                If oRet = 1 Then
                    oFlags.TimeLeft = oFlags.ShortTimeOut
                    oErr.ErrCode = 33
                    ProcessCancel()
                    Exit Sub
                End If
            End If

            If oFlags.SimSnapShell Then
                oCustomer.ScanID = 0
                oCustomer.ScanID = oCheckCashing.InsRegReview(oCustomer.ScanID, 5, 0, oCheckCashing.GetTestData("First", 0), oCheckCashing.GetTestData("Last", 0), oCheckCashing.GetTestData("DOB", 0), _
                                    oCheckCashing.GetTestData("Middle", 0), oCheckCashing.GetTestData("StateCode", 0), oCheckCashing.GetTestData("PhotoIDType", 0), oCheckCashing.GetTestData("IssueDate", 0), _
                                    oCheckCashing.GetTestData("ExpirationDate", 0), oCheckCashing.GetTestData("PhotoIDNumber", 0), oCheckCashing.GetTestData("Address1", 0), _
                                    oCheckCashing.GetTestData("Address2", 0), oCheckCashing.GetTestData("City", 0), oCheckCashing.GetTestData("Zip", 0), oCheckCashing.GetTestData("Height", 0), oCheckCashing.GetTestData("Weight", 0), _
                                    "", "", "", "Review", 0, 0, Date.Now, "", oCheckCashing.GetTestData("Sex", 0), oCustomer.RegType.ToString, 0, oCustomer.oCard.PAN)
                oCustomer.GetRegistrationDataFromPAN(oCustomer.oCard.PAN)
            End If

            oLog.LogMsg("Capture Reg ID Data called...")

            If oFlags.ReRegister = False Then
                oFlags.ActiveTimer = "MobileNum"
                BuildStartClearTimer()
            Else
                oFlags.BuildPleaseWait = True
                oFlags.TimeLeft = oFlags.LongTimeOut
            End If
        Catch ex As Exception
            oLog.LogMsg(ex.ToString)
            frmKiosk.lblErr.Text = ex.Message
            ProcessCancel()
        End Try
    End Sub

    ''' <summary>
    ''' Register New PhotoID start
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RegisterNewPhotoID()
        Try
            oLog.LogMsg("Registering new photoID start")
            oFlags.ReRegister = True
            oFlags.CheckCash = False
            oFlags.BuildPhotoIDScreen = True
        Catch ex As Exception
            oLog.LogMsg(ex.ToString)
            frmKiosk.lblErr.Text = ex.Message
            ProcessCancel()
        End Try
    End Sub
    ''' <summary>
    ''' After the check scan, Capture photo for check cashing, insert to db for check review
    ''' </summary>
    '''
    ''' <remarks></remarks>
    Public Sub CaptureCheckCashPhoto()
        Try
            oLog.LogMsg("Button start photo for check cash clicked...")
            If Not oFlags.SimSnapShell Then
                oChkService.CaptureCustImage(oCustomer.CustomerID, oCustomer.oCheck.BlockID, oFlags.WorkstationID)
                oLog.LogMsg("Capture Reg Cust Image called...")
            Else
                oFlags.TestReview = True
            End If
            Dim strXML As String = oCheckService.CheckReviewXML(oCustomer.CustomerID, oCustomer.oCheck.TransactionNumber, oCustomer.oCheck.BlockID, oFlags.WorkstationID, oCustomer.oCheck.RouteNumber, oCustomer.oCheck.AccountNumber, _
                                                                oCustomer.FirstName, oCustomer.LastName, 0, Date.Now, "", False, "Y", oCustomer.oCheck.CheckNumber.ToString())
            oLog.LogMsg("Called CheckReviewXML " + "oCustomer.CustomerID=" + CType(oCustomer.CustomerID, String) + ", TranNum=" + oCustomer.oCheck.TransactionNumber.ToString + ", Block=" + oCustomer.oCheck.BlockID.ToString _
                   + ", RouteNum=" + oCustomer.oCheck.RouteNumber + ", AccountNum=" + oCustomer.oCheck.AccountNumber + ", First=" + oCustomer.FirstName + ", Last=" + oCustomer.LastName)
            Dim xmlRes As New XmlDocument
            xmlRes.LoadXml(strXML)
            Dim xmlElem As XmlElement = xmlRes.FirstChild
            Dim iRes As Integer = CType(xmlElem.FirstChild.FirstChild.InnerText, Integer)
            If iRes <> 0 Then
                oLog.LogMsg("Check Review Reports Error: " + iRes.ToString)
                oErr.ErrCode = 9
                ProcessCancel()
                Exit Sub
            Else
                oLog.LogMsg("Check Review Reports Success...")
            End If
            oFlags.TimeLeft = oFlags.LongTimeOut
            oFlags.BuildPleaseWait = True
        Catch ex As Exception
            oLog.LogMsg(ex.ToString)
            frmKiosk.lblErr.Text = ex.Message
            ProcessCancel()
        End Try
    End Sub
    ''' <summary>
    ''' Start the process by analyzing the pan
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Start(ByVal Pan As String)
        Try
            oLog.LogMsg("Button Start clicked...")
            oCheckCashing.PrepareTestData()
            oFlags.OnIdle = False

            If oFlags.SimCardReader Then
                Pan = oCheckCashing.GetTestData("PAN", 0)
            End If
            'ProcessCard(Pan)

        Catch ex As Exception
            oCheckCashing.PrepareTestData()
            InitTimers()
            oLog.LogMsg(ex.ToString)
            frmKiosk.lblErr.Text = ex.Message
            ProcessCancel()
        End Try
    End Sub

    ''' <summary>
    ''' Exit with Card and/or cash
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Finish()
        Try
            oFlags.MultiCheck = False
            oLog.LogMsg("Button go clicked in check cashing process...")
            If oFlags.SelectPayment Then
                oLog.LogMsg("Authorize Check being called  ")
                oLog.LogMsg("CheckNum= " + oCustomer.oCheck.CheckNumber)
                oLog.LogMsg("RouteNum=" + oCustomer.oCheck.RouteNumber)
                oLog.LogMsg("Acctnum=" + oCustomer.oCheck.AccountNumber)
                oLog.LogMsg("CustID=" + oCustomer.CustomerID.ToString)
                oLog.LogMsg("Amt=" + oCustomer.oCheck.CheckAmount.ToString)
                oLog.LogMsg("Date=" + oCustomer.oCheck.CheckDate.ToString)
                oLog.LogMsg("Compname=" + oCustomer.oCheck.CompanyName)
                oLog.LogMsg("AuthCode=" + oCheckCashing.GetTestData("PayeeAuthorizationCode", 0).ToString)
                oLog.LogMsg("Company Phone=" + oCustomer.oCheck.CompanyPhone)
                oLog.LogMsg("CompID=" + oCustomer.oCheck.CompanyID.ToString)
                oLog.LogMsg("TranNum=" + oCustomer.oCheck.TransactionNumber.ToString)
                oLog.LogMsg("StoreID=" + oCustomer.oCheck.StoreID.ToString)
                oLog.LogMsg("First=" + oCustomer.FirstName)
                oLog.LogMsg("Last=" + oCustomer.LastName)
                oLog.LogMsg("DOB=" + oCustomer.oCheck.DOB)
                oLog.LogMsg("Zip=" + oCustomer.oCheck.Zip)
                oLog.LogMsg("Guarantee Code=" + oCustomer.oCheck.GuaranteeCode)
                oLog.LogMsg("Approval Code=" + oCustomer.oCheck.ApprovalCode)

                If AuthorizeCheck() Then
                    oPrint.oCustomer = oCustomer.Clone
                    oPrint.PrintReceipt()
                End If
                ProcessCancel()
                Exit Sub
            End If
            If Not oFlags.PrePaidOnly Then
                oFlags.BuildSelectPayment = True
            End If
        Catch ex As Exception
            oLog.LogMsg(ex.ToString)
            frmKiosk.lblErr.Text = ex.Message
            ProcessCancel()
        End Try
    End Sub


    ''' <summary>
    ''' Pre check scan.  Continues to CaptureCheckCashPhoto. If on 2nd or more checks, check review inserted
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub PreScanCheck(ByRef oCustomer As clsCustomer, ByRef oLog As clsLog, ByRef oErr As clsError, ByRef oFlags As clsFlags)
        Try
            oLog.LogMsg("Button Scan Check clicked...")
            If oFlags.MultiCheck Then
                oCustomer.oCheck = New clsCheck(oCustomer.oCheck.BlockID, oCustomer.oCheck.TransactionNumber, oCustomer.CustomerID, oFlags.WorkstationID)
            End If
            If oFlags.SimCheckScanner Then
                oCustomer.oCheck.RouteNumber = oCheckCashing.GetTestData("RoutingNumber", 0)
                oCustomer.oCheck.AccountNumber = oCheckCashing.GetTestData("AccountNumber", 0)
                oCustomer.oCheck.CheckNumber = oCheckCashing.GetTestData("CheckNumber", 0)
            End If
            If (oCustomer.oCheck.RouteNumber.ToString = "") Then
                oCustomer.oCheck.RouteNumber = "0"
            End If
            If (oCustomer.oCheck.AccountNumber.ToString = "") Then
                oCustomer.oCheck.AccountNumber = "0"
            End If
            If (oCustomer.oCheck.CheckNumber.ToString = "") Then
                oCustomer.oCheck.CheckNumber = "0"
            End If
            If Not oFlags.MultiCheck Then
                oFlags.BuildSTartCheckCash = True
                CaptureCheckCashPhoto()
            Else
                If oFlags.TestMode Then
                    oFlags.TestReview = True
                End If
                Dim strXML As String = oCheckService.CheckReviewXML(oCustomer.CustomerID, oCustomer.oCheck.TransactionNumber, oCustomer.oCheck.BlockID, oFlags.WorkstationID, oCustomer.oCheck.RouteNumber, oCustomer.oCheck.AccountNumber, _
                                                                oCustomer.FirstName, oCustomer.LastName, 0, Date.Now, "", False, "Y", Int32.Parse(oCustomer.oCheck.CheckNumber))
                oLog.LogMsg("Called CheckReviewXML " + "oCustomer.CustomerID=" + CType(oCustomer.CustomerID, String) + ", TranNum=" + oCustomer.oCheck.TransactionNumber.ToString + ", Block=" + oCustomer.oCheck.BlockID.ToString _
                       + ", RouteNum=" + oCustomer.oCheck.RouteNumber + ", AccountNum=" + oCustomer.oCheck.AccountNumber + ", First=" + oCustomer.FirstName + ", Last=" + oCustomer.LastName)
                Dim xmlRes As New XmlDocument
                xmlRes.LoadXml(strXML)
                Dim xmlElem As XmlElement = xmlRes.FirstChild
                Dim iRes As Integer = CType(xmlElem.FirstChild.FirstChild.InnerText, Integer)
                If iRes <> 0 Then
                    oLog.LogMsg("Check Review Reports Error: " + iRes.ToString)
                    oErr.ErrCode = 9
                    ProcessCancel()
                    Exit Sub
                Else
                    oLog.LogMsg("Check Review Reports Success...")
                End If
                oFlags.TimeLeft = oFlags.LongTimeOut
                oFlags.BuildPleaseWait = True
            End If
        Catch ex As Exception
            oLog.LogMsg(ex.ToString)
            frmKiosk.lblErr.Text = ex.Message
            ProcessCancel()
        End Try
    End Sub
    ''' <summary>
    ''' Cancel process
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub ProcessCancel()
        Try

            oLog.LogMsg("Button cancel clicked card ejected...")

            Dim bReg As Boolean = False
            Dim bErrCase As Boolean = False
            bReg = (oFlags.Register And oFlags.ScrapDragon) Or (oFlags.Register And oFlags.IssuePin And oFlags.IssueCard) Or
                (oFlags.ReRegister And oFlags.PinOK And oFlags.ImageVerify And oFlags.PhotoIDMatches)
            bErrCase = (oFlags.VPSecondTry Or oFlags.WrongCard Or oFlags.ShowPinMismatchError Or oFlags.ShowOCRError Or oFlags.ShowCheckScanError)
            If Not bErrCase And bReg Then
                If Not oFlags.SimCardReader Then
                    'oCardRW.EjectCard()
                End If

                If oCustomer.GetCustomerDataFromID Then
                    oPrint.oCustomer = oCustomer.Clone
                    oPrint.PrintRegTicket()
                    oFlags = New clsFlags(oLog)
                    lblFinish.Text = oCustomer.oLang.GetScreenLabel("Register")
                    Exit Sub
                End If

            End If
            If oErr.ErrCode <> 0 Then
                lblErr.Visible = True
                oErr.GetErrorData()
                If oErr.ErrText = "" Then
                    oErr.ErrText = "#" + oErr.ErrCode.ToString
                    frmKiosk.lblErr.Text = oErr.ErrText
                End If
                oLog.LogMsg(lblErr.Text)
            End If
            If oErr.ErrCode = 0 And oFlags.CheckCash And oFlags.CheckCaptured Then
                If Not oFlags.MultiCheck Then
                    If oFlags.EndCheckCash Then
                        oFlags = New clsFlags(oLog)
                        lblFinish.Text = oCustomer.oLang.GetScreenLabel("EndCheckCash")
                    Else
                        frmKiosk.lblErr.Text = ""
                    End If
                    oFlags = New clsFlags(oLog)
                Else
                    If oFlags.EndCheckCash Then
                        oFlags = New clsFlags(oLog)
                        oFlags.MultiCheck = True
                        lblFinish.Text = oCustomer.oLang.GetScreenLabel("EndCheckCash")
                        oFlags.BuildFinScreen = True
                        Exit Sub
                    Else
                        frmKiosk.lblErr.Text = ""
                    End If
                    oCustomer.oFlags.CustomerExists = True
                    oCheckCashing.UpdateCaseNum()
                    Exit Sub
                End If
            End If
            If frmKiosk.lblErr.Text <> "" Then
                oFlags.BuildErrScreen = True
                Exit Sub
            End If
            StopTimers()
            If Not oFlags.SimCardReader Then
                'oCardRW.EjectCard()
            End If

            If oFlags.TestMode Then

                If lblFinish.Text <> "" Then
                    oFlags.BuildFinScreen = True
                    Exit Sub
                End If
                oCheckCashing.UpdateCaseNum()
                oCustomer = New clsCustomer(oFlags.WorkstationID)
                oFlags = New clsFlags(oLog)
            ElseIf oFlags.MultiCheck Then
                ChooseCashCheck()
            End If
            If lblFinish.Text <> "" Then
                oFlags.BuildFinScreen = True
                Exit Sub
            Else
                oFlags.BuildStartScreen = True
            End If


        Catch ex As Exception
            oLog.LogMsg(ex.ToString)
            frmKiosk.lblErr.Text = ex.Message
            oFlags.BuildErrScreen = True

        End Try
    End Sub

    ''' <summary>
    ''' Sets the appropriate flags for cashing a check, capture check cash photo is next
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub ChooseCashCheck()
        Try
            oLog.LogMsg("Button cash check clicked...")
            oFlags.ReRegister = False
            oFlags.Register = False
            oFlags.CheckCash = True
            oFlags.BuildSignCheck = True
        Catch ex As Exception
            oLog.LogMsg(ex.ToString)
            frmKiosk.lblErr.Text = ex.Message
            ProcessCancel()
        End Try
    End Sub
    Public Sub BuildPleaseWait()

        oFlags.WaitingScreen = True
        If oFlags.CheckCash Then
            tmrCheckReviewed.Enabled = True
        Else
            tmrReg.Enabled = True
        End If
    End Sub

    Public Sub buildSelectRegistration()
        oLog.LogMsg("Building Select Registration Screen...")
        If oFlags.SimCardReader Then
            ChooseToRegister()
        End If
        oFlags.TimeLeft = oFlags.TimeOutSecs
    End Sub
    Public Sub GetDOBInput()
        oFlags.ActiveTimer = "DOB"
        BuildStartClearTimer()
    End Sub
    Public Sub GetCoPhoneInput()
        oFlags.ActiveTimer = "COPhone"
        BuildStartClearTimer()
    End Sub
    Public Sub GetSSNInput()
        oFlags.ActiveTimer = "SSN"
        BuildStartClearTimer()
    End Sub
    Public Sub GetCashInput()
        oFlags.ActiveTimer = "CASH"
        BuildStartClearTimer()
    End Sub
    Public Sub GetPinInput()
        If oFlags.Register Then
        Else
            oFlags.EnteringPin = True
        End If
        oQData = New clsQueue(oFlags.WorkstationID)
        oQData.CustomerID = oCustomer.CustomerID
        oQData.QueueCode = "EP"
        oQData.Data2 = oCustomer.oCard.CardID
        oQData.Data3 = "PN1"
        oQData.Data4 = "343"
        oQData.Data5 = oCustomer.oLang.LangID
        PostAndProcessQueue()
    End Sub

    Public Sub BuildEnterDOB()

        oFlags.BuildEnterDOB = True
    End Sub
    Public Sub BuildEnterSSN()

        oFlags.BuildEnterSSN = True
    End Sub
    Public Sub BuildEnterCoPhone()

        oFlags.BuildEnterCoPhone = True
    End Sub
    Public Sub buildEnterMobileNum()

        oFlags.BuildEnterMobileNum = True
    End Sub

    Public Sub GetMobileNumInput()
        oFlags.ActiveTimer = "MoblieNum"
        BuildStartClearTimer()
    End Sub
    ''' <summary>
    ''' A code from ErrorCodes table returns the cannot cash message
    ''' </summary>
    ''' <remarks></remarks>
    Public Function buildCannotCash(ByVal code As String) As String
        Try

            Dim strMsg As String = oCheckCashing.GetCheckCodeText(code)
            oFlags.TimeLeft = oFlags.ShortTimeOut
            If oFlags.TestMode Then
                ProcessCancel()
            End If
            Return strMsg
        Catch ex As Exception
            oLog.LogMsg(ex.ToString)
            frmKiosk.lblErr.Text = ex.Message
            ProcessCancel()
        End Try
    End Function
    Public Sub BuildErrScreen()
        lblErr.Location = New Point(25, 25)
        lblErr.Height = 300
        lblErr.Width = 640

        If lblErr.Text <> "" Then
            lblErr.Visible = True
        End If

        oFlags.TimeLeft = oFlags.ShortTimeOut
        InitTimers()

    End Sub
    ''' <summary>
    ''' Show NetAmt,Fee from cashed checks
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub buildCheckInfo()
        Try
            Dim rc1, rc2 As String
            rc1 = ""
            rc2 = ""
            oCustomer.oCheck.Fee = "0"
            oChkService.GetFee(oFlags.WorkstationID, oCustomer.CustomerID, oCustomer.oCheck.CheckAmount, "CHK", 0, oCustomer.oCheck.Fee, rc1, rc2)
            If rc1 <> "0" Or rc2 <> "0" Then
                oErr.ErrCode = 34
                ProcessCancel()
            End If
            oCustomer.oCheck.NetAmt = oCustomer.oCheck.CheckAmount - oCustomer.oCheck.Fee
            oCustomer.oCheck.PrePaidAmount = oCustomer.oCheck.NetAmt
            oLog.LogMsg("Building Check Information Screen...")
            oFlags.TimeLeft = oFlags.TimeOutSecs

            If oFlags.TestMode Then
                Finish()
            End If
        Catch ex As Exception
            oLog.LogMsg(ex.ToString)
            frmKiosk.lblErr.Text = ex.Message
            ProcessCancel()
        End Try
    End Sub

    ''' <summary>
    ''' The first screen
    ''' </summary>
    ''' <remarks></remarks>
    ''' //was buildstartscreen
    Public Sub StartOver()
        Try
            oCustomer = New clsCustomer(oFlags.WorkstationID)
            oFlags = New clsFlags(oLog)
            oErr = New clsError(oCustomer)
            StopTimers()
            oLog.LogMsg("Building Start Screen...")
            oFlags.TimeLeft = oFlags.TimeOutSecs
        Catch ex As Exception
            InitTimers()
            oLog.LogMsg(ex.ToString)
            frmKiosk.lblErr.Text = ex.Message
            ProcessCancel()
        End Try
    End Sub
    ''' <summary>
    ''' Ask the customer to select their langauge
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BuildSelectLangScreen()
        Try
            oFlags.Register = True
            oLog.LogMsg("Building Select Language Screen...")



            oLog.LogMsg("Calling Fill Languages to get Language List...")
            oCustomer.oLang.FillLanguages()
            If oFlags.TestMode Then
                ChooseEnglish()
            End If
            oFlags.TimeLeft = oFlags.TimeOutSecs
        Catch ex As Exception
            oLog.LogMsg(ex.ToString)
            frmKiosk.lblErr.Text = ex.Message
            ProcessCancel()
        End Try
    End Sub
    ''' <summary>
    ''' Depending on oFlags.ActiveTimer, the appropriate input screen is created.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BuildStartClearTimer()

        If oFlags.ActiveTimer = "MobileNum" Then
            oQData = New clsQueue(oFlags.WorkstationID)
            oQData.CustomerID = oCustomer.CustomerID
            oQData.QueueCode = "EP"
            oQData.Data1 = "30"
            oQData.Data4 = "5507"
            oQData.Data3 = "MOB"
            PostAndProcessQueue()

        ElseIf oFlags.ActiveTimer = "COPhone" Then
            oQData = New clsQueue(oFlags.WorkstationID)
            oQData.CustomerID = oCustomer.CustomerID
            oQData.QueueCode = "EP"
            oQData.Data1 = "30"
            oQData.Data4 = "10684"
            oQData.Data3 = "COP"
            PostAndProcessQueue()

        ElseIf oFlags.ActiveTimer = "SSN" Then
            oQData = New clsQueue(oFlags.WorkstationID)
            oQData.CustomerID = oCustomer.CustomerID
            oQData.QueueCode = "EP"
            oQData.Data1 = "30"
            oQData.Data4 = "5503"
            oQData.Data3 = "SSN"
            PostAndProcessQueue()

        ElseIf oFlags.ActiveTimer = "DOB" Then
            oQData = New clsQueue(oFlags.WorkstationID)
            oQData.CustomerID = oCustomer.CustomerID
            oQData.QueueCode = "EP"
            oQData.Data1 = "30"
            oQData.Data4 = "9682"
            oQData.Data3 = "DOB"
            PostAndProcessQueue()

        ElseIf oFlags.ActiveTimer = "CASH" Then
            oQData = New clsQueue(oFlags.WorkstationID)
            oQData.QueueCode = "EP"
            oQData.CustomerID = oCustomer.CustomerID
            oQData.Data1 = "30"
            oQData.Data4 = "10685"
            oQData.Data3 = "CSH"
            PostAndProcessQueue()

        End If
    End Sub

    Public Sub BuildSelectPayment()
        Dim iTemp As Double = 0

        oLog.LogMsg("Building Select Payment Screen...")
        oFlags.SelectPayment = True
        oFlags.TimeLeft = oFlags.TimeOutSecs
        If oFlags.TestMode Then
            CashAnotherCheck()
        End If
    End Sub

    Public Sub RaiseButton(ByVal b As Button)
        Dim t As Type = b.GetType
        Dim p As New EventArgs

        Dim o As Object() = {p}

        Dim mInfo As MethodInfo = t.GetMethod("OnClick", BindingFlags.NonPublic Or BindingFlags.Instance)
        mInfo.Invoke(b, o)

    End Sub
    Public Sub InitTimers()
        tmrCheckReviewed.Enabled = False
        tmrReg.Enabled = False
        tmrTimeOut.Enabled = True
        tmrTimeOut.Interval = 1000
    End Sub

    Public Sub StopTimers()
        tmrCheckReviewed.Enabled = False
        tmrReg.Enabled = False
        tmrTimeOut.Interval = 1000
    End Sub
    ''' <summary>
    ''' Create the parameters, then pass by reference.
    ''' </summary>
    ''' <param name="oCustomer">Customer Object</param>
    ''' <param name="oFlags">Flags Object</param>
    ''' <param name="oErr">Error Object</param>
    ''' <param name="PAN">Pan</param>
    ''' <remarks></remarks>
    Public Sub New(ByRef oCustomer As clsCustomer, ByRef oFlags As clsFlags, ByRef oErr As clsError, ByVal PAN As String)
        Try
            InitWebServices()
            oFlags = New clsFlags(oLog)
            oCustomer = New clsCustomer(oFlags.WorkstationID)
            oErr = New clsError(oCustomer)
            oQData = New clsQueue(oFlags.WorkstationID)
            oPrint = New clsPrinter
            oAffiliate.AffiliateID = ConfigurationManager.AppSettings("AffiliateID")
            oAffiliate.GetCompanyData()
            oPrint.oCustomer = oCustomer.Clone
            oPrint.PrintReceipt()
            oFlags.TimeLeft = oFlags.TimeOutSecs
            InitTimers()
            oFlags.TimeLeft = oFlags.TimeOutSecs



            If oFlags.TestMode = True Then
                oCheckCashing.PrepareTestData()
            End If
            oFlags.BuildStartScreen = True
            Start(PAN)
        Catch ex As Exception
            oLog.LogMsg(ex.ToString())
            Throw (ex)
        End Try
    End Sub
    ''' <summary>
    ''' Depending on the oFlags.ActiveTimer, start capture inputs.  CoPhone should be last, before finish
    ''' </summary>
    ''' <param name="strClearType"></param>
    ''' <param name="strClear"></param>
    ''' <remarks></remarks>
    Public Sub ProcessClearText(ByVal strClearType As String, ByVal strClear As String)
        Try
            Dim iTemp As Integer = 0
            If oCustomer.oCheck.Cash = "" Then
                oCustomer.oCheck.Cash = iTemp.ToString("C")
            End If
            If oFlags.SimPinPad Then
                If oFlags.ActiveTimer = "MobileNum" Then
                    strClear = oCheckCashing.GetTestData("Mobile", 0)
                ElseIf oFlags.ActiveTimer = "SSN" Then
                    strClear = oCheckCashing.GetTestData("SSN", 0)
                ElseIf oFlags.ActiveTimer = "COPhone" Then
                    ' strClear = oCheckCashing.GetTestData("COTelephoneNumber",nothing)
                    strClear = oCheckCashing.GetRandomNum(3, 999).ToString.PadRight(5, "0") + _
                        oCheckCashing.GetRandomNum(3, 999).ToString.PadRight(5, "0")
                ElseIf oFlags.ActiveTimer = "DOB" Then
                    strClear = oCheckCashing.GetTestData("DOB", 0)
                End If
                If oFlags.ActiveTimer = "CASH" Then
                Else
                    oFlags.Enter = True
                End If
            End If
            If strClearType = "MobileNum" Then
                oCustomer.Phone = strClear
                oErr = New clsError(oCustomer)
                If oFlags.ResumeReg Then
                    oFlags.BUildPinScreen = True
                    GetPinInput()
                Else
                    Dim iRet As Integer = oCustomer.CompareCustomerData()
                    If iRet >= 0 Then
                        oCustomer.GetCustomerDataFromID()
                    End If
                    oFlags.BuildPleaseWait = True
                    BuildPleaseWait()
                    Exit Sub
                End If

                oFlags.SetAfterEnterFlags()
                oFlags.TimeLeft = oFlags.LongTimeOut
                Exit Sub
            ElseIf strClearType = "SSN" Then
                oCustomer.SSN = strClear
                oErr = New clsError(oCustomer)
                If Not oFlags.Lost Then
                    If oCustomer.SSN = "999999999" Then
                        oFlags.BackGroundCheck = True
                    Else
                        oCustomer.DoBackGroundCheck()
                    End If
                Else
                    oCustomer.oCustomer = oCustomer.Clone
                    oCustomer.ProcessLostCard()
                    oCustomer = oCustomer.oCustomer.Clone
                    oFlags.BUildPinScreen = True
                    GetPinInput()
                    Exit Sub
                End If
                If oCustomer.SSN = "999999999" Then
                    oFlags.BackGroundCheck = True
                End If
                If oFlags.BackGroundCheck Then
                    ProcessEntity()
                    If oFlags.ResumeReg Then
                        oFlags.SetAfterEnterFlags()
                        oFlags.ActiveTimer = "MobileNum"
                        BuildStartClearTimer()
                    Else
                        oFlags.BUildPinScreen = True
                        GetPinInput()
                    End If
                Else
                    If frmKiosk.lblErr.Text <> "" Then
                        tmrReg.Enabled = False
                        ProcessCancel()
                        Exit Sub
                    End If
                End If
                Exit Sub
            ElseIf strClearType = "COPhone" Then
                oCustomer.oCheck.CompanyPhone = strClear
                oErr = New clsError(oCustomer)
                oLog.LogMsg("Called check verify ")
                oLog.LogMsg("CheckNum= " + oCustomer.oCheck.CheckNumber)
                oLog.LogMsg("RouteNum=" + oCustomer.oCheck.RouteNumber)
                oLog.LogMsg("Acctnum=" + oCustomer.oCheck.AccountNumber)
                oLog.LogMsg("CustID=" + oCustomer.CustomerID.ToString)
                oLog.LogMsg("Amt=" + oCustomer.oCheck.CheckAmount.ToString)
                oLog.LogMsg("Date=" + oCustomer.oCheck.CheckDate.ToString)
                oLog.LogMsg("Compname=" + oCustomer.oCheck.CompanyName)
                oLog.LogMsg("AuthCode=" + oCheckCashing.GetTestData("PayeeAuthorizationCode", 0).ToString)
                oLog.LogMsg("Company Phone=" + oCustomer.oCheck.CompanyPhone)
                oLog.LogMsg("CompID=" + oCustomer.oCheck.CompanyID.ToString)
                oLog.LogMsg("TranNum=" + oCustomer.oCheck.TransactionNumber.ToString)
                oLog.LogMsg("StoreID=" + oCustomer.oCheck.StoreID.ToString)
                oLog.LogMsg("First=" + oCustomer.FirstName)
                oLog.LogMsg("Last=" + oCustomer.LastName)
                oLog.LogMsg("DOB=" + oCustomer.oCheck.DOB)
                oLog.LogMsg("Zip=" + oCustomer.oCheck.Zip)
                If Not oCustomer.oCheck.CheckVerify() Then
                    oLog.LogMsg("Guarantee Code=" + oCustomer.oCheck.GuaranteeCode)
                    oLog.LogMsg("Check Not Cashed")
                    oFlags.BuildCannotCash = True
                    Exit Sub
                Else
                    oLog.LogMsg("Cash check")
                    oLog.LogMsg("Guarantee Code=" + oCustomer.oCheck.GuaranteeCode)
                    oFlags.BuildCheckInfoScreen = True
                    Exit Sub
                End If


                Exit Sub
            ElseIf strClearType = "CASH" Then
                iTemp = CType(strClear, Integer)
                If iTemp > oCustomer.oCheck.NetAmt Then
                    oFlags.SetAfterEnterFlags()
                    oFlags.ActiveTimer = "CASH"
                    BuildStartClearTimer()
                    Exit Sub
                End If
                If AuthorizeCheck() Then
                    oCustomer.oCheck.Cash = iTemp
                    oCustomer.oCheck.Cash = Math.Round(oCustomer.oCheck.CashAmt, 2)
                    oCustomer.oCheck.Cash = oCustomer.oCheck.CashAmt.ToString("C")
                    oCustomer.oCheck.CreateTransaction("WDL", "TFR", oCustomer.oCheck.BlockID, oCustomer.ActID, oCustomer.oCheck.NetAmt)
                    oLog.LogMsg("timer Cash stopped...")
                    oFlags.SetAfterEnterFlags()
                    AddMoney()

                    oPrint.oCustomer = oCustomer.Clone
                    oPrint.PrintReceipt()
                End If
                ProcessCancel()
                Exit Sub
            ElseIf strClearType = "DOB" Then
                If oCustomer.GetCustomerDataFromID() Then
                    oFlags.BuildSelectLangScreen = True
                    Exit Sub
                ElseIf oFlags.ResumeReg Then
                    If oCustomer.DOB = Date.Parse(strClear) Then
                        oCustomer.GetRegistrationDataFromPAN(oCustomer.oCard.PAN)
                        oChkService.CaptureRegCustImage(oFlags.WorkstationID, oCustomer.ScanID)
                        If frmKiosk.lblErr.Text = "" Then
                            oFlags.ResumeReg = True
                            oFlags.BuildHaveSSN = True
                            Exit Sub
                        Else
                            ProcessCancel()
                            Exit Sub
                        End If
                    Else
                        oErr.ErrCode = 29
                        ProcessCancel()
                    End If
                    Exit Sub
                Else
                    BuildEnterDOB()
                End If
            End If
            If oFlags.ActiveTimer = "CASH" Then
                If strClear = "" Then
                    iTemp = 0
                Else
                    iTemp = CType(strClear, Integer)
                End If

                If iTemp > oCustomer.oCheck.NetAmt Then
                    oFlags.Clear = True
                    Exit Sub
                End If
                oCustomer.oCheck.Cash = iTemp
                oCustomer.oCheck.PrePaidAmount = oCustomer.oCheck.NetAmt - oCustomer.oCheck.Cash
                oCustomer.oCheck.Cash = Math.Round(oCustomer.oCheck.CashAmt, 2)
                oCustomer.oCheck.Cash = oCustomer.oCheck.CashAmt.ToString("C")
            End If

        Catch ex As Exception
            oLog.LogMsg(ex.ToString)
            frmKiosk.lblErr.Text = ex.Message
            ProcessCancel()
        End Try
    End Sub
    Public Sub AddMoney()
        oQData = New clsQueue(oFlags.WorkstationID)
        oQData.CustomerID = oCustomer.CustomerID
        oQData.QueueCode = "PP"
        oQData.Data1 = oCustomer.oCheck.CashAmt
        oQData.Data2 = oCustomer.oCard.CardID
        oQData.Data3 = "WDL"
        oQData.Data4 = oCustomer.oCheck.BlockID
        oQData.Data5 = oCustomer.oCheck.TransactionNumber
        PostAndProcessQueue()
    End Sub
    ''' <summary>
    ''' Creates the customer and card
    ''' </summary>
    ''' <remarks></remarks>
    'Public Sub CreateCustomer()
    '    Try
    '        ProcessEntity()
    '        If Not oCustomer.CreateCustomer Then
    '            oErr.ErrCode = 5
    '            ProcessCancel()
    '            Exit Sub
    '        End If
    '    Catch ex As Exception
    '        oLog.LogMsg(ex.ToString)
    '        frmKiosk.lblErr.Text = ex.Message
    '        ProcessCancel()
    '    End Try
    'End Sub
    ''' <summary>
    ''' Wait for transact website to approve registration
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    'Public Sub tmrReg_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrReg.Tick
    '    Try
    '        oCustomer.ErrorText = ""
    '        oErr.ErrCode = 0
    '        If Not oFlags.Reviewed Then
    '            oFlags.Reviewed = oCustomer.RegistrationReviewed
    '        End If
    '        If oErr.ErrCode > 0 Then
    '            ProcessCancel()
    '            Exit Sub
    '        End If
    '        If Not (oFlags.Reviewed And oFlags.PhotoIDMatches And oFlags.ImageVerify And oCustomer.PhotoIDExpiration > Date.Now) Then

    '            If oCustomer.ErrorText <> "" Or oFlags.Reviewed Then
    '                If oCustomer.PhotoIDExpiration < Date.Now Then
    '                    oErr.ErrCode = 23
    '                Else
    '                    oErr.ErrCode = 17
    '                End If
    '                tmrReg.Enabled = False
    '                ProcessCancel()
    '                Exit Sub
    '            End If
    '        ElseIf oFlags.Reviewed Then
    '            tmrReg.Enabled = False
    '            If Not (oFlags.PhotoIDMatches And oFlags.ImageVerify) Then
    '                tmrReg.Enabled = False
    '                oErr.ErrCode = 17
    '                ProcessCancel()
    '                Exit Sub
    '            End If
    '            oLog.LogMsg("Customer registration was Reviewed...")
    '            If Not oFlags.ReRegister And Not oFlags.ScrapDragon Then
    '                oFlags.BuildHaveSSN = True
    '            Else
    '                If oCustomer.SSN = "" Then oCustomer.SSN = "0"
    '                If oCustomer.CreateCustomer Then
    '                    ProcessCancel()
    '                Else
    '                    oErr.ErrCode = 5
    '                    ProcessCancel()
    '                End If
    '                Exit Sub
    '            End If

    '        Else
    '            If frmKiosk.lblErr.Text <> "" Then
    '                tmrReg.Enabled = False
    '                ProcessCancel()
    '                Exit Sub
    '            End If
    '        End If

    '    Catch ex As Exception
    '        oLog.LogMsg(ex.ToString)
    '        frmKiosk.lblErr.Text = ex.Message
    '        ProcessCancel()
    '    End Try
    'End Sub


    ''' <summary>
    ''' Check to be sure the process was not abandoned
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub tmrTimeOut_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrTimeOut.Tick
        Try
            If oErr Is Nothing Then
                oErr = New clsError(oCustomer)
            End If
            oFlags.TimeLeft -= 1
            If oFlags.TimeLeft < 0 Then
                lblFinish.Text = ""
                oErr = New clsError(oCustomer)
                oLog.LogMsg("Kiosk timer timed out...")
                If oFlags.WrongCard Then
                    oFlags.WrongCard = False
                    'oCardRW.EjectCard()
                    oFlags.BUildPinScreen = True
                    GetPinInput()
                    Exit Sub
                End If
                If oFlags.CheckReviewedTimeOut Then
                    oFlags.CheckReviewedTimeOut = False
                    oErr.ErrCode = 39
                    ProcessCancel()
                    Exit Sub
                End If
                If oFlags.ShowOCRError Then
                    oFlags.ShowOCRError = False
                    oFlags.BuildPhotoIDScreen = True
                    Exit Sub
                End If
                If oFlags.ShowCheckScanError Then
                    oFlags.ShowCheckScanError = False

                    oFlags.BuildSignCheck = True
                    Exit Sub
                End If
                If oFlags.ShowPinMismatchError Then
                    oFlags.ShowPinMismatchError = False
                    If oFlags.EPSecondTry = False Then
                        oFlags.EPSecondTry = True
                        oFlags.BUildPinScreen = True
                        GetPinInput()
                        Exit Sub
                    Else
                        oErr.ErrCode = 28
                        oFlags.EnteringPin = False
                    End If
                End If
                If oFlags.EnteringPin Then
                    oFlags.EnteringPin = False
                    If oFlags.VPSecondTry Then
                        oFlags.BUildPinScreen = True
                        GetPinInput()
                        Exit Sub
                    Else
                        oFlags.VPSecondTry = False
                    End If
                End If
                ProcessCancel()
                oFlags.TimeLeft = oFlags.TimeOutSecs
            End If

        Catch ex As Exception
            oLog.LogMsg(ex.ToString)
            frmKiosk.lblErr.Text = ex.Message
            ProcessCancel()
        End Try
    End Sub
    ''' <summary>
    ''' Waiting for website to review the check
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub tmrCheckReview_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmrCheckReviewed.Tick
        Try
            oFlags.CheckReviewed = oCustomer.oCheck.CheckReviewed()
            If oFlags.CheckReviewed = False Then
                oFlags.CheckReviewedTimeOut = True


            Else
                oFlags.CheckReviewedTimeOut = False
                tmrCheckReviewed.Enabled = False
                If oCustomer.oCheck.PayeeAuthCode = "" Then
                    oCustomer.oCheck.PayeeAuthCode = oCheckCashing.GetRandomNumber(3, 999)
                End If
                oLog.LogMsg("Check was Reviewed...")
                oLog.LogMsg("Get Customer Data from CustiD on Card...")
                oLog.LogMsg("GetCustomerDataFromID RouteNumber=" + oCustomer.oCheck.RouteNumber)
                oLog.LogMsg("AcountNumber" + oCustomer.oCheck.AccountNumber)
                oLog.LogMsg("CheckNumber=" + oCustomer.oCheck.CheckNumber)
                oLog.LogMsg("CheckAmount=" + oCustomer.oCheck.CheckAmount.ToString())
                oLog.LogMsg("CheckDate=" + oCustomer.oCheck.CheckDate.ToString())
                Dim bRet As Boolean = oCustomer.GetCustomerDataFromID()
                If Not bRet Then
                    oLog.LogMsg("Customer " + oCustomer.CustomerString + " not in database.")
                    oErr.ErrCode = 3
                    ProcessCancel()
                    Exit Sub
                End If

                If oFlags.InPositiveFile Then
                    oFlags.BuildCheckInfoScreen = True
                    Exit Sub
                End If
                If oCustomer.oCheck.CompanyID = 0 Then
                    oFlags.ActiveTimer = "COPhone"
                    BuildStartClearTimer()
                    Exit Sub
                End If
                oLog.LogMsg("Called check verify ")
                oLog.LogMsg("CheckNum= " + oCustomer.oCheck.CheckNumber)
                oLog.LogMsg("RouteNum=" + oCustomer.oCheck.RouteNumber)
                oLog.LogMsg("Acctnum=" + oCustomer.oCheck.AccountNumber)
                oLog.LogMsg("CustID=" + oCustomer.CustomerString)
                oLog.LogMsg("Amt=" + oCustomer.oCheck.CheckAmount.ToString)
                oLog.LogMsg("Date=" + oCustomer.oCheck.CheckDate.ToString)
                oLog.LogMsg("Compname=" + oCustomer.oCheck.CompanyName)
                oLog.LogMsg("AuthCode=" + oCheckCashing.GetTestData("PayeeAuthorizationCode", 0).ToString)
                oLog.LogMsg("Company Phone=" + oCustomer.oCheck.CompanyPhone)
                oLog.LogMsg("CompID=" + oCustomer.oCheck.CompanyID.ToString)
                oLog.LogMsg("TranNum=" + oCustomer.oCheck.TransactionNumber.ToString)
                oLog.LogMsg("StoreID=" + oCustomer.oCheck.StoreID.ToString)
                oLog.LogMsg("First=" + oCustomer.FirstName)
                oLog.LogMsg("Last=" + oCustomer.LastName)
                oLog.LogMsg("DOB=" + oCustomer.oCheck.DOB)
                oLog.LogMsg("Zip=" + oCustomer.oCheck.Zip)
                If Not oCustomer.oCheck.CheckVerify() Then
                    oLog.LogMsg("Guarantee Code=" + oCustomer.oCheck.GuaranteeCode)
                    oLog.LogMsg("Check Not Cashed")
                    oFlags.BuildCannotCash = True
                    Exit Sub
                Else
                    oLog.LogMsg("Cash check")
                    oLog.LogMsg("Guarantee Code=" + oCustomer.oCheck.GuaranteeCode)
                    oFlags.BuildCheckInfoScreen = True
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            oLog.LogMsg(ex.ToString)
            frmKiosk.lblErr.Text = ex.Message
            ProcessCancel()
        End Try
    End Sub



    'Public Sub ProcessGoodPin()
    '    oLog.LogMsg("ProcessGoodPin called")
    '    If oFlags.ReRegister Then
    '        If Not oCustomer.CreateCustomer() Then
    '            oErr.ErrCode = 15
    '            ProcessCancel()
    '        End If
    '        If oCustomer.oFlags.CustomerExists Then
    '            frmKiosk.lblErr.Text = oCustomer.oLang.GetScreenLabel("reregister")
    '            ProcessCancel()
    '            Exit Sub
    '        Else
    '            oErr.ErrCode = 36
    '            ProcessCancel()
    '        End If
    '    ElseIf oFlags.Lost Then
    '        Dim strType As String = oCustomer.oCard.ValidateBin()
    '        If strType = "PP" Then
    '            oErr.ErrCode = 25
    '        ElseIf strType = "PY" Then
    '            oErr.ErrCode = 26
    '        ElseIf strType = "PN" Then
    '            oErr.ErrCode = 27
    '        End If
    '        oFlags.WrongCard = True
    '        ProcessCancel()
    '        Exit Sub
    '    ElseIf oFlags.ScrapDragon Then
    '        oFlags.BuildSelectLangScreen = True
    '    ElseIf oCustomer.oFlags.CustomerExists Or oFlags.TestMode Then
    '        oCustomer.oCheck.CreateTransaction("DEP", "TFR", oCustomer.oCheck.BlockID, oCustomer.ActID, 0)
    '        Dim oSave As New SaveJournal.Journal
    '        Dim strXMl As String = oSave.SaveJournalXML(oCustomer.CustomerID, Date.Now.ToString, oCustomer.oCheck.BlockID, oCustomer.oCheck.TransactionNumber, "DK", "KIO", "Created Transaction", 0, "")
    '        Dim xmlDoc As New XmlDocument
    '        oLog.LogMsg("Save Journal called...")
    '        xmlDoc.LoadXml(strXMl)
    '        Dim xmlNode As XmlElement
    '        For Each xmlNode In xmlDoc.ChildNodes(0).ChildNodes(0).ChildNodes
    '            If xmlNode.Name = "ReturnCode" Then
    '                If xmlNode.InnerText = 0 Then
    '                    oLog.LogMsg("Save Journal Succeeded...")
    '                Else
    '                    oLog.LogMsg("Save Journal Failed with return code: " + xmlNode.InnerText)
    '                End If
    '            End If
    '        Next

    '        If oFlags.TestMode Then
    '            If oFlags.TestType = "N" Then
    '                RegisterNewPhotoID()
    '            ElseIf oFlags.TestType = "C" Then
    '                ChooseCashCheck()
    '            ElseIf oFlags.TestType = "R" Then
    '                oFlags.BuildSelectLangScreen = True
    '            End If
    '        Else
    '            ChooseCashCheck()
    '        End If
    '    End If
    'End Sub

    'Public Function ProcessQueue() As Boolean
    '    Try
    '        oLog.LogMsg("QueueCode=" + oQData.QueueCode + vbCrLf)
    '        oLog.LogMsg("Status=" + oQData.StatusCode + vbCrLf)
    '        If oQData.ReturnCode2 Is Nothing Then
    '            oLog.LogMsg("ReturnCode2=nothing" + vbCrLf)
    '        Else
    '            oLog.LogMsg("ReturnCode2=" + oQData.ReturnCode2 + vbCrLf)
    '        End If
    '        oLog.LogMsg("Data1=" + oQData.Data1)
    '        oLog.LogMsg("Data2=" + oQData.Data2)
    '        oLog.LogMsg("Data3=" + oQData.Data3)
    '        oLog.LogMsg("oFlags.Register=" + oFlags.Register.ToString())
    '        If oFlags.SimPinPad Then
    '            oQData.ReturnCode2 = "0"
    '            oQData.Data1 = "0"
    '            oQData.Data2 = "0"
    '        End If
    '        If oQData.InputQueue.QueueCode = "VP" Then
    '            If oQData.StatusCode = "Processing" Then
    '                oFlags.PinOK = False
    '            ElseIf oQData.ReturnCode2 = "0" Then
    '                oFlags.PinOK = True
    '            Else
    '                oFlags.PinOK = False
    '            End If
    '            If oFlags.PinOK Then
    '                oFlags.VPSecondTry = False
    '                oCustomer.oCheck.PrePaidBalance = CType(oQData.Data1, Decimal)
    '                oCustomer.oCheck.PrePaidAvailable = CType(oQData.Data2, Decimal)
    '                oCustomer.oCheck.BeginningBal = CType(oQData.Data1, Decimal)
    '                oCustomer.oFlags.CustomerExists = True
    '                If oFlags.Register And oFlags.IssueCard Then
    '                    oFlags.EPSecondTry = False
    '                    Dim oJournal As New SaveJournal.Journal
    '                    Dim strCode As String = ""
    '                    If oFlags.ReRegister = True Then
    '                        strCode = "ReReg"
    '                    Else
    '                        strCode = "Reg"
    '                    End If
    '                    'oJournal.SaveJournalXML(oCustomer.CustomerID, Date.Now.ToString, strCode, "KIOSK", "ProgData", 0, oCustomer.oCard.Track1)
    '                    Dim seg1 As New clsDataModule.clsLabel
    '                    seg1.strLabel = "CMReg1"
    '                    seg1.iTranslate = 1
    '                    Dim seg2 As New clsDataModule.clsLabel
    '                    seg2.strLabel = "PasswordEntry"
    '                    seg2.iTranslate = 1
    '                    Dim seg3 As New clsDataModule.clsLabel
    '                    seg3.strLabel = oCheckCashing.GetRandomNum(3, 999).ToString().PadRight(3, "0") + oCheckCashing.GetRandomNum(3, 999).ToString().PadRight(3, "0") + oCheckCashing.GetRandomNum(2, 99).ToString().PadRight(2, "0")
    '                    seg3.iTranslate = 0
    '                    oCustomer.Password = seg3.strLabel
    '                    Dim arrSeg(2) As clsDataModule.clsLabel
    '                    arrSeg(0) = seg1
    '                    arrSeg(1) = seg2
    '                    arrSeg(2) = seg3
    '                    oCheckCashing.UpdateCustomer(oCheckCashing.CreatePasswordHash(seg3.strLabel, oCustomer.UserSalt), oCustomer.CustomerID)

    '                    oCheckCashing.COMMS(oCustomer.CustomerID, arrSeg)
    '                    ProcessCancel()
    '                ElseIf Not oFlags.Register Then
    '                    ProcessGoodPin()
    '                End If
    '            ElseIf oFlags.VPSecondTry Then
    '                oFlags.VPSecondTry = False
    '                oErr.ErrCode = 30
    '                ProcessCancel()
    '            Else
    '                oFlags.VPSecondTry = True
    '                oErr.ErrCode = 18
    '                ProcessCancel()
    '            End If

    '        ElseIf oQData.InputQueue.QueueCode = "IP" Then
    '            If oQData.StatusCode = "Processing" Then
    '                oFlags.IssuePin = False
    '            ElseIf oQData.ReturnCode2 = "0" Then
    '                oFlags.IssuePin = True
    '            Else
    '                oFlags.IssuePin = False
    '            End If
    '            oFlags.EnteringPin = False
    '            If oFlags.IssuePin Then
    '                IssueCard()
    '            Else
    '                oErr.ErrCode = 19
    '                ProcessCancel()
    '            End If
    '        ElseIf oQData.InputQueue.QueueCode = "IC" Then
    '            If oQData.StatusCode = "Processing" Then
    '                oFlags.IssueCard = False
    '            ElseIf oQData.ReturnCode2 = "0" Then
    '                oFlags.IssueCard = True
    '            Else
    '                oFlags.IssueCard = False
    '            End If

    '            If oFlags.IssueCard Then
    '                oQData = New clsQueue(oFlags.WorkstationID)
    '                oQData.CustomerID = oCustomer.CustomerID
    '                oQData.QueueCode = "EP"
    '                oQData.Data2 = oCustomer.oCard.CardID
    '                oQData.Data3 = "PN1"
    '                oQData.Data4 = "343"
    '                oQData.Data5 = oCustomer.oLang.LangID
    '                PostAndProcessQueue()

    '            End If

    '        ElseIf oQData.InputQueue.QueueCode = "PP" Then
    '            If oQData.StatusCode = "Processing" Then
    '                oErr.ErrCode = 21
    '                ProcessCancel()
    '            End If
    '            If oQData.ReturnCode2 = "0" Then
    '                oCustomer.oCheck.PrePaidBalance = Decimal.Parse(oQData.Data1)
    '                oCustomer.oCheck.PrePaidAvailable = Decimal.Parse(oQData.Data2)
    '                If oCustomer.oCheck.PrePaidBalance < oCustomer.oCheck.PrePaidAvailable Then
    '                    oCustomer.oCheck.PrePaidAvailable = oCustomer.oCheck.PrePaidBalance
    '                End If
    '            Else
    '                oErr.ErrCode = 21
    '                ProcessCancel()
    '            End If
    '        ElseIf oQData.InputQueue.QueueCode = "EC" Then
    '            If oQData.StatusCode = "Processing" Then
    '                oErr.ErrCode = 53
    '                ProcessCancel()
    '            ElseIf oQData.ReturnCode2 = 0 Then
    '                oCustomer.oCard.PAN = oQData.Data2
    '                ProcessCard(oCustomer.oCard.PAN)
    '            Else
    '                oErr.ErrCode = 53
    '                ProcessCancel()
    '            End If
    '        ElseIf oQData.InputQueue.QueueCode = "EP" Then
    '            If oQData.StatusCode = "Processing" Then
    '                oErr.ErrCode = 53
    '                ProcessCancel()
    '            ElseIf oQData.ReturnCode2 = "0" Then
    '                If oQData.InputQueue.Data3 = "MOB" Then
    '                    oCustomer.Phone = oQData.Data2.Trim()
    '                    ProcessClearText("MobileNum", oCustomer.Phone)
    '                ElseIf oQData.InputQueue.Data3 = "SSN" Then
    '                    oCustomer.SSN = oQData.Data2.Trim()
    '                    ProcessClearText("SSN", oCustomer.SSN)
    '                ElseIf oQData.InputQueue.Data3 = "COP" Then
    '                    oCustomer.oCheck.CompanyPhone = oQData.Data2.Trim()
    '                    ProcessClearText("COPhone", oCustomer.oCheck.CompanyPhone)
    '                ElseIf oQData.InputQueue.Data3 = "DOB" Then
    '                    oCustomer.DOB = oQData.Data2.Trim()
    '                    ProcessClearText("DOB", oCustomer.oCheck.CompanyPhone)
    '                ElseIf oQData.InputQueue.Data3 = "CSH" Then
    '                    oCustomer.oCheck.Cash = oQData.Data2.Trim()
    '                    ProcessClearText("CASH", oCustomer.oCheck.Cash)
    '                ElseIf oQData.InputQueue.Data3 = "PN1" Then
    '                    oCustomer.oCard.KSN = oQData.Data3.Trim()
    '                    oCustomer.oCard.PIN = oQData.Data2.Trim()
    '                    If Not oFlags.Register Then
    '                        oQData = New clsQueue(oFlags.WorkstationID)
    '                        oQData.CustomerID = oCustomer.CustomerID
    '                        oQData.QueueCode = "VP"
    '                        oQData.Data1 = oCustomer.oCard.PIN2
    '                        oQData.Data2 = oCustomer.oCard.CardID
    '                        oQData.Data3 = oCustomer.oCard.KSN2
    '                        PostAndProcessQueue()
    '                    ElseIf oFlags.bOnFirstPin Then
    '                        oFlags.OnSecondPin = True
    '                        oFlags.bOnFirstPin = False
    '                        If Not oCustomer.CreateCustomer() Then
    '                            oErr.ErrCode = 15
    '                            ProcessCancel()
    '                        End If
    '                        IssuePin()


    '                    ElseIf oFlags.OnSecondPin Then
    '                        oFlags.bOnFirstPin = False
    '                        oFlags.OnSecondPin = False
    '                        VerifyPin()

    '                    End If
    '                End If
    '            Else
    '                oErr.ErrCode = 53
    '                ProcessCancel()
    '            End If

    '        End If

    '        If oQData.QueueCode <> "" Then
    '            oQData = New clsQueue(oFlags.WorkstationID)
    '        End If

    '    Catch ex As Exception
    '        oLog.LogMsg(ex.ToString)
    '        frmKiosk.lblErr.Text = ex.Message
    '        ProcessCancel()
    '    End Try
    'End Function
    Public Sub IssuePin()
        oQData = New clsQueue(oFlags.WorkstationID)
        oQData.QueueCode = "IP"
        oQData.Data1 = oCustomer.oCard.PIN
        oQData.Data2 = oCustomer.oCard.CardID
        oQData.Data3 = oCustomer.oCard.KSN
        oQData.Data4 = String.Empty
        oQData.Data5 = String.Empty
        PostAndProcessQueue()
    End Sub
    Public Sub VerifyPin()
        oQData = New clsQueue(oFlags.WorkstationID)
        oQData.CustomerID = oCustomer.CustomerID
        oQData.QueueCode = "VP"
        oQData.Data1 = oCustomer.oCard.PIN2
        oQData.Data2 = oCustomer.oCard.CardID
        oQData.Data3 = oCustomer.oCard.KSN2
        oQData.Data4 = "308"
        oQData.Data5 = oCustomer.oLang.LangID
        PostAndProcessQueue()
    End Sub
    Public Sub IssueCard()
        oQData = New clsQueue(oFlags.WorkstationID)
        oQData.CustomerID = oCustomer.CustomerID
        oQData.QueueCode = "IC"
        oQData.Data2 = oCustomer.oCard.CardID
        oQData.Data3 = oCustomer.oCard.TransactCardType
        oQData.Data4 = "0"
        PostAndProcessQueue()
    End Sub

    ''' <summary>
    ''' Authorize check to finish
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AuthorizeCheck() As Boolean
        oFlags.TimeLeft = oFlags.ShortTimeOut
        oFlags.EndCheckCash = True
        If oCustomer.oCheck.Authorized(oFlags.WorkstationID, oCustomer.oCard.CardID) Then
            oLog.LogMsg("Authorize Check reports success")
            oLog.LogMsg("Check authorized")
            Return True
        Else
            oLog.LogMsg("Authorize Check reports an error")
            oErr.ErrCode = 22
            Return False
        End If
    End Function

    Public Function TestHeavyMetalRegistration() As Integer
        Try
            Dim i As Integer = 0
            Dim out1, out2 As Integer
            Dim rc As Integer = 0
            Dim rcBackGrd As String = ""
            Dim rcCard As Integer = 0
            Dim ocheckService As New CheckServices.CheckServices()
            Dim netCard As Decimal = Decimal.Parse(oCheckCashing.GetTestData("NetCard", 300))
            Dim syTranID As Integer = oCheckCashing.GetRandomNumber(3, 999)
            Dim strPhoto As String = oCheckCashing.GetTestData("PhotoIDNumber", 300)
            Dim strState As String = oCheckCashing.GetTestData("StateCode", 300)
            Dim dDob As Date = oCheckCashing.GetTestData("DOB", 300)
            Dim strPan As String = oCheckCashing.GetTestData("PAN", 300)
            Dim iType As Integer = Int32.Parse(oCheckCashing.GetTestData("SDIDType", 300))
            Dim strPrefix As String = oCheckCashing.GetTestData("Surname", 300)
            Dim strCounty As String = oCheckCashing.GetTestData("County", 300)
            Dim strIssueDate As String = oCheckCashing.GetTestData("IssueDate", 300)
            Dim strExpirationDate As String = oCheckCashing.GetTestData("ExpirationDate", 300)
            Dim strEye As String = oCheckCashing.GetTestData("Eye", 300)
            Dim strHair As String = oCheckCashing.GetTestData("Hair", 300)
            Dim strFirst As String = oCheckCashing.GetTestData("First", 300)
            Dim strLast As String = oCheckCashing.GetTestData("Last", 300)
            Dim strMiddle As String = oCheckCashing.GetTestData("Middle", 300)
            Dim strAddr As String = oCheckCashing.GetTestData("Address1", 300)
            Dim strAddr2 As String = oCheckCashing.GetTestData("Address2", 300)
            Dim strCity As String = oCheckCashing.GetTestData("City", 300)
            Dim strZip As String = oCheckCashing.GetTestData("Zip", 300)
            Dim strHeight As String = oCheckCashing.GetTestData("Height", 300)
            Dim strweight As String = oCheckCashing.GetTestData("Weight", 300)
            Dim strSex As String = oCheckCashing.GetTestData("Sex", 300)
            Dim strSSN As String = oCheckCashing.GetTestData("SSN", 300)
            Dim strMobile As String = oCheckCashing.GetTestData("Mobile", 300)
            Dim strPin1 As String = oCheckCashing.GetTestData("PinBlock1", 300)
            Dim strPin2 As String = oCheckCashing.GetTestData("PinBlock2", 300)
            Dim Password As String = oCheckCashing.GetTestData("Password", 300)
            ocheckService.Timeout = ConfigurationManager.AppSettings("WebServiceTimeout")
            Dim regArray As New ArrayList
            regArray.Add("type=R")
            regArray.Add("sytranid=" + syTranID.ToString())
            regArray.Add("photoID=" + strPhoto)
            regArray.Add("issueDate=" + strIssueDate)
            regArray.Add("expDate=" + strExpirationDate)
            regArray.Add("dob=" + dDob.ToShortDateString())
            regArray.Add("sdidtype=" + iType.ToString)
            regArray.Add("surname=" + strPrefix)
            regArray.Add("firstname=" + strFirst)
            regArray.Add("middlename=" + strMiddle)
            regArray.Add("lastname=" + strLast)
            regArray.Add("address1=" + strAddr)
            regArray.Add("address2=" + strAddr2)
            regArray.Add("city=" + strCity)
            regArray.Add("state=" + strState)
            regArray.Add("zip=" + strZip)
            regArray.Add("county=" + strCounty)
            regArray.Add("eyecolor=" + strEye)
            regArray.Add("haircolor=" + strHair)
            regArray.Add("height=" + strHeight)
            regArray.Add("weight=" + strweight)
            regArray.Add("race=Caucasian")
            regArray.Add("sex=" + strSex)
            regArray.Add("eaddress=" + oCustomer.EmailAddress)
            regArray.Add("langID=" + oCustomer.oLang.LangID.ToString())
            regArray.Add("providerID=" + ConfigurationManager.AppSettings("ProviderID").ToString())
            regArray.Add("test=0")
            regArray.Add("pan=" + strPan)
            regArray.Add("testpass=" + Password)
            regArray.Add("AutoReview=1")
            'ocheckService.HMRHeavyMetalRegistration(strPhoto, dDob, strFirst, strMiddle, strLast, strAddr, strAddr2, strCity, strState, strZip, oCustomer.EmailAddress, 4566, "", "", 0, "", rcCard)

            Dim oJ As New SaveJournal.Journal
            oJ.SaveJournal(oCustomer.CustomerID, Date.Now.ToString, oCustomer.oCheck.BlockID, oCustomer.oCheck.TransactionNumber, "K0", "KIOSK", "Load Scrap Regs", 0, "out1=" + out1.ToString() + " out2=" + out2.ToString() + " rc=" + rc.ToString() + " rcBackGrd=" + rcBackGrd.ToString() + " rcCard" + rcCard.ToString())
            Return rcCard
        Catch ex As Exception
            oLog.LogMsg(ex.ToString)
        End Try

    End Function
    ''' <summary>
    ''' Determines next step in flow
    ''' </summary>
    ''' <param name="Pan"></param>
    ''' <remarks></remarks>
    Public Sub ProcessCard(ByVal Pan As String)
        Try
            InitTimers()

            Dim iRet As Integer = 0
            If oFlags.SimCardReader Then
                If (oFlags.SimScrapYard) Then
                    oFlags.BuildTestScreen = True
                    Exit Sub
                End If
            End If
            oCustomer.oCard.PAN = Pan
            oCustomer.oCard.PANNotEncrypted = Pan
            iRet = oCustomer.oCard.ProcessCard(oCustomer)
            oLog.LogMsg("Ret=" + iRet.ToString + "Track1=" + oCustomer.oCard.Track1 + "Track2=" + oCustomer.oCard.Track2 + "Track3=" + oCustomer.oCard.Track3)
            If (oErr.ErrCode > 0) Then
                ProcessCancel()
                Exit Sub
            End If
            'oPinPad.Account = oCustomer.oCard.PAN
            oCustomer.oCard.PAN = oCustomer.EncryptValue(oCustomer.oCard.PAN)
            oCustomer.oCard.PANNotEncrypted = oCustomer.oCard.PAN
            oLog.LogMsg("Pan=" + oCustomer.oCard.PAN)
            If iRet = 10 Then
                oCheckCashing.UpdateCaseNum()
            End If
            If iRet = -1 Then
                ProcessCancel()
                Exit Sub
            ElseIf iRet = 1 Then
                oFlags.BUildPinScreen = True
                GetPinInput()
            ElseIf iRet = 2 Then
                If Not oFlags.ResumeReg Then
                    oFlags.BuildSelectREgistration = True
                Else
                    BuildEnterDOB()
                End If
            End If
            oFlags.TimeLeft = oFlags.TimeOutSecs
        Catch ex As Exception
            oLog.LogMsg(ex.ToString)
            frmKiosk.lblErr.Text = ex.Message
            ProcessCancel()
        End Try
    End Sub
    ''' <summary>
    ''' Sets the oCustomer.oCard.TransactCardType to loadable or non
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ProcessEntity()
        oCustomer.oCard.TransactCardType = oFlags.HMILoadable
        If oAffiliate.EntitiesBK = 1 And Not oFlags.BackGroundCheck Then

            oCustomer.oCard.TransactCardType = oFlags.HMINonloadable
        End If
        If ((oAffiliate.EntitiesUS = 1 Or oAffiliate.EntitiesFN = 1)) Then
            If oCustomer.IDType > 8 Then
                oCustomer.oCard.TransactCardType = oFlags.HMINonloadable
            End If
        End If
        If oCustomer.oCard.TransactCardType = oFlags.HMINonloadable Then
            oFlags.Reloadable = False
        ElseIf oCustomer.oCard.TransactCardType = oFlags.HMILoadable Then
            oFlags.Reloadable = True
        End If
    End Sub

    Public Sub ChooseToRegister()
        oFlags.Register = True
        oFlags.ReRegister = False
        oFlags.CheckCash = False
        oFlags.BuildSelectLangScreen = True
    End Sub

    Public Sub MarkasPreRegistered()
        oFlags.CardManagement = True
        oFlags.BuildSelectLangScreen = True
    End Sub
    ''' <summary>
    ''' Sets the card non reloadable
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CustWithNoSSN()
        ProcessEntity()
    End Sub

    Public Sub CaptureSSN()
        oFlags.ActiveTimer = "SSN"
        BuildStartClearTimer()
    End Sub

    Public Sub GetCashAmountToDispense()
        oFlags.MultiCheck = False
        If Not oFlags.PrePaidOnly Then
            oFlags.ActiveTimer = "CASH"
            BuildStartClearTimer()
        End If
    End Sub

    Public Sub CashAnotherCheck()
        oFlags.MultiCheck = True
        If AuthorizeCheck() Then
            oPrint.oCustomer = oCustomer.Clone
            oPrint.PrintReceipt()
        End If
        'ProcessGoodPin()
        ProcessCancel()
    End Sub

    Public Function TestPrepaid() As String
        Dim i As Integer = 0
        Dim out1, out2 As Integer
        Dim rc As String = 0
        Dim rcBackGrd As String = ""
        Dim rcCard As String = 0
        Dim ocheckService As New CheckServices.CheckServices()
        Dim netCard As Decimal = Decimal.Parse(oCheckCashing.GetTestData("NetCard", 550))
        Dim syTranID As Integer = oCheckCashing.GetRandomNumber(3, 999)
        Dim strPhoto As String = oCheckCashing.GetTestData("PhotoID", 550)
        Dim strState As String = oCheckCashing.GetTestData("State", 550)
        Dim dDob As Date = CType(oCheckCashing.GetTestData("DOB", 550), Date).ToShortDateString
        Dim strPan As String = oCheckCashing.GetTestData("PAN", 550)
        ocheckService.Timeout = ConfigurationManager.AppSettings("WebServiceTimeout")
        Dim OutCardBal As String = "0"
        Dim OutCardAvailable As String = "0"
        'ocheckService.HeavyMetalPrePaid("P", syTranID.ToString(), netCard, strPhoto,
        '                                     strState,
        '                                     dDob.ToString(),
        '                                     strPan,
        '                                     ConfigurationManager.AppSettings("ProviderID"),
        '                                     OutCardBal,
        '                                     OutCardAvailable,
        '                                     0,
        '                                     rc,
        '                                     rcCard)
        Dim oJ As New SaveJournal.Journal
        oJ.SaveJournal(oCustomer.CustomerID, Date.Now.ToString, oCustomer.oCheck.BlockID, oCustomer.oCheck.TransactionNumber, "K0", "KIOSK", "Load Scrap Regs", 0, "out1=" + out1.ToString() + " out2=" + out2.ToString() + " rc=" + rc.ToString() + " rcBackGrd=" + rcBackGrd.ToString() + " rcCard" + rcCard.ToString())
        Return rcCard

    End Function
    Public Sub TestPositiveCheck(ByRef rc As Integer, ByRef rc1 As String, ByRef rc2 As String)
        Dim i As Integer = 0
        Dim rc3 As Integer = 0
        Dim ocheckService As New CheckServices.CheckServices()
        Dim netCard As Decimal = Decimal.Parse(oCheckCashing.GetTestData("NetCheck", 501))
        Dim syTranID As Integer = oCheckCashing.GetRandomNumber(3, 999)
        Dim strPhoto As String = oCheckCashing.GetTestData("PhotoID", 501)
        Dim strState As String = oCheckCashing.GetTestData("State", 501)
        Dim dDob As Date = oCheckCashing.GetTestData("DOB", 501)
        Dim strType As String = oCheckCashing.GetTestData("Type", 501)
        Dim strRoute As String = oCheckCashing.GetTestData("RouteNum", 501)
        Dim strCheck As String = oCheckCashing.GetTestData("CheckNum", 501)
        Dim strAccount As String = oCheckCashing.GetTestData("Account", 501)
        Dim dCheck As String = oCheckCashing.GetTestData("CheckDate", 501)
        Dim cPrint As Char = oCheckCashing.GetTestData("PrintCode", 501)
        'ocheckService.HeavyMetalPositiveCheck(strType, syTranID, netCard, strRoute, strAccount, strCheck, dCheck, cPrint.ToString(), strPhoto, dDob, strState, System.Configuration.ConfigurationManager.AppSettings("ProviderID"), rc, rc1, rc2)
        Dim oJ As New SaveJournal.Journal
        oJ.SaveJournal(oCustomer.CustomerID, Date.Now.ToString, oCustomer.oCheck.BlockID, oCustomer.oCheck.TransactionNumber, "K0", "KIOSK", "Scrap Pos Check", 0, "rc=" + rc.ToString() + " rc1=" + rc1.ToString() + " rc2=" + rc2.ToString())

        oCheckCashing.PrepareTestData()
    End Sub




    Public Sub PostAndProcessQueue()
        oQData.PostToQueue()
        'ProcessQueue()
    End Sub

    Public Sub InitWebServices()

        oCheckService.Url = ConfigurationManager.AppSettings("CheckServicesURL")
        oOCR.Url = ConfigurationManager.AppSettings("OCRScanURL")
        oChkService.Url = ConfigurationManager.AppSettings("CheckServiceURL")
        oJourn.Url = ConfigurationManager.AppSettings("SaveJournalURL")
    End Sub
End Class
