Imports System.Xml
Imports System.Configuration
Imports System
Public Class clsCheck
    Public Property oFlags As clsCheck.clsCheckFlags
    Dim mCheckAmt As Double = 0
    Dim mFee1Amt As Double = 0
    Dim mNetAmt As Double = 0
    Dim strRouteNum As String = "0"
    Dim strAccountNum As String = "0"
    Dim strCheckNum As String = "0"
    Dim iBlockID As Integer = 0
    Dim strApprovalCode As String
    Dim strGuaranteeCode As String
    Dim mPPBal As Double = 0
    Dim strCompName As String = ""
    Dim iCompID As Integer = 0
    Dim oCheckCashing As New clsDataModule.clsInterface
    Dim dPrePaidAvailable As Decimal = 0.0
    Dim dBeginningBal As Decimal = 0.0
    Dim iStoreID As Integer = 0
    Dim dCheckDate As Date = Nothing
    Dim strBarcode As String
    Dim mCashAmt As Double
    Dim mPrePaidAmt As Double
    Dim strCash As String = "0"
    Dim bPrePaidAvailable As Double
    Dim strPayeeAuthCode As String = ""
    Dim iCheckCategoryID As Integer = 0

    Dim oCheckServ As New CheckService.ICheckServiceservice
    Dim oCheckService As New CheckServices.CheckServices

    Public Property oLog As New clsLog
    Public Const NUMTRIESCHECKSCAN As Integer = 3
    Public Property PayeeAuthCode As String
        Get
            Return strPayeeAuthCode
        End Get
        Set(value As String)
            strPayeeAuthCode = value
        End Set
    End Property
    Public Property BeginningBal As Decimal
        Get
            Return dBeginningBal
        End Get
        Set(ByVal value As Decimal)
            dBeginningBal = value
        End Set
    End Property
    Dim strCustID As String
    Dim strCoPhone As String = ""
    Dim strLastName As String = ""
    Dim strFirstname As String = ""
    Dim iTransNum As Integer = 0
    Dim dCustDOB As New Date
    Dim strZip As String
    Dim iVaultAccount As Integer = 0
    Dim oVerify As New CheckVerify.CheckValidator
    Dim iActID As Integer = 0
    Public Property vaultAccount As Integer
        Get
            Return iVaultAccount
        End Get
        Set(value As Integer)
            iVaultAccount = value
        End Set
    End Property
    Public Property Zip As String
        Get
            Return strZip
        End Get
        Set(value As String)
            strZip = value
        End Set
    End Property
    Public Property DOB As Date
        Get
            Return dCustDOB
        End Get
        Set(value As Date)
            dCustDOB = value
        End Set
    End Property
    Public Property TransactionNumber As Integer
        Get
            Return iTransNum
        End Get
        Set(value As Integer)
            iTransNum = value
        End Set
    End Property
    Public Property LastName As String
        Get
            Return strLastName
        End Get
        Set(value As String)
            strLastName = value
        End Set
    End Property
    Public Property FirstName As String
        Get
            Return strFirstname
        End Get
        Set(value As String)
            strFirstname = value
        End Set
    End Property
    Public Property CompanyPhone As String
        Get
            Return strCoPhone
        End Get
        Set(value As String)
            strCoPhone = value
        End Set
    End Property

    Public Property CustomerID As String
        Get
            Return strCustID
        End Get
        Set(value As String)
            strCustID = value
        End Set
    End Property
    Public Property Cash As String
        Get
            Return strCash
        End Get
        Set(value As String)
            strCash = value
            If IsNumeric(strCash) Then
                CashAmt = CType(strCash, Integer)
            End If
        End Set
    End Property
    Public Property PrePaidAvailable As Double
        Get
            Return bPrePaidAvailable
        End Get
        Set(value As Double)
            bPrePaidAvailable = value
        End Set
    End Property
    Public Property PrePaidAmount As Double
        Get
            Return mPrePaidAmt
        End Get
        Set(value As Double)
            mPrePaidAmt = value
        End Set
    End Property
    Public Property CashAmt As Double
        Get
            Return mCashAmt
        End Get
        Set(value As Double)
            mCashAmt = value
        End Set
    End Property
    Public Property Barcode As String
        Get
            Return strBarcode
        End Get
        Set(value As String)
            strBarcode = value
        End Set
    End Property
    Public Property CompanyID As Integer
        Get
            Return iCompID
        End Get
        Set(value As Integer)
            iCompID = value
        End Set
    End Property
    Public Property CheckDate As Date
        Get
            Return dCheckDate
        End Get
        Set(value As Date)
            dCheckDate = value
        End Set
    End Property

    Public Property StoreID As Integer
        Get
            Return iStoreID
        End Get
        Set(value As Integer)
            iStoreID = value
        End Set
    End Property
    Public Property ActID As Integer
        Get
            Return iActID

        End Get
        Set(value As Integer)
            iActID = value
        End Set
    End Property


    Public Property CompanyName As String
        Get
            Return strCompName
        End Get
        Set(value As String)
            strCompName = value
        End Set
    End Property
    Public Property PrePaidBalance As Double
        Get
            Return mPPBal
        End Get
        Set(value As Double)
            mPPBal = value
        End Set
    End Property

    Public Property GuaranteeCode As String
        Get
            Return strGuaranteeCode
        End Get
        Set(value As String)
            strGuaranteeCode = value
        End Set
    End Property
    Public Property ApprovalCode As String
        Get
            Return strApprovalCode
        End Get
        Set(value As String)
            strApprovalCode = value
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
    Public Property CheckNumber As String
        Get
            Return strCheckNum
        End Get
        Set(value As String)
            strCheckNum = Trim(value)
        End Set
    End Property
    Public Property AccountNumber As String
        Get
            Return strAccountNum
        End Get
        Set(value As String)
            strAccountNum = Trim(value)
        End Set
    End Property

    Public Property RouteNumber As String
        Get
            Return strRouteNum
        End Get
        Set(value As String)
            strRouteNum = Trim(value)
        End Set
    End Property
    Public Property CheckAmount As Double
        Get
            Return mCheckAmt
        End Get
        Set(value As Double)
            mCheckAmt = value
        End Set
    End Property
    Public Property Fee As Double
        Get
            Return mFee1Amt
        End Get
        Set(value As Double)
            mFee1Amt = value
        End Set
    End Property

    Public Property NetAmt As Double
        Get
            Return mNetAmt
        End Get
        Set(value As Double)
            mNetAmt = value
        End Set
    End Property

    Public Property CheckCategoryID As Integer
        Get
            Return iCheckCategoryID
        End Get
        Set(value As Integer)
            iCheckCategoryID = value
        End Set
    End Property

    Public Function CheckVerify() As Boolean
        StoreID = oCheckCashing.GetKioskSettings("StoreID", oFlags.WorkstationID)
        Dim oRet As Boolean = False
        Dim strResultCode As String = ""
        If CompanyPhone = "" Then CompanyPhone = Space(10)
        Dim oDs As DataSet
        If Not oFlags.SimCheckVerify Then
            Dim traceMsg As String = "Calling CheckVerification(" + CheckNumber + ", " + RouteNumber + ", " + AccountNumber + ", " + CustomerID + ", " + CheckAmount.ToString + ", " + CheckDate.ToString + ", " + CompanyName + ", " + PayeeAuthCode + ", " + CompanyPhone + ", " + CompanyID.ToString + ", " + TransactionNumber.ToString + ", " + StoreID.ToString + ", " + FirstName + ", " + LastName + ", " + DOB.ToString + ", " + Zip
            'oCheckServ.Trace(traceMsg)
            oDs = oVerify.CheckVerification(CheckNumber, RouteNumber.PadLeft(9, "0"), AccountNumber, CustomerID.ToString, CheckAmount, CheckDate, CompanyName, PayeeAuthCode, CompanyPhone, CompanyID.ToString, TransactionNumber, StoreID, FirstName, LastName, DOB, Trim(Zip), New Kiosk.CheckVerify.BusinessLogic)
            For Each oT In oDs.Tables
                For Each oRow In oT.Rows
                    For Each oC In oT.Columns
                        If Not oRow(oC) Is Nothing Then
                            'oCheckServ.Trace("CV" + oC.Caption + " " + oRow(oC))
                            If oC.Caption = "RESULT_CODE" Then
                                strResultCode = oRow(oC)
                            ElseIf oC.Caption = "APPROVAL_CODE" Then
                                ApprovalCode = oRow(oC)
                            ElseIf oC.Caption = "COMPANY_ID" Then
                                CompanyID = oRow(oC)
                            ElseIf oC.Caption = "GURANTEE_CODE" Then
                                GuaranteeCode = oRow(oC)
                                'If oFlags.TestMode Then
                                '    oFlags.TestCode = oCheckCashing.GetCheckCaseCode()
                                'Else
                                '    oFlags.TestCode = GuaranteeCode
                                'End If
                                If GuaranteeCode <> oFlags.TestCode Then
                                    'oJourn.SaveJournal(CustomerID, Date.Now.ToString, BlockID.ToString, TransactionNumber.ToString, "CN", "KIOSK", "CurrentCase", CheckAmount, oCheckCashing.GetCurrentCaseNum)
                                    'oJourn.SaveJournal(CustomerID, Date.Now.ToString, BlockID.ToString, TransactionNumber.ToString, "GC", "KIOSK", "Unmatched result", CheckAmount, "result=" + GuaranteeCode + " expected=" + oFlags.TestCode)
                                End If
                                If GuaranteeCode.Contains("DNC") Then
                                    oRet = False
                                Else
                                    ApprovalCode = oCheckCashing.GetRandomNumber(3, 999)
                                    oRet = True
                                End If

                            End If
                        End If
                    Next
                Next
            Next
            'oCheckServ.Trace("Maker ID from CheckVerify=" + CompanyID.ToString())
        Else
            GuaranteeCode = "CSH2"
            ApprovalCode = oCheckCashing.GetRandomNumber(3, 999)
            Return True
        End If
        Return oRet
    End Function
    Public Function Authorized(ByVal iWorkstationID As Integer, ByVal iCardID As Integer) As Boolean
        Dim out1 = ""
        Dim out2 = ""
        Dim out3 = ""
        Dim ret1 As Integer = 0
        Dim ret2 = ""
        Dim ret3 As Integer = 0
        Dim OPcode As String = ""
        oCheckService.AuthorizeCheck(CustomerID, GuaranteeCode, ApprovalCode, BlockID.ToString, TransactionNumber, CheckNumber, CheckAmount, OPcode, iCardID, ConfigurationManager.AppSettings("ProviderID"), out1, out2, out3, ret1, ret2, ret3)
       
        If ret1 = "1" Then
            Return False
        End If
        If ret3 = 1 Then
            Return False
        End If

        If ret2 = "0" Then
            oFlags.PrePaidBalUpdated = True
        Else
            oFlags.PrePaidBalUpdated = False
        End If
        If out1.Length > 0 Then
            PrePaidBalance = Double.Parse(out1)
        End If
        If out2.Length > 0 Then
            PrePaidAvailable = Double.Parse(out2)
        End If
        'If out3.Length > 0 Then
        '    Fee = out3
        'End If

        If PrePaidAvailable > PrePaidBalance Then
            PrePaidAvailable = PrePaidBalance
        End If


        Dim strPPUpdate As String = ""
        If oFlags.PrePaidBalUpdated = False Then
            strPPUpdate = "Prepaid card was not updated"
        Else
            strPPUpdate = "Prepaid card was updated"
        End If
        ' oJourn.SaveJournalXML(CustomerID, Date.Now.ToString, BlockID.ToString, TransactionNumber.ToString, "CC", "KIOSK", "Authorized check", CType(NetAmt, Decimal), strPPUpdate)
        ' oJourn.SaveJournalXML(CustomerID, Date.Now.ToString, BlockID.ToString, TransactionNumber.ToString, "SM", "KIOSK", "Check Data Processed", CType(NetAmt, Decimal), "GuaranteeCode=" + GuaranteeCode + "|ApprovalCode=" + ApprovalCode + "|CompanyID=" + CompanyID.ToString)

        'oQData.QueueCode = "OW"
        'oQData.PostToQueue()


        Return True
    End Function
    Public Sub CreateTransaction(ByVal tranType As String, ByVal sec_tran_type As String, ByVal iBlockID As Integer, ByVal iActID As Integer, dAmt As Decimal)
        Dim xmlEle As XmlNode = oCheckCashing.CreateTransactionNum(CustomerID, tranType, sec_tran_type, iBlockID, iActID, dAmt)
        TransactionNumber = CType(xmlEle.ChildNodes(0).InnerText, Integer)
        BlockID = CType(xmlEle.ChildNodes(1).InnerText, Integer)
    End Sub
    Public Function CheckReviewed() As Boolean
        oCheckServ.Url = ConfigurationManager.AppSettings("CheckServiceURL")
        oCheckService.Url = ConfigurationManager.AppSettings("CheckServicesURL")
        oCheckServ.Trace("this checkserviceURL: " + oCheckService.Url.ToString())
        oCheckServ.Trace("oCheckServ: " + oCheckService.Url.ToString())
        Dim bReviewed As Boolean = False
        If oFlags.TestReview Then
            Dim tmpStr As String
            tmpStr = oCheckCashing.GetTestData("CheckAmount", 0)
            If tmpStr = "" Then
                mCheckAmt = 0
            Else
                mCheckAmt = CType(tmpStr, Double)
            End If
            CheckDate = CType(oCheckCashing.GetTestData("CheckDate", 0), Date)
            If CheckDate = Nothing Then CheckDate = Date.Now

            tmpStr = oCheckCashing.GetTestData("CompanyID", 0)
            CompanyName = oCheckCashing.GetTestData("CompanyName", 0)
            If CompanyName = "" Then
                CompanyName = Space(10)
            End If
            If tmpStr = "" Then CompanyID = 0 Else CompanyID = CType(tmpStr, Integer)
            oCheckCashing.InsChecks(BlockID, TransactionNumber, CustomerID)
            oCheckCashing.UpdateCheckReview(BlockID, CheckAmount, CheckDate)
            oFlags.ImageVerify = 1
            oFlags.PayeeVerified = 1
            oFlags.CheckSigned = 1
            oFlags.CheckAltered = 0
            oFlags.TestReview = False
            oFlags.CheckReviewed = True
            Return True
        End If
        Dim xmlElem As XmlElement
        Dim strXML As String
        Dim xmlRes As New XmlDocument
        If Not oFlags.InPositiveFile Then
            'Dim oXml As XmlElement = oCheckCashing.HasCheckBeenReviewed(AccountNumber.Trim, RouteNumber.Trim, CheckNumber.Trim, CustomerID, oFlags.WorkstationID)
            'If Not oXml.FirstChild Is Nothing Then
            '    BlockID = Integer.Parse(oXml.FirstChild.InnerText)
            'End IfDim traceMsg As String = "Calling CheckVerification(" + CheckNumber + " " + RouteNumber + " " + AccountNumber + " " + CustomerID + ", " + CheckAmount.ToString + ", " + CheckDate.ToString + " " + CompanyName + " " + PayeeAuthCode + " " + CompanyPhone + " " + CompanyID.ToString + " " + TransactionNumber.ToString + " " + StoreID.ToString + " " + FirstName + " " + LastName + " " + DOB.ToString + " " + Zip
            Dim traceMsg As String = "Calling CheckReviewedXML(" + CustomerID + " " + BlockID.ToString + " " + TransactionNumber.ToString + " " + oFlags.WorkstationID.ToString
            oCheckServ.Url = ConfigurationManager.AppSettings("CheckServiceURL")
            oCheckService.Url = ConfigurationManager.AppSettings("CheckServicesURL")
            oCheckServ.Trace("checkserviceURL: " + oCheckService.Url.ToString())
            strXML = oCheckService.CheckReviewedXML(CustomerID, BlockID, TransactionNumber, oFlags.WorkstationID)
            xmlRes.LoadXml(strXML)
            xmlElem = xmlRes.FirstChild
            Dim iRes As Integer = CType(xmlElem.FirstChild.FirstChild.InnerText, Integer)
            If iRes = 1 Then
                Return False
                Exit Function
            End If

            For Each oNode In xmlElem.FirstChild.ChildNodes
                ' oCheckServ.Trace(oNode.InnerText.ToString)
                If oNode.Name = "CheckAmount" Then
                    CheckAmount = CType(oNode.InnerText, Double)
                ElseIf oNode.Name = "CheckDate" Then
                    CheckDate = CType(oNode.InnerText, Date)
                ElseIf oNode.Name = "CompanyName" Then
                    CompanyName = oNode.InnerText
                    If CompanyName = "" Then
                        CompanyName = Space(10)
                    End If
                ElseIf oNode.Name = "MakerPhone" Then
                    CompanyPhone = oNode.InnerText
                ElseIf oNode.Name = "CompanyID" Then
                    If oNode.InnerText = "" Then
                        CompanyID = 0
                    Else
                        CompanyID = CType(oNode.InnerText, Integer)
                    End If
                ElseIf oNode.name = "result1" Then
                    If oNode.InnerText = "False" Then
                        oFlags.ImageVerify = False
                    Else
                        oFlags.ImageVerify = True
                    End If
                ElseIf oNode.name = "result2" Then
                    If oNode.InnerText = "False" Then
                        oFlags.PayeeVerified = False
                    Else
                        oFlags.PayeeVerified = True
                    End If
                ElseIf oNode.name = "result3" Then
                    If oNode.innertext = "False" Then
                        oFlags.CheckSigned = False
                    Else
                        oFlags.CheckSigned = True
                    End If
                ElseIf oNode.name = "result4" Then
                    If oNode.InnerText = "false" Then
                        oFlags.CheckAltered = False
                    Else
                        oFlags.CheckAltered = True
                    End If
                ElseIf oNode.name = "Status" Then
                    If oNode.InnerText = "Review" Then
                        oFlags.CheckReviewed = False
                    Else
                        oFlags.CheckReviewed = True
                    End If
                ElseIf oNode.name = "CheckCategoryID" Then
                    CheckCategoryID = oNode.InnerText
                End If

            Next
        Else
            oFlags.CheckReviewed = True
        End If
        If CompanyID = 1111 And CompanyName = "" Then
            CompanyID = 0
        End If
        'Roshelle 2/3/14 -- CheckAmount can no longer be used to determine if check has been reviewed as we are saving it before checkReview.
        'If (CheckAmount > 0) Then
        '    Return True
        'Else
        '    Return False
        'End If
        Return oFlags.CheckReviewed

    End Function
    Public Sub New(ByVal iBlockID As Integer, iTransNum As Integer, iCustomerId As Integer, ByVal WorkstationID As Integer)
        BlockID = iBlockID
        TransactionNumber = iTransNum
        CustomerID = iCustomerId
        'Roshelle 2 / 6 / 15
        oCheckServ.Url = ConfigurationManager.AppSettings("CheckServiceURL")
        oCheckService.Url = ConfigurationManager.AppSettings("CheckServicesURL")
        'oChkService.Url = ConfigurationManager.AppSettings("CheckServiceURL")
        'oJourn.Url = ConfigurationManager.AppSettings("SaveJournalURL")
        oFlags = New clsCheck.clsCheckFlags(WorkstationID)
    End Sub
    Public Sub New()
        oCheckServ.Url = ConfigurationManager.AppSettings("CheckServiceURL")
        oCheckService.Url = ConfigurationManager.AppSettings("CheckServicesURL")
    End Sub
    Public Class clsCheckFlags
        Public Property SimCheckVerify As Boolean = False
        Public Property TestMode As Boolean = False
        Public Property TestCode As String = ""
        Public Property PrePaidBalUpdated As Boolean = False
        Public Property TestReview As Boolean = False
        Public Property ImageVerify As Boolean = False
        Public Property PayeeVerified As Boolean = False
        Public Property CheckSigned As Boolean = False
        Public Property CheckAltered As Boolean = False
        Public Property CheckReviewed As Boolean = False
        Public Property InPositiveFile As Boolean = False
        Public Property PhotoIDMatches As Boolean = False
        Public Property WorkstationID As Integer = 0
        Public Property SimulateAll As Integer = False
        Public Sub New(ByVal pWorkStationID As Integer)
            WorkstationID = pWorkStationID
            Dim oCheckCashing As New clsDataModule.clsInterface()
            TestMode = CType(oCheckCashing.GetKioskSettings("TestMode", WorkstationID), Integer)
            SimCheckVerify = CType(oCheckCashing.GetKioskSettings("SimCheckVerify", WorkstationID), Integer)

        End Sub
        Public Sub New()

        End Sub

    End Class
End Class
