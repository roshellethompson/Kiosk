Imports System.IO
Imports System.Xml
Imports System.Text
Imports System
Imports System.Configuration

Public Class clsPinPad
    Inherits Ports.SerialPort
    Dim oFlags As New clsFlags
    Dim cItem As String = ""
    Dim strIncoming As String = ""
    Dim oCheckCashing As New clsDataModule.clsInterface
    Public oLog As clsLog
    Dim strAcctNum As String
    Dim strPINBlock As String
    Dim strAmount As String
    Dim fKey As System.IO.FileStream
    ''' <summary>
    ''' clear text message variable
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MsgNum As String = ""
    ''' <summary>
    ''' clear text message variable. 0 = "*" 1 = "Number"
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>  
    Public Property Mask As Integer = 0
    ''' <summary>
    ''' clear text message variable.  Defines max length
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Length As Integer = 0
    ''' <summary>
    ''' clear text message variable. Controls the length of time in seconds the pin pad will wait.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TimeOut As Integer = 0
    ''' <summary>
    ''' Pin message variable
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Amount As String
        Get
            Return strAmount
        End Get
        Set(ByVal value As String)
            strAmount = value
        End Set
    End Property
    ''' <summary>
    ''' Stores the pin  block retrieved from the pin pad.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PINBlock As String
        Get
            Return strPINBlock
        End Get
        Set(ByVal value As String)
            strPINBlock = value
        End Set
    End Property
    ''' <summary>
    ''' The pan
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Account As String
        Get
            Return strAcctNum
        End Get
        Set(ByVal value As String)
            strAcctNum = value
        End Set
    End Property

    ''' <summary>
    ''' Temporary input buffer
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Value As String
        Get
            Return strIncoming
        End Get
        Set(ByVal value As String)
            strIncoming = value
        End Set
    End Property
    ''' <summary>
    ''' The last value in a  byte stream
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CheckSum As String
        Get
            Return cItem
        End Get
        Set(ByVal value As String)
            cItem = value
        End Set
    End Property
    ''' <summary>
    ''' Hex values used in communications
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum ControlCodes
        STX = &H2 'beginning of message
        ETX = &H3 'end of message
        EOT = &H4 'session terminated
        ACK = &H6 'acknowledgment msg received
        SI = &HF  'beginning of message
        SO = &HE  'end of message
        NAK = &H15 'invalid message
        SUBCode = &H1A 'parameter follows
        FS = &H1C 'field separator
    End Enum

    Dim bkey_ID As String
    ''' <summary>
    ''' Which key is tested, 0-F
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property KeyID As String
        Get
            Return bkey_ID
        End Get
        Set(ByVal value As String)
            bkey_ID = value
        End Set
    End Property
    ''' <summary>
    ''' List comm ports available
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property AvailPorts As String()
        Get
            Return System.IO.Ports.SerialPort.GetPortNames
        End Get
    End Property
    Dim bMasterKey As String
    ''' <summary>
    ''' The key to load as 32 character hex string
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MasterKey As String
        Get
            Return bMasterKey
        End Get
        Set(ByVal value As String)
            bMasterKey = value
        End Set
    End Property
    Dim strResult As String = ""
    ''' <summary>
    ''' Value to return
    ''' </summary>
    ''' <value></value>
    ''' <returns>letters, numbers, empty, or asterisk</returns>
    ''' <remarks></remarks>
    Public Property Result As String
        Get
            Return strResult
        End Get
        Set(ByVal value As String)
            If Char.IsNumber(value) Or Char.IsLetter(value) Or value = "" Or value.Contains("*") Then
                strResult = value
            End If
        End Set
    End Property

    Dim iKeyLoaded As Integer = 0
    ''' <summary>
    ''' Msg 04 sets this to true or false
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property KeyLoaded As Boolean
        Get
            Return iKeyLoaded
        End Get
        Set(ByVal value As Boolean)
            iKeyLoaded = value
        End Set
    End Property
    Dim strMessageNum As String
    ''' <summary>
    ''' The clear text message number
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MessageNum As String
        Get
            Return strMessageNum
        End Get
        Set(ByVal value As String)
            strMessageNum = value
        End Set
    End Property
    Dim iRandom As String
    Public Property Random As String
        Get
            Return iRandom
        End Get
        Set(ByVal value As String)
            iRandom = value
        End Set
    End Property

    Public Sub Log(ByVal strMsg As String, ByVal iCustID As Integer)
        Dim cLetter As Char
        Dim i As Integer = 0
        strMsg = Date.Now.ToString + "." + Date.Now.Millisecond.ToString + ": " + strMsg
        Dim bMsg(strMsg.Length) As Byte
        For Each cLetter In strMsg
            bMsg(i) = AscW(cLetter)
            i += 1
        Next
        oLog.LogMsg(strMsg)

    End Sub

    Public Sub CloseLog()
        oLog.Close()
    End Sub

    Public Sub OpenLog()
        oLog = New clsLog(".\Log.txt")

    End Sub
    Public Function SendMsg04() As String
        ' Send strings to a serial port.

        CheckSum = ""
        Dim msgNum(5) As Byte
        msgNum(0) = AscW(Chr(ControlCodes.SI))
        msgNum(1) = AscW(Chr(Asc("0")))
        msgNum(2) = AscW(Chr(Asc("4")))
        msgNum(3) = AscW(Chr(Asc(KeyID)))
        msgNum(4) = ControlCodes.SO
        Return SendData(msgNum)
    End Function

    Public Function SendGetRand() As String
        ' Send strings to a serial port.

        CheckSum = ""
        Dim msgNum(4) As Byte
        msgNum(0) = AscW(Chr(ControlCodes.SI))
        msgNum(1) = AscW(Chr(Asc("1")))
        msgNum(2) = AscW(Chr(Asc("7")))
        msgNum(3) = ControlCodes.SO
        Return SendData(msgNum)
    End Function
    Public Function SendACK() As String
        ' Send strings to a serial port.

        CheckSum = ""
        Dim msgNum(1) As Byte
        msgNum(0) = &H6
        Return SendData(msgNum)
    End Function
    Public Function SendNACK() As String
        ' Send strings to a serial port.

        CheckSum = ""
        Dim msgNum(1) As Byte
        msgNum(0) = AscW(Chr("15"))
        Return SendData(msgNum)
    End Function

    Public Function SendEOT() As String
        ' Send strings to a serial port.

        CheckSum = ""
        Dim msgNum(1) As Byte
        msgNum(0) = AscW(Chr(Asc("4")))
        Return SendData(msgNum)
    End Function
    Public Function SendMsg17() As String
        ' Send strings to a serial port.

        CheckSum = ""
        Dim msgNum(4) As Byte
        Dim bItem As New Byte
        msgNum(0) = AscW(Chr(ControlCodes.SI))
        msgNum(1) = AscW(Chr(Asc("1")))
        msgNum(2) = AscW(Chr(Asc("7")))
        msgNum(3) = AscW(Chr(ControlCodes.SO))
        Return SendData(msgNum)
    End Function
    Public Function SendGetClearText() As String
        ' Send strings to a serial port.
        CheckSum = ""
        Dim msgNum(11) As Byte
        Dim bItem As New Byte
        Result = ""
        Dim i As Integer = 0
        MessageNum = "Z50"
        msgNum(i) = AscW(Chr(ControlCodes.STX))
        i += 1
        For Each b In MessageNum
            msgNum(i) = AscW(Chr(Asc(b)))
            i += 1
        Next
        Mask = 1
        msgNum(i) = AscW(Chr(Asc(Mask.ToString)))
        i += 1
        TimeOut = 60
        For Each b In TimeOut.ToString.PadLeft(3, "0")
            msgNum(i) = AscW(Chr(Asc(b)))
            i += 1
        Next
        For Each b In Length.ToString.PadLeft(2, "0")
            msgNum(i) = AscW(Chr(Asc(b)))
            i += 1
        Next
        msgNum(i) = AscW(Chr(ControlCodes.ETX))
        If Not oFlags.ActiveTimer = "CASH" And oFlags.SimPinPad Then
            Return 0
        End If
        Return SendData(msgNum)
    End Function

    Public iCustID As Integer = 0
    Public Function SendGetPIN() As String
        ' Send strings to a serial port.
        CheckSum = ""
        Amount = "0000"
        Log(MasterKey + Account + Amount, iCustID)
        Dim msgNum(4 + MasterKey.Length + Account.Length + Amount.Length + 2) As Byte
        Dim bItem As New Byte
        msgNum(0) = AscW(Chr(ControlCodes.STX))
        msgNum(1) = AscW(Chr(Asc("7")))
        msgNum(2) = AscW(Chr(Asc("0")))
        msgNum(3) = &H2E
        Result = ""
        Dim index As Integer = 4
        For Each c In Account
            msgNum(index) = AscW(Chr(Asc(c)))
            index += 1
        Next
        msgNum(index) = &H1C
        index += 1
        For Each c In MasterKey
            msgNum(index) = AscW(Chr(Asc(c)))
            index += 1
        Next

        For Each c In Amount
            msgNum(index) = AscW(Chr(Asc(c)))
            index += 1
        Next
        msgNum(index) = AscW(Chr(ControlCodes.ETX))
        If oFlags.SimPinPad Then
            Return 0
        End If
        Return SendData(msgNum)
    End Function
    Public Function SendColdReset() As String
        ' Send strings to a serial port.
        CheckSum = ""
        Dim msgNum(11) As Byte
        Dim bItem As New Byte
        msgNum(0) = AscW(Chr(ControlCodes.STX))
        msgNum(1) = AscW(Chr(Asc("I")))
        msgNum(2) = AscW(Chr(Asc("0")))
        msgNum(3) = AscW(Chr(Asc("1")))
        msgNum(4) = AscW(Chr(ControlCodes.ETX))
        Return SendData(msgNum)
    End Function

    Public Function SendLoadAKey() As String
        ' Send strings to a serial port.
        Dim strLog As String = ""
        CheckSum = ""
        Dim msgNum(37) As Byte
        Dim bItem As New Byte
        msgNum(0) = AscW(Chr(ControlCodes.SI))
        msgNum(1) = AscW(Chr(Asc("0")))
        msgNum(2) = AscW(Chr(Asc("2")))
        msgNum(3) = AscW(Chr(Asc(KeyID)))
        Dim cItem As New Char
        Dim iIndex As Integer = 4
        For Each cItem In MasterKey
            msgNum(iIndex) = AscW(Chr(Asc(cItem)))
            iIndex += 1
        Next
        msgNum(iIndex) = AscW(Chr(ControlCodes.SO))
        Return SendData(msgNum)
    End Function

    Public Function SendMsg08() As String
        ' Send strings to a serial port.
        Dim strLog As String = ""
        CheckSum = ""
        Dim msgNum(37) As Byte
        Dim bItem As New Byte
        msgNum(0) = AscW(Chr(ControlCodes.SI))
        Dim cItem As New Char
        Dim iIndex As Integer = 1
        For Each cItem In MasterKey
            msgNum(iIndex) = AscW(Chr(Asc(cItem)))
            iIndex += 1
        Next
        msgNum(iIndex) = AscW(Chr(ControlCodes.SO))
        Array.Resize(msgNum, msgNum.Length - 1)
        Return SendData(msgNum)
    End Function

    Public Function SendLoadDUKPT() As String
        ' Send strings to a serial port.
        Dim strLog As String = ""
        CheckSum = ""
        Dim msgNum(41) As Byte
        Dim bItem As New Byte
        msgNum(0) = AscW(Chr(ControlCodes.STX))
        msgNum(1) = AscW(Chr(Asc("9")))
        msgNum(2) = AscW(Chr(Asc("0")))
        Dim cItem As New Char
        Dim iIndex As Integer = 3
        For Each cItem In MasterKey.Substring(0, 16)
            msgNum(iIndex) = AscW(Chr(Asc(cItem)))
            iIndex += 1
        Next
        Dim iCountSN As Integer = iIndex + 20
        Dim strSN As String = "FFFF9876543210E00000"
        For Each c In strSN
            msgNum(iIndex) = AscW(Chr(Asc(c)))
            iIndex += 1
        Next
        Array.Resize(msgNum, msgNum.Length - 1)
        msgNum(iIndex) = AscW(Chr(ControlCodes.ETX))
        Return SendData(msgNum)
    End Function

    Public Function RequestPasswords(ByVal iNumPass As Integer) As String
        ' Send strings to a serial port.
        Dim strLog As String = ""
        CheckSum = ""
        Dim msgNum(8) As Byte
        Dim bItem As New Byte
        msgNum(0) = AscW(Chr(ControlCodes.STX))
        msgNum(1) = AscW(Chr(Asc("3")))
        msgNum(2) = AscW(Chr(Asc("0")))
        msgNum(3) = AscW(Chr(Asc(iNumPass.ToString)))
        msgNum(4) = AscW(Chr(Asc("0")))
        msgNum(5) = AscW(Chr(Asc("6")))
        msgNum(6) = AscW(Chr(Asc("0")))
        msgNum(7) = AscW(Chr(ControlCodes.ETX))
        Return SendData(msgNum)
    End Function

    Public Function UpdatePasswords(ByVal iNumPass As Integer) As String
        ' Send strings to a serial port.
        Dim strLog As String = ""
        CheckSum = ""
        Dim msgNum(9) As Byte
        Dim bItem As New Byte
        msgNum(0) = AscW(Chr(ControlCodes.STX))
        msgNum(1) = AscW(Chr(Asc("3")))
        msgNum(2) = AscW(Chr(Asc("2")))
        msgNum(3) = AscW(Chr(Asc(iNumPass.ToString)))
        msgNum(4) = AscW(Chr(Asc("0")))
        msgNum(5) = AscW(Chr(Asc("6")))
        msgNum(6) = AscW(Chr(Asc("0")))
        msgNum(7) = AscW(Chr(ControlCodes.ETX))
        Return SendData(msgNum)
    End Function
    Public Function GetKeyFromFile() As Boolean
        If System.IO.File.Exists(".\KEY.txt") Then
            fKey = New System.IO.FileStream(".\KEY.txt", System.IO.FileMode.Open)
            Dim fKeyAttr As New System.IO.FileInfo(".\KEY.txt")
            Dim dFile As Date = fKeyAttr.LastWriteTime
            If ((Date.Now - dFile.Date) > New TimeSpan(2, 0, 0, 0)) Or fKey.Length = 0 Then
                If fKey.Length <> 0 Then
                    Dim arrKey(fKey.Length) As Byte
                    fKey.Read(arrKey, 0, arrKey.Length)
                    For Each c In arrKey
                        If c <> 0 Then
                            MasterKey += Chr(c)
                        End If
                    Next
                Else
                    Return False
                End If
                fKey.Close()
                System.IO.File.Delete(".\KEY.txt")
                fKey = New System.IO.FileStream(".\KEY.txt", System.IO.FileMode.Create)
                MasterKey = MasterKey.Trim()
                fKey.Close()
                Return False
            Else
                Dim arrKey(fKey.Length) As Byte
                fKey.Read(arrKey, 0, arrKey.Length)
                If (arrKey.Length = 32) Then
                    For Each c In arrKey
                        If c <> 0 Then
                            MasterKey += Chr(c)
                        End If
                    Next
                    MasterKey = MasterKey.Trim()
                    fKey.Close()
                    Return True
                Else
                    Return False
                End If
            End If
        Else

            fKey = New System.IO.FileStream(".\KEY.txt", System.IO.FileMode.Create)

            fKey.Close()
            Return False
        End If
    End Function

    Public Function GetCheckSum(ByVal arrBytes As Byte()) As Byte
        Dim bCheck As New Byte
        Dim index As Integer = 0
        Dim bItem As Byte
        For Each bItem In arrBytes
            If index > 0 Then
                bCheck = bCheck Xor bItem
            End If
            index += 1
        Next
        Return bCheck
    End Function
    Public Function ByteArrayToString(ByVal ba As Byte()) As String
        Dim hex As New StringBuilder(ba.Length * 2)
        Dim b As Byte
        For Each b In ba
            hex.AppendFormat("{0:x2}", b)
        Next
        Return hex.ToString()
    End Function
    Public Function StringToByteArray(ByVal hex As String) As Byte()
        Dim NumberChars As Integer = hex.Length
        Dim bytes(NumberChars / 2) As Byte
        Dim i As Integer = 0
        While (i < NumberChars)
            bytes(i / 2) = Convert.ToByte(hex.Substring(i, 2), 16)
            i += 2
        End While
        Return bytes
    End Function
    Public Function SendData(ByVal arrBytes As Byte()) As String

        Dim bCheck As Byte = GetCheckSum(arrBytes)

        arrBytes(arrBytes.Length - 1) = bCheck
        CheckSum = bCheck.ToString
        Dim strSent As String = ByteArrayToString(arrBytes)
        Dim bSent(StringToByteArray(strSent).Length) As Byte
        bSent = StringToByteArray(strSent)
        Array.Resize(bSent, bSent.Length - 1)
        Write(bSent, 0, bSent.Length)
        Dim strLog As String = ""
        For Each bItem In bSent
            strLog += "<" + AscW(Chr(bItem)).ToString + ">"
        Next
        Log("PC->DTE " + strLog + vbCrLf, iCustID)

        Return ByteArrayToString(arrBytes)
    End Function

    Public Sub OpenPort()
        Close()
        BaudRate = 9600
        StopBits = Ports.StopBits.One
        Parity = Ports.Parity.None
        DataBits = 8
        Handshake = Ports.Handshake.None
        PortName = oCheckCashing.GetKioskSettings("KeyPadPortName", oFlags.WorkstationID)
        Me.DtrEnable = True
        If Not IsOpen() Then Open()

    End Sub

    Public Sub ClosePort()
        Close()
    End Sub
    Public Sub VerifyMasterKey(ByVal buffer As Byte())
        Dim index As Integer = 0
        For Each iChar In buffer
            If index = 4 Then
                If Chr(iChar) = "F" Then
                    KeyLoaded = False
                Else
                    KeyLoaded = True
                End If
            End If
            index += 1
        Next
    End Sub

    Public Function ExtractRand(ByVal buffer As Byte()) As String
        Dim i As Integer = 0
        Dim strRandom As String = ""
        For Each iByte In buffer
            If i > 2 And i < 19 Then
                strRandom += Chr(iByte)
            End If
            i += 1
        Next
        Return strRandom
    End Function
    Public Sub SetIncomingText(ByVal buffer As Byte())
        Dim iChar As Byte
        Dim index As Integer = 0
        index = 0
        index = 0
        Value = ""
        For Each iChar In buffer
            If index > 3 Then
                If iChar = AscW(Chr(3)) Then
                    Exit For
                Else
                    Result += Chr(iChar)
                End If
            End If
            index += 1
        Next
    End Sub
    Public Sub ReceivePasswords(ByVal buffer As Byte())
        Dim iChar As Byte
        Dim index As Integer = 0
        For Each iChar In buffer
            If index > 2 Then
                Result += Chr(iChar)
            End If
            index += 1
        Next
    End Sub
    Public Sub ExtractPinBlock(ByVal buffer As Byte())
        Dim iChar As Byte
        Dim index As Integer = 0
        Dim bGetPIN As Boolean = False
        Dim strBegin As String = ""

        PINBlock = ""
        For Each iChar In buffer
            If index > 3 Then
                If index > 6 Then
                    strBegin += Chr(iChar)
                End If
                If strBegin = "01" Then
                    bGetPIN = True
                    index += 1
                    Continue For
                End If
                If iChar = ControlCodes.ETX Then
                    Exit Sub
                End If
                If bGetPIN = False Then
                    Result += Chr(iChar)
                Else
                    PINBlock += Chr(iChar)
                End If
            End If
            index += 1
            If PINBlock <> "" Then
                Result = ""
            End If
        Next
    End Sub
    Public Sub Reset()
        Me.Result = ""
        Me.PINBlock = ""
        oFlags.Enter = False
        oFlags.CurrentInputLength = 0
        oFlags.Activity = False
        oFlags.OnSecondPin = False
        oFlags.Clear = False
        oFlags.Cancel = False
        oFlags.TimeOut = False
        oFlags.Ack = False
    End Sub
    Dim bVerifyEnter As Boolean = False
    Dim strText As String = ""
    Private Sub comPort_DataReceived(ByVal sender As Object, ByVal e As System.IO.Ports.SerialDataReceivedEventArgs) Handles Me.DataReceived
        CheckSum = ""
        Dim sp As Ports.SerialPort = CType(sender, Ports.SerialPort)
        Dim buffer(sp.BytesToRead) As Byte
        sp.Read(buffer, 0, sp.BytesToRead)
        Dim iChar As Byte
        Dim index As Integer = 0
        Dim bNext As Boolean = False
        Dim strMsgNum As String = ""
        Dim bMsgNum(2) As Byte
        Dim strClear As String = ""
        Value = ""
        If oFlags.Enter = False Then
            oFlags.SetDataReceivedFlags()
        End If
        Dim strLog As String = ""
        For Each bItem In buffer
            strLog += "<" + AscW(Chr(bItem)).ToString + ">"
        Next
        While oLog.fLog Is Nothing
            System.Threading.Thread.Sleep(100)
        End While
        Log("DTE->PC " + strLog + vbCrLf, iCustID)
        For Each iChar In buffer
            If iChar = 6 Then
                Continue For
            End If
            If index > 2 Then

                Value += Chr(iChar)
                If Value.Contains(&H4) Then
                    oFlags.EOT = True
                End If

            End If
            If iChar = AscW(Chr(6)) Then
                index += 1
                oFlags.Ack = True
                Continue For
            End If

            If index > 0 And index < 4 Then
                If index = 1 And Chr(iChar) <> "Z" Then
                    strMsgNum = Chr(iChar)
                ElseIf index = 2 Then
                    strMsgNum += Chr(iChar)
                ElseIf strMsgNum.Length = 1 Then
                    strMsgNum += Chr(iChar)
                End If
            End If
            If index > 3 And index < 10 Then

                strClear += Chr(iChar)
            End If
            If index = 1 Or index = 2 Then
                strClear += Chr(iChar)

            End If
            index += 1
            If index = buffer.Length Then
                Exit For
            End If
        Next

        If strClear.Contains("0008") Then
            oFlags.Clear = True
            Result = ""
            Exit Sub
        ElseIf strClear.Contains("001B") Then
            oFlags.Cancel = True
            Exit Sub
        ElseIf strClear.Contains("?") Then
            oFlags.TimeOut = True
            Exit Sub
        Else
            oFlags.Clear = False
            oFlags.Cancel = False
            oFlags.TimeOut = False
        End If



        Array.Resize(bMsgNum, bMsgNum.Length - 1)
        MessageNum = strMsgNum
        If MessageNum = "17" Then
            Random = ExtractRand(buffer)
        ElseIf MessageNum = "04" Then
            VerifyMasterKey(buffer)
        ElseIf MessageNum = "51" Then
            Result = ""
            SetIncomingText(buffer)

        ElseIf MessageNum = "31" Then
            Result = ""
            ReceivePasswords(buffer)
        ElseIf MessageNum = "71" Then
            PINBlock = ""
            Result = ""
            ExtractPinBlock(buffer)

        End If

    End Sub
    Public Sub WriteMasterKey()
        If MasterKey = "" Then
            Exit Sub
        End If
        fKey.Close()
        fKey = New System.IO.FileStream(".\KEY.txt", System.IO.FileMode.Create)
        Dim arrKey(MasterKey.Length) As Byte
        Dim i As Integer = 0
        For Each c In MasterKey
            arrKey(i) = AscW(c)
            i = i + 1
        Next
        fKey.Write(arrKey, 0, arrKey.Length)
        fKey.Close()

    End Sub
    Public Sub New()

        oLog = New clsLog(".\Log.txt")

        OpenPort()
        Amount = "0000"
    End Sub
End Class
