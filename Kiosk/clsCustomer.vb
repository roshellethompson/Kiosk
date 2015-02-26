Imports System.Xml
Imports System.Configuration
Imports System

Public Class clsCustomer
    Public Property oFlags As clsCustomer.clsCustomerFlags
    Public Property oIDs As clsCustomerPhotoIDs
    Public oErr As clsError
    Dim iCustID As Integer = 0
    Dim strCustID As String = "0"
    Dim oCreateCust As New CreateCustomerID.CreateCustomerID
    Dim strPhone As String = ""
    Dim strSSN As String = ""
    Dim strPIN As String = ""
    Dim strTrack1 As String = ""
    Dim strTrack2 As String = ""
    Dim strTrack3 As String = ""
    Dim strTrack3Rem As String = ""
    Dim strPhotoID As String = ""
    Dim strFirstName As String = ""
    Dim strLastName As String = ""
    Dim dDOB As Date = Date.Now
    Dim iScanID As Integer = 0
    Dim dPhotoIDExpiration As New Date
    Public oLang As New clsLanguage
    Dim strAddr1 As String = ""
    Dim strCity As String = ""
    Dim strZip As String = ""
    Dim iActNum As Integer = 0
    Dim strStatus As String = ""
    Public oCard As clsCard
    Dim strIDType As String
    Dim strState As String = ""
    Dim cRegType As Char = ""
    Dim iImageID As Integer = 0
    Dim strRevType As String = ""
    Public oCheck As New clsCheck()
    Public oBill As New clsBillPay()
    Public Property Password As String = ""
    Public Property EmailAddress As String = ""
    Public Property Workstation As Integer = 0
    Public Property BackGroundCheckCode As Integer = 0
    Public oCustomer As clsCustomer
    Public Property IDCode As String = ""
    Public Property CustACK As Integer = 0
    Public Property ImageID As Integer
        Get
            Return iImageID
        End Get
        Set(ByVal value As Integer)
            iImageID = value
        End Set
    End Property
    Public Property RegType As Char
        Get
            Return cRegType
        End Get
        Set(ByVal value As Char)
            cRegType = value
        End Set
    End Property
    Public Property StateID As String
        Get
            Return strState
        End Get
        Set(ByVal value As String)
            strState = value
        End Set
    End Property

    Public Property RegSource As String

    Public Property RegFlag As String
    Public Property UserSalt As String
    Public Function Clone() As clsCustomer
        Return Me
    End Function

    Public Property IDType As String
        Get
            Return strIDType
        End Get
        Set(value As String)
            strIDType = value
        End Set
    End Property
    Dim oReg As New RegisterReview.RegReview
    Dim bgCheck As New BgCheck
    Dim strErr As String = ""
    'Dim oChkService As New CheckService.ICheckServiceservice
    Dim oCheckCashing As New clsDataModule.clsInterface
    Public Property Status As String
        Get
            Return strStatus
        End Get
        Set(value As String)
            strStatus = value
        End Set
    End Property
    Public Property ErrorText As String
        Get
            Return strErr
        End Get
        Set(value As String)
            strErr = value
        End Set
    End Property
    Public Property PhotoIDExpiration As Date
        Get
            Return dPhotoIDExpiration
        End Get
        Set(value As Date)
            dPhotoIDExpiration = value
        End Set
    End Property
    Public Property AccountNumber As Integer
        Get
            Return iActNum
        End Get
        Set(value As Integer)
            iActNum = value
            oCheck.vaultAccount = iActNum
        End Set
    End Property
    Public Property Zip As String
        Get
            Return strZip
        End Get
        Set(value As String)
            strZip = value
            oCheck.Zip = value
        End Set
    End Property
    Public Property City As String
        Get
            Return strCity
        End Get
        Set(value As String)
            strCity = value
        End Set
    End Property
    Public Property Address As String
        Get
            Return strAddr1
        End Get
        Set(value As String)
            strAddr1 = value
        End Set
    End Property



    Public Property ScanID As Integer
        Get
            Return iScanID
        End Get
        Set(value As Integer)
            iScanID = value
        End Set
    End Property

    Public Property CustomerString As String
        Get
            Return strCustID
        End Get
        Set(value As String)
            strCustID = value
            If IsNumeric(strCustID) Then CustomerID = CType(strCustID, Integer)
        End Set
    End Property
    Public Property DOB As Date
        Get
            Return dDOB
        End Get
        Set(value As Date)
            dDOB = value
            oCheck.DOB = value
        End Set
    End Property
    Public Property LastName As String
        Get
            Return strLastName
        End Get
        Set(value As String)
            strLastName = value
            oCheck.LastName = value
        End Set
    End Property
    Public Property FirstName As String
        Get
            Return strFirstName
        End Get
        Set(value As String)
            strFirstName = value
            oCheck.FirstName = value
        End Set
    End Property
    Public Property PhotoID As String
        Get
            Return strPhotoID
        End Get
        Set(value As String)
            strPhotoID = value
        End Set
    End Property

    Public Property SSN As String
        Get
            Return strSSN
        End Get
        Set(value As String)
            strSSN = value
        End Set
    End Property
    Public Property Phone As String
        Get
            Return strPhone
        End Get
        Set(value As String)
            strPhone = value
        End Set
    End Property
    Public Property ActID As Integer = 0
    Public Property CustomerID As Integer
        Get
            Return iCustID
        End Get
        Set(value As Integer)
            iCustID = value
            oCheck.CustomerID = value

        End Set
    End Property
    Public Function CompareCustomerData() As Integer

        Dim xmlDoc As New XmlDocument
        Dim strXML As String = oCreateCust.CompareCustomerDataXML(ScanID)
        xmlDoc.LoadXml(strXML)
        Dim Ret1 As Integer = 0
        Dim Ret2 As Integer = 0
        Ret1 = 1
        Ret2 = 2
        CustomerID = 0
        Dim xmlIter As XmlElement
        For Each xmlIter In xmlDoc.ChildNodes(0).ChildNodes(0).ChildNodes
            If xmlIter.Name = "ReturnCode" Then
                Ret1 = Int32.Parse(xmlIter.InnerText)
            ElseIf xmlIter.Name = "ReturnCode2" Then
                Ret2 = Int32.Parse(xmlIter.InnerText)
            ElseIf xmlIter.Name = "CustomerID" Then
                CustomerID = Int32.Parse(xmlIter.InnerText)
            End If
        Next
        If CustomerID > 0 Then
            If Ret1 = 1 Then
                Return -1
            ElseIf Ret2 = 1 Then
                Return 0
            End If
        Else
            Return -1
        End If
        Return -1
    End Function

    ''' <summary>
    ''' Return database information from customer id
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetCustomerDataFromID() As Boolean


        Dim cRet2 As Char = ""

        Dim xmlCust As XmlElement = oCheckCashing.GetCustomerDataFromID(PhotoID, DOB, StateID, CustomerID, oCheck.RouteNumber, oCheck.AccountNumber, CType(oCheck.CheckNumber, Integer), oCheck.CheckAmount, oFlags.WorkstationID)
        If xmlCust.ChildNodes.Count = 0 Or CustomerID = 0 Then
            Return False
        End If
        Dim xmlEle As XmlNode
        For Each xmlEle In xmlCust.ChildNodes
            If xmlEle.Name = "First" Then
                FirstName = xmlEle.InnerText
            ElseIf xmlEle.Name = "CustomerID" Then
                CustomerID = Int32.Parse(xmlEle.InnerText)
            ElseIf xmlEle.Name = "Last" Then
                LastName = xmlEle.InnerText
            ElseIf xmlEle.Name = "DOB" Then
                If xmlEle.InnerText = "" Then
                    DOB = Date.Now
                Else
                    DOB = Date.Parse(xmlEle.InnerText)
                End If
            ElseIf xmlEle.Name = "SSN" Then
                If SSN = "" Then
                    SSN = xmlEle.InnerText
                End If
            ElseIf xmlEle.Name = "PHOTO_ID" Then
                PhotoID = xmlEle.InnerText
            ElseIf xmlEle.Name = "ReturnCode2" Then
                cRet2 = xmlEle.InnerText
            ElseIf xmlEle.Name = "Zip" Then
                Zip = xmlEle.InnerText
            ElseIf xmlEle.Name = "DOB" Then
                If xmlEle.InnerText = "" Then
                    DOB = Date.Now
                    Continue For
                End If
                DOB = Date.Parse(xmlEle.InnerText)
            ElseIf xmlEle.Name = "LangID" Then
                oLang.LangID = xmlEle.InnerText
                FillCaptions()
            ElseIf xmlEle.Name = "PAN" Then
                If xmlEle.InnerText <> "" And oCard.PAN = "" Then
                    oCard.PAN = xmlEle.InnerText
                End If
            ElseIf xmlEle.Name = "VaultAccountID" Then
                If xmlEle.InnerText <> "" Then
                    Me.AccountNumber = xmlEle.InnerText
                End If
            ElseIf xmlEle.Name = "CheckDate" Then
                If xmlEle.InnerText <> "" Then
                    oCheck.CheckDate = Date.Parse(xmlEle.InnerText)

                End If
            ElseIf xmlEle.Name = "State" Then
                StateID = xmlEle.InnerText
            ElseIf xmlEle.Name = "BGCheck" Then
                If xmlEle.InnerText = "P" Then
                    oFlags.BackGroundCheck = True
                Else
                    oFlags.BackGroundCheck = False
                End If
            ElseIf xmlEle.Name = "Customer_ACK" Then
                If xmlEle.InnerText = "1" Then
                    oFlags.CustACK = True
                Else
                    oFlags.CustACK = False
                End If

            ElseIf xmlEle.Name = "ActID" Then
                If Not xmlEle.InnerText = "" Then
                    ActID = xmlEle.InnerText
                End If

            ElseIf xmlEle.Name = "LostCardCode" Then

            ElseIf xmlEle.Name = "Email" Then
                If Not xmlEle.InnerText = "" Then
                    EmailAddress = xmlEle.InnerText
                End If

            ElseIf xmlEle.Name = "user_salt" Then
                If Not xmlEle.InnerText = "" Then
                    UserSalt = xmlEle.InnerText
                End If
            ElseIf xmlEle.Name = "CardID" Then
                If Not xmlEle.InnerText = "" Then
                    oCard.CardID = xmlEle.InnerText
                End If
            End If
        Next

        If FirstName = "" Or LastName = "" Or DOB = Date.Now Or Zip = "" Or CustomerID = 0 Then
            Return False
        End If
        If cRet2 = "M" Then
            oFlags.InPositiveFile = True
        Else
            oFlags.InPositiveFile = False
        End If

        Return True

    End Function
    Public Function ProcessLostCard()
        oCard.oCustomer = Me
        If Me.GetCustomerDataFromID() Then
            Return True
        Else
            Return False
        End If

    End Function
    Public Function GetCustomerDataFromScan() As Boolean

        Dim cRet2 As Char = ""
        Dim strScanID As String = ""
        If ScanID = 0 Then
            Return False
        End If
        Dim xmlCust As XmlElement = oCheckCashing.GetCustomerDataFromScan(ScanID)
        If xmlCust.ChildNodes.Count = 0 Then
            oErr.ErrCode = 4
            Return False
        Else
            strScanID = xmlCust.ChildNodes(0).InnerText
        End If

        If strScanID = "" Then strScanID = "0"
        ScanID = CType(strScanID, Integer)
        Dim xmlEle As XmlNode
        For Each xmlEle In xmlCust.ChildNodes
            If xmlEle.Name = "DOB" Then
                If xmlEle.InnerText <> "" Then
                    DOB = Date.Parse(xmlEle.InnerText)
                Else
                    Return False
                End If
            ElseIf xmlEle.Name = "First" Then
                FirstName = xmlEle.InnerText
            ElseIf xmlEle.Name = "Last" Then
                LastName = xmlEle.InnerText
            ElseIf xmlEle.Name = "PHOTO_ID" Then

                PhotoID = xmlEle.InnerText
            ElseIf xmlEle.Name = "Address1" Then
                Address = xmlEle.InnerText
            ElseIf xmlEle.Name = "City" Then
                City = xmlEle.InnerText
            ElseIf xmlEle.Name = "Zip" Then
                Zip = xmlEle.InnerText
            ElseIf xmlEle.Name = "ReturnCode2" Then
                cRet2 = xmlEle.InnerText
            ElseIf xmlEle.Name = "Expiration_Date" Then
                PhotoIDExpiration = Date.Parse(xmlEle.InnerText)
            ElseIf xmlEle.Name = "Status" Then
                Status = xmlEle.InnerText
                If Status.Trim() = "Reviewed" Or Status.Trim() = "Finished" Then
                    oFlags.Reviewed = True
                Else
                    oFlags.Reviewed = False
                End If
            ElseIf xmlEle.Name = "PhotoIDMatches" Then
                If xmlEle.InnerText = "True" Then
                    oFlags.PhotoIDMatches = True
                End If
            ElseIf xmlEle.Name = "CustomerImageVerify" Then
                If xmlEle.InnerText = "True" Then
                    oFlags.ImageVerify = True
                End If
            ElseIf xmlEle.Name = "State" Then
                StateID = xmlEle.InnerText
            ElseIf xmlEle.Name = "Customer_ACK" Then
                If xmlEle.InnerText = "1" Then
                    oFlags.CustACK = True
                Else
                    oFlags.CustACK = False
                End If

            ElseIf xmlEle.Name = "LostCardCode" Then

            End If

        Next


        Return True

    End Function
    Public Function GetRegistrationDataFromPAN(ByVal strPAN As String) As Boolean

        Dim cRet2 As Char = ""
        Dim strScanID As String = ""
        Dim xmlCust As XmlElement = oCheckCashing.GetRegistrationDataFromPan(strPAN)
        If xmlCust.ChildNodes.Count = 0 Then
            Return False
        End If

        Dim xmlEle As XmlNode
        For Each xmlEle In xmlCust.ChildNodes
            If xmlEle.InnerText = "" Then
                Continue For
            End If
            If xmlEle.Name = "DOB" Then
                If xmlEle.InnerText <> "" Then
                    DOB = Date.Parse(xmlEle.InnerText)
                End If
            ElseIf xmlEle.Name = "First" Then
                FirstName = xmlEle.InnerText
            ElseIf xmlEle.Name = "Last" Then
                LastName = xmlEle.InnerText
            ElseIf xmlEle.Name = "Photo_ID" Then

                PhotoID = xmlEle.InnerText
            ElseIf xmlEle.Name = "Address1" Then
                Address = xmlEle.InnerText
            ElseIf xmlEle.Name = "City" Then
                City = xmlEle.InnerText
            ElseIf xmlEle.Name = "Zip" Then
                Zip = xmlEle.InnerText
            ElseIf xmlEle.Name = "ReturnCode2" Then
                cRet2 = xmlEle.InnerText
            ElseIf xmlEle.Name = "Expiration_Date" Then
                PhotoIDExpiration = Date.Parse(xmlEle.InnerText)
            ElseIf xmlEle.Name = "Status" Then

                Status = Trim(xmlEle.InnerText)
                If Status = "Finished" Or Status = "Reviewed" Then
                    oFlags.Reviewed = True
                End If
            ElseIf xmlEle.Name = "PhotoIDMatches" Then
                oFlags.PhotoIDMatches = CType(xmlEle.InnerText, Boolean)
            ElseIf xmlEle.Name = "CustomerImageVerify" Then
                oFlags.ImageVerify = (CType(xmlEle.InnerText, Boolean))
            ElseIf xmlEle.Name = "State" Then
                StateID = xmlEle.InnerText
            ElseIf xmlEle.Name = "CustomerID" Then
                CustomerID = Int32.Parse(xmlEle.InnerText)
            ElseIf xmlEle.Name = "ScanID" Then
                ScanID = xmlEle.InnerText
            ElseIf xmlEle.Name = "BGCheck" Then
                If xmlEle.InnerText = "P" Then
                    oFlags.BackGroundCheck = True
                Else
                    oFlags.BackGroundCheck = False
                End If

            End If

        Next

        Return True
    End Function


    Public Function EncryptValue(ByVal strClear As String)
        Return TPSUtilities.AESEncryption.Encrypt(strClear, "tr@ns@ct", "123-98pnw-f9pcj9-qruk1-2uh0q34yh", "SHA1", 2, "16CHARSLONG12345", 256)
    End Function

    Public Function GetCustomerDataFromPAN(ByVal strPAN As String) As Boolean

        Dim cRet2 As Char = ""
        Dim strScanID As String = ""
        Dim xmlCust As XmlElement = oCheckCashing.GetCustomerDataFromPan(strPAN)
        If xmlCust.ChildNodes.Count = 0 Then
            Return False
        End If

        Dim xmlEle As XmlNode
        For Each xmlEle In xmlCust.ChildNodes
            If xmlEle.InnerText = "" Then
                Continue For
            End If
            If xmlEle.Name = "DOB" Then
                If xmlEle.InnerText <> "" Then
                    DOB = Date.Parse(xmlEle.InnerText)
                End If
            ElseIf xmlEle.Name = "First" Then
                FirstName = xmlEle.InnerText
            ElseIf xmlEle.Name = "Last" Then
                LastName = xmlEle.InnerText
            ElseIf xmlEle.Name = "Photo_ID" Then

                PhotoID = xmlEle.InnerText
            ElseIf xmlEle.Name = "Address1" Then
                Address = xmlEle.InnerText
            ElseIf xmlEle.Name = "City" Then
                City = xmlEle.InnerText
            ElseIf xmlEle.Name = "Zip" Then
                Zip = xmlEle.InnerText
            ElseIf xmlEle.Name = "ReturnCode2" Then
                cRet2 = xmlEle.InnerText
            ElseIf xmlEle.Name = "Expiration_Date" Then
                PhotoIDExpiration = Date.Parse(xmlEle.InnerText)
            ElseIf xmlEle.Name = "Status" Then

                Status = Trim(xmlEle.InnerText)

            ElseIf xmlEle.Name = "PhotoIDMatches" Then
                oFlags.PhotoIDMatches = CType(xmlEle.InnerText, Boolean)
            ElseIf xmlEle.Name = "CustomerImageVerify" Then
                oFlags.ImageVerify = (CType(xmlEle.InnerText, Boolean))
            ElseIf xmlEle.Name = "State" Then
                StateID = xmlEle.InnerText
            ElseIf xmlEle.Name = "CustomerID" Then
                CustomerID = Int32.Parse(xmlEle.InnerText)
            ElseIf xmlEle.Name = "Registration_Source" Then
                RegSource = Trim(xmlEle.InnerText)
            ElseIf xmlEle.Name = "Registration_Flag" Then
                RegFlag = Trim(xmlEle.InnerText)
            ElseIf xmlEle.Name = "Customer_ACK" Then
                If xmlEle.InnerText = "1" Then
                    oFlags.CustACK = True
                Else
                    oFlags.CustACK = False
                End If

            ElseIf xmlEle.Name = "LostCardCode" Then
            ElseIf xmlEle.Name = "cardID" Then
                oCard.CardID = Int32.Parse(xmlEle.InnerText)
                If oCard.CardID > 0 Then
                    oFlags.CustomerExists = True
                End If
            ElseIf xmlEle.Name = "CDType" Then
                oCard.TransactCardType = Trim(xmlEle.InnerText)
            ElseIf xmlEle.Name = "Email" Then
                EmailAddress = Trim(xmlEle.InnerText)
            ElseIf xmlEle.Name = "ID_Code" Then
                IDType = Trim(xmlEle.InnerText)
            ElseIf xmlEle.Name = "SSN" Then
                SSN = Trim(xmlEle.InnerText)
            ElseIf xmlEle.Name = "user_salt" Then
                UserSalt = Trim(xmlEle.InnerText)
            ElseIf xmlEle.Name = "LangID" Then
                oLang.LangID = Trim(xmlEle.InnerText)
            End If

        Next

        Return True

    End Function
    Public Sub New()

    End Sub
    Public Sub New(ByVal iWorkstationID As Integer)
        oCreateCust.Url = ConfigurationManager.AppSettings("CreateCustomerIDURL")
        oReg.Url = ConfigurationManager.AppSettings("RegistrationReviewURL")
        'oChkService.Url = ConfigurationManager.AppSettings("CheckServiceURL")
        'oJourn.Url = ConfigurationManager.AppSettings("SaveJournalURL")
        oFlags = New clsCustomer.clsCustomerFlags(iWorkstationID)
        oErr = New clsError(Me)
        oCheck = New clsCheck(0, 0, 0, iWorkstationID)
        oCard = New clsCard(oFlags.WorkstationID, oFlags.CustomerExists, oErr)
        oIDs = New clsCustomerPhotoIDs()
        If oLang.LangID = 0 Then
            oLang.LangID = 1
        End If
        If oLang.LanguageName = "" Then
            oLang.LanguageName = "English"
        End If
        FillCaptions()
    End Sub


    Public Sub FillCaptions()
        oLang.Refresh()
    End Sub
    ''' <summary>
    ''' REturns true when registration is reviewed
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RegistrationReviewed() As Boolean
        If oFlags.TestMode Then
            If oFlags.Register = True Or oFlags.ReRegister Or oFlags.ScrapDragon Or oFlags.CardManagement Then
                oCheckCashing.UpdateRegReview(ScanID, FirstName, LastName, DOB)
            End If
        End If
        Dim strXMl = oReg.RegisterReviewedServiceXML(ScanID)
        Dim oXML As New XmlDocument
        oXML.LoadXml(strXMl)
        If oXML.FirstChild Is Nothing Then
            Return False
        End If
        Dim xmlEle As XmlElement
        For Each xmlEle In oXML.ChildNodes(0).ChildNodes(0)
            If xmlEle.Name = "ReturnCode" Then
                If Trim(xmlEle.InnerText) = "1" Then
                    oErr.ErrCode = 33
                    Return False
                End If
            End If
            If xmlEle.Name = "Status" Then
                If Trim(xmlEle.InnerXml) = "Reviewed" Or _
                     Trim(xmlEle.InnerXml) = "Finished" Then
                    oFlags.Reviewed = True
                Else
                    oFlags.Reviewed = False
                End If
            ElseIf xmlEle.Name = "FirstName" Then
                FirstName = Trim(xmlEle.InnerText)
            ElseIf xmlEle.Name = "LastName" Then
                LastName = Trim(xmlEle.InnerText)
            ElseIf xmlEle.Name = "DOB" Then
                If xmlEle.InnerText = "" Then
                    DOB = Date.Now
                Else
                    DOB = Date.Parse(Trim(xmlEle.InnerText))
                End If
            ElseIf xmlEle.Name = "CustomerImageVerify" Then
                oFlags.ImageVerify = CType(xmlEle.InnerText, Boolean)
            ElseIf xmlEle.Name = "PhotoIDMatches" Then
                oFlags.PhotoIDMatches = CType(xmlEle.InnerText, Boolean)
            ElseIf xmlEle.Name = "StateCode" Then
                StateID = Trim(xmlEle.InnerText)
            ElseIf xmlEle.Name = "PhotoID" Then
                PhotoID = Trim(xmlEle.InnerText)
            ElseIf xmlEle.Name = "IDType" Then
                IDType = xmlEle.InnerText
            ElseIf xmlEle.Name = "ReviewType" Then
                oFlags.ReviewType = Trim(xmlEle.InnerText)
                If oFlags.ReviewType = "S" Then
                    oFlags.ScrapDragon = True
                End If
            ElseIf xmlEle.Name = "ExpirationDate" Then
                PhotoIDExpiration = Date.Parse(Trim(xmlEle.InnerText))
            End If
        Next

        Return oFlags.Reviewed

    End Function
    'Public Function CreateCustomer() As Boolean
    '    Dim xmlResult As String = oCreateCust.CreateCustomerXML(ScanID, SSN, Phone, oCheckCashing.GetLangID(oLang.LanguageName), Workstation, oCard.TransactCardType)
    '    Dim xmlDoc As New XmlDocument
    '    xmlDoc.LoadXml(xmlResult)
    '    Dim oRet As Integer
    '    For Each XmlNode In xmlDoc.ChildNodes(0).ChildNodes(0).ChildNodes
    '        If XmlNode.Name = "CustomerID" Then
    '            CustomerString = XmlNode.InnerText
    '            If CustomerString = "" Then
    '                CustomerString = "0"
    '            End If

    '            CustomerID = CustomerString
    '            If CustomerID > 0 Then
    '                If GetCustomerDataFromID() = False Then
    '                    Return False
    '                End If
    '            Else
    '                Return False
    '            End If
    '        ElseIf XmlNode.name = "vaultAccount" Then
    '            oCard.ActID = XmlNode.innertext
    '        ElseIf XmlNode.Name = "ReturnCode" Then
    '            oRet = Trim(XmlNode.InnerText)
    '            If oRet = 2 Then
    '                oFlags.CustomerExists = True
    '            ElseIf oRet = 1 Then
    '                Return False
    '            End If
    '        Else
    '        End If
    '    Next
    '    Return True
    'End Function
    Public Function LookupByPan()
        Dim xmlItem As XmlElement = oCheckCashing.GetCustomerDataFromPan(oCard.PAN)
        If xmlItem.ChildNodes.Count = 0 Or CustomerID = 0 Then
            Return False
        End If
        Dim xmlTemp As XmlNode
        For Each xmlTemp In xmlItem.ChildNodes
            If xmlTemp.Name = "First" Then
                FirstName = xmlTemp.InnerText
            ElseIf xmlTemp.Name = "Last" Then
                LastName = xmlTemp.InnerText
            ElseIf xmlTemp.Name = "DOB" Then
                If xmlTemp.InnerText = "" Then
                    DOB = Date.Now
                Else
                    DOB = Date.Parse(xmlTemp.InnerText)
                End If
            ElseIf xmlTemp.Name = "SSN" Then
                If SSN = "" Then
                    SSN = xmlTemp.InnerText
                End If
            ElseIf xmlTemp.Name = "PHOTO_ID" Then
                PhotoID = xmlTemp.InnerText
            ElseIf xmlTemp.Name = "Zip" Then
                Zip = xmlTemp.InnerText
            ElseIf xmlTemp.Name = "DOB" Then
                If xmlTemp.InnerText = "" Then
                    DOB = Date.Now
                    Continue For
                End If
                DOB = Date.Parse(xmlTemp.InnerText)
            ElseIf xmlTemp.Name = "LangID" Then
                oLang.LangID = xmlTemp.InnerText
                FillCaptions()
            ElseIf xmlTemp.Name = "PAN" Then
                If xmlTemp.InnerText <> "" And oCard.PAN = "" Then
                    oCard.PAN = xmlTemp.InnerText
                End If
            ElseIf xmlTemp.Name = "VaultAccountID" Then
                If xmlTemp.InnerText <> "" Then
                    Me.AccountNumber = xmlTemp.InnerText
                End If
            ElseIf xmlTemp.Name = "CheckDate" Then
                If xmlTemp.InnerText <> "" Then
                    oCheck.CheckDate = Date.Parse(xmlTemp.InnerText)

                End If
            ElseIf xmlTemp.Name = "State" Then
                StateID = xmlTemp.InnerText
            End If
        Next
    End Function
    Public Function DoBackGroundCheck() As Boolean
        Try
            Dim returnCode As Integer = 0
            returnCode = bgCheck.doBackgroundCheck(SSN)



            If returnCode = bgCheck.BGCHECK_FAIL Then
                ' oJourn.SaveJournal(CustomerID, Date.Now.ToString, oCheck.BlockID.ToString, oCheck.TransactionNumber.ToString, "BF", "KIOSK", "Background check failed.", 0, "")
                ErrorText = oLang.GetScreenLabel("MISSING_IDENTIFICATION")
                oFlags.BackGroundCheck = False

            Else
                ' oJourn.SaveJournal(CustomerID, Date.Now.ToString, oCheck.BlockID.ToString, oCheck.TransactionNumber.ToString, "BF", "KIOSK", "Background check succeeded.", 0, "")
            End If
            Dim dDOB As Date = Date.Parse(DOB)

            If bgCheck.DOB <> dDOB.Month.ToString().PadLeft(2, "0") + dDOB.Day.ToString().PadLeft(2, "0") + dDOB.Year.ToString Then
                ErrorText = oLang.GetScreenLabel("MISSING_IDENTIFICATION")
                oFlags.BackGroundCheck = False
            Else
                oFlags.BackGroundCheck = True
            End If
        Catch ex As Exception
            oErr.ErrCode = 6
            oFlags.BackGroundCheck = False
        End Try

    End Function
    Public Class clsCustomerFlags
        Public Property BackGroundCheck As Boolean = False
        Public Property CustACK As Boolean = False
        Public Property InPositiveFile As Boolean = False
        Public Property Reviewed As Boolean = False
        Public Property PhotoIDMatches As Boolean = False
        Public Property ImageVerify As Boolean = False
        Public Property ScrapDragon As Boolean = False
        Public Property CustomerExists As Boolean = False
        Public Property WorkstationID As Integer = 0
        Public Property TestMode As Boolean = False
        Public Property Register As Boolean = False
        Public Property CardManagement As Boolean = False
        Public Property ReRegister As Boolean = False
        Public Property ReviewType As String = ""

        Public Sub New(ByVal iWorkstationID)

            WorkstationID = iWorkstationID
        End Sub
        Public Sub New()

        End Sub
    End Class

End Class
