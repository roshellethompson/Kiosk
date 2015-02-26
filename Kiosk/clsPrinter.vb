Imports System.IO
Imports System.Printing
Imports System.Text
Imports System
Imports System.Configuration

Public Class clsPrinter
    Dim oFlags As New clsFlags
    Dim strPrintername As String = ""
    Dim strHeader As String = "TRANSACT"
    Dim strCheckAmt As String = "Check Amt: "
    Dim strFeeAmt As String = "Fee Amt: "
    Dim strPPBalAmt As String = "Prepaid Card Balance: "
    Dim strPPAvailAmt As String = "Available Card Balance: "
    Dim strNetAmt As String = "Net Amount: "
    Dim strCashAmt As String = "Cash Amt: "
    Dim mCheckAmt As Decimal = 0
    Dim mFeeAmt As Decimal = 0
    Dim mPPBalAmt As Decimal = 0
    Dim mPPAvailAMt As Decimal = 0
    Dim mNetAmt As Decimal = 0
    Dim mCashAmt As Decimal = 0
    Dim strCRLF As String = vbCrLf
    Dim strJobName As String = "Receipt"
    Dim arrPrint As New ArrayList
    Dim oServer As New PrintServer
    Dim oQueue As PrintQueue
    Dim oSettings As PrintJobSettings
    Dim oDlg As New PrintDialog
    Dim iLineNum As Integer = 1
    Dim strBarcodeText As String = ""
    Dim encoding As UTF8Encoding = New UTF8Encoding()
    Dim iCustId As Integer = 0
    Dim iReceiptNumber As Integer = 0
    Dim oCheckCashing As New clsDataModule.clsInterface
    Dim oCheckSvc As New CheckService.ICheckServiceservice
    Public oCustomer As clsCustomer

    Public Property PrinterName As String
        Get
            Return strPrintername
        End Get
        Set(value As String)
            strPrintername = value
        End Set
    End Property
    Public Property BarcodeText As String
        Get
            Return strBarcodeText
        End Get
        Set(value As String)
            strBarcodeText = value
        End Set
    End Property
    Public Sub SelectPrinter()
        PrinterName = oCheckCashing.GetKioskSettings("PrinterName", oFlags.WorkstationID)
        oQueue = New PrintQueue(oServer, PrinterName)
    End Sub
    Public Sub New()

        oCheckSvc.Url = ConfigurationManager.AppSettings("CheckServiceURL")
        SelectPrinter()
    End Sub
    Public Function WriteToPrinter(ByVal arrByte As Byte())
        Dim oPSInfo As PrintSystemJobInfo = oQueue.AddJob(strJobName)
        Dim oStrm As Stream = oPSInfo.JobStream
        oStrm.Write(arrByte, 0, arrByte.Length)
        oStrm.Close()
    End Function



    Public Function PrintRegTicket() As Boolean
        Dim command As Byte() = New Byte() {}
        oFlags.PrintReg = True
        Cursor.Current = Cursors.WaitCursor

        Try
            Dim n1 As String = Chr(&H4) '' option 4 is Code39
            Dim n2int As Integer = 1 ''shifted + 1 to fix index being at zero.
            Dim n2 As String = n2int.ToString("X") ''Under-bar and line feed options, 
            Dim n3 As String = String.Empty

            n3 = Chr(&H39)
            Dim n4 As String = 80 ''The height


            Dim commandList As List(Of Byte) = New List(Of Byte)
            commandList.AddRange(New Byte() {&H1B, &H1D, &H61, &H1}) 'Center Alignment - Refer to Pg. 3-29
            commandList.AddRange(encoding.GetBytes("TRANSACT" + vbCrLf))
            commandList.AddRange(encoding.GetBytes(oCheckCashing.GetKioskSettings("Address", oFlags.WorkstationID) + vbCrLf))
            commandList.AddRange(encoding.GetBytes(oCheckCashing.GetKioskSettings("City", oFlags.WorkstationID) + ", " + oCheckCashing.GetKioskSettings("Zip", oFlags.WorkstationID) + vbCrLf))
            commandList.AddRange(New Byte() {&H1B, &H1C, &H70, &H1, &H0, &HD, &HA}) 'Stored Logo Printing - Refer to Pg. 3-38
            commandList.AddRange(encoding.GetBytes("Heavy Metal Financial LLC" + vbCrLf))
            commandList.AddRange(New Byte() {&H1B, &H1D, &H61, &H0}) 'Left Alignment - Refer to Pg. 3-29
            commandList.AddRange(New Byte() {&H1B, &H44, &H2, &H10, &H22, &H0}) 'Setting Horizontal Tab - Pg. 3-27
            commandList.AddRange(encoding.GetBytes("Date: " + Date.Now.ToString("d")))
            commandList.AddRange(New Byte() {&H20, &H9, &H20})                                      'Moving Horizontal Tab - Pg. 3-26
            commandList.AddRange(encoding.GetBytes("Time: " + Date.Now.ToString("T") + vbCrLf))
            commandList.AddRange(encoding.GetBytes("Customer #: " + oCustomer.CustomerID.ToString))
            commandList.AddRange(encoding.GetBytes("------------------------------------------------" + vbCrLf + vbCrLf))
            commandList.AddRange(New Byte() {&H1B, &H45})
            commandList.AddRange(encoding.GetBytes("First Name: " + oCustomer.FirstName + vbCrLf))
            commandList.AddRange(New Byte() {&H1B, &H46})
            commandList.AddRange(encoding.GetBytes("Last Name: " + oCustomer.LastName + vbCrLf))
            Dim seg1 As New clsDataModule.clsLabel
            seg1.strLabel = "CMReg1"
            seg1.iTranslate = 1
            Dim seg2 As New clsDataModule.clsLabel
            seg2.strLabel = "UpgradeCard"
            seg2.iTranslate = 1
            Dim segs(1) As clsDataModule.clsLabel
            segs(0) = seg1
            segs(1) = seg2
            commandList.AddRange(encoding.GetBytes(oCheckCashing.BuildCustomerMessage(segs, oCustomer.oLang.LangID) + " Password: " + oCustomer.Password))
            commandList.AddRange(encoding.GetBytes(" Remember to take your card." + vbCrLf))
            commandList.AddRange(encoding.GetBytes("Thank you." + vbCrLf))
            commandList.AddRange(encoding.GetBytes("------------------------------------------------" + vbCrLf))
            commandList.AddRange(New Byte() {&H1B, &H64, &H2}) 'Cut  - Pg. 3-41

            commandList.Add(&H7)

            command = commandList.ToArray()

            WriteToPrinter(command)
            Return True
        Catch px As Exception

            Return False
        End Try


    End Function


    '    End While
    '    Dim tItem As Type = GetType(Byte)
    '    Dim arrTmp() As Byte = arrPrint.ToArray(tItem)
    '    Return arrTmp
    'End Function
    Public Function PrintReceipt() As Boolean
        Dim command As Byte() = New Byte() {}

        Cursor.Current = Cursors.WaitCursor

        Try
            Dim n1 As String = Chr(&H34) '' option 4 is Code39
            Dim n2int As Integer = 1 ''shifted + 1 to fix index being at zero.
            Dim n2 As String = n2int.ToString("X") ''Under-bar and line feed options, 
            Dim n3 As String = String.Empty

            n3 = Chr(&H39)
            Dim n4 As String = Char.ConvertFromUtf32(CType("80", Integer)) ''The height


            Dim commandList As List(Of Byte) = New List(Of Byte)
            commandList.AddRange(New Byte() {&H1B, &H1D, &H61, &H1}) 'Center Alignment - Refer to Pg. 3-29
            commandList.AddRange(encoding.GetBytes("TRANSACT" + vbCrLf))
            commandList.AddRange(encoding.GetBytes(oCheckCashing.GetKioskSettings("Address", oFlags.WorkstationID) + vbCrLf))
            commandList.AddRange(encoding.GetBytes(oCheckCashing.GetKioskSettings("City", oFlags.WorkstationID) + ", " + oCheckCashing.GetKioskSettings("Zip", oFlags.WorkstationID) + vbCrLf))
            commandList.AddRange(New Byte() {&H1B, &H1C, &H70, &H1, &H0, &HD, &HA}) 'Stored Logo Printing - Refer to Pg. 3-38
            commandList.AddRange(encoding.GetBytes("Heavy Metal Financial LLC" + vbCrLf))
            commandList.AddRange(New Byte() {&H1B, &H1D, &H61, &H0}) 'Left Alignment - Refer to Pg. 3-29
            commandList.AddRange(New Byte() {&H1B, &H44, &H2, &H10, &H22, &H0}) 'Setting Horizontal Tab - Pg. 3-27
            commandList.AddRange(encoding.GetBytes("Date: " + Date.Now.ToString("d")))
            commandList.AddRange(New Byte() {&H20, &H9, &H20})                                      'Moving Horizontal Tab - Pg. 3-26
            commandList.AddRange(encoding.GetBytes("Time: " + Date.Now.ToString("T") + vbCrLf))
            commandList.AddRange(encoding.GetBytes("Customer #: " + oCustomer.CustomerID.ToString + "  Receipt #: " + oCustomer.oCheck.TransactionNumber.ToString))
            commandList.AddRange(New Byte() {&H1B, &H45})
            commandList.AddRange(encoding.GetBytes("------------------------------------------------" + vbCrLf + vbCrLf))
            commandList.AddRange(New Byte() {&H1B, &H45})
            commandList.AddRange(encoding.GetBytes("Previous Card Balance " + oCustomer.oCheck.BeginningBal.ToString("C") + vbCrLf))
            commandList.AddRange(New Byte() {&H1B, &H45})
            commandList.AddRange(encoding.GetBytes("Check Amount: " + oCustomer.oCheck.CheckAmount.ToString("C") + vbCrLf))
            commandList.AddRange(New Byte() {&H1B, &H46})
            commandList.AddRange(encoding.GetBytes("Fee Amount: " + oCustomer.oCheck.Fee.ToString("C") + vbCrLf))
            commandList.AddRange(encoding.GetBytes("Net Amount: " + oCustomer.oCheck.NetAmt.ToString("C") + vbCrLf))
            commandList.AddRange(encoding.GetBytes("PrePaid Card Balance " + oCustomer.oCheck.PrePaidBalance.ToString("C") + vbCrLf))
            commandList.AddRange(encoding.GetBytes("Available Card Balance " + oCustomer.oCheck.PrePaidAvailable.ToString("C") + vbCrLf))

            If oCustomer.oCheck.CashAmt > 0 Then
                commandList.AddRange(encoding.GetBytes("Cash Amount: " + oCustomer.oCheck.CashAmt.ToString("C") + vbCrLf))
            End If
            commandList.AddRange(encoding.GetBytes("Thank your for cashing your check with TRANSACT" + vbCrLf))

            commandList.AddRange(encoding.GetBytes("------------------------------------------------" + vbCrLf))
            Dim errText As String = ""
            If oCustomer.oCheck.CashAmt > 0 Then
                oCheckSvc.GetBarcode(oFlags.WorkstationID, oCustomer.oCheck.BlockID, System.Configuration.ConfigurationManager.AppSettings("ProviderID"), oCustomer.oCheck.CashAmt, BarcodeText, "")
                oCustomer.oCheck.Barcode = BarcodeText
            End If
            If oCustomer.oCheck.Barcode <> "" Then
                commandList.AddRange(encoding.GetBytes(Chr(&H1B) + Chr(&H62) + n1 + n2 + n3 + n4 + oCustomer.oCheck.Barcode + Chr(&H1E)))
            End If

            commandList.AddRange(New Byte() {&H1B, &H64, &H2}) 'Cut  - Pg. 3-41

            commandList.Add(&H7)

            command = commandList.ToArray()

            WriteToPrinter(command)
            If "" <> "" Then
                Return False
            End If
            Return True
        Catch px As Exception

            Return False
        End Try


    End Function


    ''~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~''
    ''     Check Block - ETB
    ''~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~''
    ''' <summary>
    ''' Check Block (ETB) is a function that will communicate with the printer to ask if the job has been printed successfully.
    ''' Use this if you must know if the print job was sent successfully.
    ''' </summary>
    Public Sub PrintChars()
        Dim str As String = Chr(&H1B) + Chr(&H1D) + Chr(&H61) + Chr(&H0) + "     ______________" + vbCrLf
        str += "    /             /|" + vbCrLf
        str += "   /             / |" + vbCrLf
        str += "  /____________ /  |" + vbCrLf
        str += " |  _________  |   |____________________" + vbCrLf
        str += " | |     2011| |   |/        /|,       /|" + vbCrLf
        str += " | |  :)     | |   /        / /S      / |" + vbCrLf
        str += " | |  =D     | |  /_______ / /S      /  |" + vbCrLf
        str += " | |_________| | |  ____ +| /S      /   |" + vbCrLf
        str += " |________++___|/|________|/S      /    |" + vbCrLf
        str += "    ________________     ,S`      /   / |" + vbCrLf
        str += "   /  -/      /-   /|  ,S        /   /| |" + vbCrLf
        str += "  /______________ ''|,S         /   / | |" + vbCrLf
        str += " |       ______  ||,S          /   /  | |" + vbCrLf
        str += " |  -+  |_STAR_| ||/          /   /|  | |" + vbCrLf
        str += " |_______________|/__________/   / |  | |" + vbCrLf
        str += " ''''/----------/|           |  /  |  | |" + vbCrLf
        str += " |o    Star     o|           |  |  |  | |" + vbCrLf
        str += " |o  Micronics  o|______     |  |  |  | |" + vbCrLf
        str += " |o   Printer   o|      |    |  |  |  | /" + vbCrLf
        str += " |o             o|      |    |  |  |__|/" + vbCrLf
        str += " |o  Block Mode o|      |    |  |" + vbCrLf
        str += " |o-------------o|      |    |  |" + vbCrLf
        str += " |o  /^^\\Testing|      |    |  |" + vbCrLf
        str += " |o  / o o|......|      |    |  |" + vbCrLf
        str += " |o/ \\_+_/ .....|      |    |  |" + vbCrLf
        str += " |o|\\     \\ ...|      |    |  |" + vbCrLf
        str += " |o | |+ +-|  ...|      |    |  |" + vbCrLf
        str += " |o------------..|      |    |  |" + vbCrLf
        str += " |o     /|      .|      |    | /Star*" + vbCrLf
        str += " |/|/|./ |_|/|/\_|      |____|/Micronics" + vbCrLf
        str += Chr(&H1B) + Chr(&H64) + Chr(&H2)
        '    MessageBox.Show("Now sending data to printer, press ok and please wait.", "ETB Check Block", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Dim command As Byte() = System.Text.Encoding.GetEncoding(932).GetBytes(str)
        Dim totalSizeCommunicated As UInteger = WriteToPrinter(command)

    End Sub

    ''~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~''
    ''     1D Barcodes
    ''~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~''



End Class
