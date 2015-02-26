Imports System

Public Class FpReaderCommand

    Public COMMAND_SHOWFORM_CMD = 1
    Public COMMAND_ACK = 2
    Public COMMAND_OPEN_PASSTHROUGH = 3
    Public COMMAND_AMC_RESET = 4
    Public COMMAND_AMC_ARM2READ = 5
    Public COMMAND_AMC_SWIPE = 6
    Public COMMAND_AMC_READTK1 = 7
    Public COMMAND_AMC_READTK2 = 8
    Public COMMAND_AMC_READTK3 = 9
    Public COMMAND_SHOWFORM_DATATK3 = 10
    Public COMMAND_SHOWFORM_PLACEFINGER = 11
    Public COMMAND_SHOWFORM_STATUS = 12
    Public COMMAND_SHOW_STATUS_MESSAGE = 13
    Public COMMAND_SHOWFORM_LIFTFINGER = 14
    Public COMMAND_SHOWFORM_ENTERPIN = 15
    Public COMMAND_SHOWFORM_WAIT = 16
    Public COMMAND_SETCOERCIVITY = 17
    Public COMMAND_HYPERCOMSWIPE = 18
    Public COMMAND_AMC_WRITETK3 = 19
    Public COMMAND_SHOWFORM_ENROLLOK = 20
    Public COMMAND_AMC_UNKNOWN1 = 21
    Public COMMAND_AMC_UNKNOWN2 = 22
    Public COMMAND_LOAD_TEXT = 23
    Public COMMAND_SHOWFORM_TEXT = 24
    Public COMMAND_SHOWFORM_VERIFYFAIL = 25
    Public COMMAND_SHOWFORM_VERIFYOK = 26
    Public COMMAND_SHOWFORM_ENROLLFAIL = 27

    Public Shared ERROR_INVALID_PORT = -101        ' Com port not valid
    Public Shared ERROR_PORT_ALREADY_IN_USE = -102 ' Com port in use by other app
    Public Shared ERROR_FP_READER_NOT_FOUND = -103 ' FpReader not found on Com port
    Public Shared ERROR_INIT_PROBLEM = -104        ' Unexpected initialization error

    Public commandData(1000) As Byte
    Public command As Integer
    Public commandLength As Integer
    Public track3 As String
    Public statusText As String
    Public textLine1 As String
    Public textLine2 As String
    Public textLine3 As String
    Public textLine4 As String

    Public Sub New(ByRef commandBuffer As Byte(), ByVal offset As Integer, ByVal numBytes As Integer)
        Console.WriteLine("")
        Console.WriteLine("------ PROCESSING COMMAND------------")
        For j = offset To (offset + numBytes - 1)
            commandData(j - offset) = commandBuffer(j)
            Console.Write(IIf(commandBuffer(j) < &H20, " ", ChrW(commandBuffer(j))))
        Next j
        Console.WriteLine("> " & (offset + numBytes))

        commandLength = numBytes

        'Identify the command
        command = identifyCommand()
    End Sub

    Public Function identifyCommand() As Integer
        Console.WriteLine("identifying command")
        If (isCommandMatch(CMD_DATA_ACK)) Then
            Console.WriteLine("ACK")
            Return COMMAND_ACK
        End If
        If (isCommandMatch(CMD_DATA_OPEN_PASSTHROUGH)) Then
            Console.WriteLine("it's the open-pass-through command")
            Return COMMAND_OPEN_PASSTHROUGH
        ElseIf (isCommandMatch(CMD_DATA_SHOW_FORM_COMMAND)) Then
            Console.WriteLine("SHOW COMMAND FORM")
            Return COMMAND_SHOWFORM_CMD
        ElseIf (isCommandMatch(CMD_DATA_AMC_RESET)) Then
            Console.WriteLine("reset mag strip")
            Return COMMAND_AMC_RESET
        ElseIf (isCommandMatch(CMD_DATA_AMC_ARM2READ)) Then
            Console.WriteLine("AMC Arm to Read")
            Return COMMAND_AMC_ARM2READ
        ElseIf (isCommandMatch(CMD_DATA_SHOW_FORM_SWIPE)) Then
            Console.WriteLine("AMC Ready for Swipe")
            Return COMMAND_AMC_SWIPE
        ElseIf (isCommandMatch(CMD_DATA_AMC_READTK1)) Then
            Console.WriteLine("AMC Read Track 1")
            Return COMMAND_AMC_READTK1
        ElseIf (isCommandMatch(CMD_DATA_AMC_READTK2)) Then
            Console.WriteLine("AMC Read Track 2")
            Return COMMAND_AMC_READTK2
        ElseIf (isCommandMatch(CMD_DATA_AMC_READTK3)) Then
            Console.WriteLine("AMC Read Track 3")
            Return COMMAND_AMC_READTK3
        ElseIf (isCommandMatch(CMD_DATA_SHOW_FORM_DATATK3)) Then
            Console.WriteLine(" show data track 3 form")
            Return COMMAND_SHOWFORM_DATATK3
        ElseIf (isCommandMatch(CMD_DATA_SHOW_FORM_PLACEFINGER)) Then
            Console.WriteLine(" show placefinger form")
            Return COMMAND_SHOWFORM_PLACEFINGER
        ElseIf (isCommandMatch(CMD_DATA_SHOW_FORM_STATUS)) Then
            Console.WriteLine(" show status form")
            Return COMMAND_SHOWFORM_STATUS
        ElseIf (isCommandMatch(CMD_DATA_SHOW_FORM_LIFTFINGER)) Then
            Console.WriteLine(" show lift finger form")
            Return COMMAND_SHOWFORM_LIFTFINGER
        ElseIf (isCommandMatch(CMD_DATA_SHOW_FORM_ENTERPIN)) Then
            Console.WriteLine(" show enter pin form")
            Return COMMAND_SHOWFORM_ENTERPIN
        ElseIf (isCommandMatch(CMD_DATA_SHOW_FORM_WAIT)) Then
            Console.WriteLine(" show wait form")
            Return COMMAND_SHOWFORM_WAIT
        ElseIf (isCommandMatch(CMD_DATA_SETCOERCIVITY)) Then
            Console.WriteLine(" set coercivity")
            Return COMMAND_SETCOERCIVITY
        ElseIf (isCommandMatch(CMD_DATA_HYPERCOMSWIPE)) Then
            Console.WriteLine(" hypercom card swipe")
            Return COMMAND_HYPERCOMSWIPE
        ElseIf (isCommandMatch(CMD_DATA_SHOW_FORM_ENROLLOK)) Then
            Console.WriteLine(" show enroll OK form")
            Return COMMAND_SHOWFORM_ENROLLOK
        ElseIf (isCommandMatch(CMD_DATA_AMC_UNKNOWN1)) Then
            Console.WriteLine(" unknown AMC command1")
            Return COMMAND_AMC_UNKNOWN1
        ElseIf (isCommandMatch(CMD_DATA_AMC_UNKNOWN2)) Then
            Console.WriteLine(" unknown AMC command2")
            Return COMMAND_AMC_UNKNOWN2
        ElseIf (isCommandMatch(CMD_DATA_SHOW_FORM_SHOWTEXT)) Then
            Console.WriteLine(" show text")
            Return COMMAND_SHOWFORM_TEXT
        ElseIf (isCommandMatch(CMD_DATA_SHOW_FORM_VERIFYFAIL)) Then
            Console.WriteLine("verify failed")
            Return COMMAND_SHOWFORM_VERIFYFAIL
        ElseIf (isCommandMatch(CMD_DATA_SHOW_FORM_VERIFYOK)) Then
            Console.WriteLine("verify OK")
            Return COMMAND_SHOWFORM_VERIFYOK
        ElseIf (isCommandMatch(CMD_DATA_SHOW_FORM_ENROLLFAIL)) Then
            Console.WriteLine("enroll Failed")
            Return COMMAND_SHOWFORM_ENROLLFAIL
        ElseIf (isPrefixMatch(CMD_PREFIX_SHOW_MESSAGE)) Then
            Console.WriteLine("show status message")
            statusText = ""
            If (commandLength > 27) Then
                For j = 27 To (commandLength - 1)
                    If (commandData(j) = &H3) Then
                        Exit For
                    End If
                    statusText = statusText & ChrW(commandData(j))
                Next
            End If
            Return COMMAND_SHOW_STATUS_MESSAGE
        ElseIf (isPrefixMatch(CMD_PREFIX_LOAD_TEXT)) Then
            Console.WriteLine("load text")
            textLine1 = ""
            textLine2 = ""
            textLine3 = ""
            If (commandLength > 94) Then
                For j = 6 To 45
                    textLine1 = textLine1 & ChrW(commandData(j))
                    Console.WriteLine("line 1: " & j & " = " & Byte2Char(commandData(j)) & " (0x" & HexByte2Char(commandData(j)) & ")")
                Next
                For j = 50 To 57
                    textLine2 = textLine2 & ChrW(commandData(j))
                    Console.WriteLine("line 2: " & j & " = " & Byte2Char(commandData(j)) & " (0x" & HexByte2Char(commandData(j)) & ")")
                Next
                For j = 62 To 79
                    textLine3 = textLine3 & ChrW(commandData(j))
                    Console.WriteLine("line 3: " & j & " = " & Byte2Char(commandData(j)) & " (0x" & HexByte2Char(commandData(j)) & ")")
                Next
                For j = 87 To 93
                    textLine4 = textLine4 & ChrW(commandData(j))
                    Console.WriteLine("line 4: " & j & " = " & Byte2Char(commandData(j)) & " (0x" & HexByte2Char(commandData(j)) & ")")
                Next
            End If
            Return COMMAND_LOAD_TEXT
        ElseIf (isPrefixMatch(CMD_PREFIX_AMC_WRITETK3) And _
                    (commandData(17) = &H63) And (commandData(18) = &H31)) Then
            Console.WriteLine("write tk3")
            Console.WriteLine("num characters: " & BitConverter.ToString(commandData, 13, 3))
            Console.WriteLine("num characters: " & ChrW(commandData(13)) & ChrW(commandData(14)) & ChrW(commandData(15)))
            Dim track3length = Val(byteArray2String(commandData, 13, 3)) - 3
            Console.WriteLine("as int = " & track3length)
            track3 = byteArray2String(commandData, 19, track3length)
            Console.WriteLine("track 3 = " & track3)
            Return COMMAND_AMC_WRITETK3
        Else
            Console.WriteLine("UNIDENTIFIED COMMAND-------")
            For j = 0 To (commandLength - 1)
                Console.WriteLine("j: " & j & " = " & Byte2Char(commandData(j)) & " (0x" & HexByte2Char(commandData(j)) & ")")
            Next
        End If

    End Function

    Public Function isCommandMatch(ByRef compareArray As Byte()) As Boolean
        'Console.WriteLine("commandlength = " & commandLength & "   comparelength: " & compareArray.Length)
        If ((commandLength = compareArray.Length) And _
            byteArrayCompare(commandData, compareArray, commandLength)) Then
            Return True
        End If
        Return False
    End Function

    Public Function isPrefixMatch(ByRef compareArray As Byte()) As Boolean
        'Console.WriteLine("commandlength = " & commandLength & "   comparelength: " & compareArray.Length)
        If (byteArrayCompare(commandData, compareArray, compareArray.Length)) Then
            Return True
        End If
        Return False
    End Function

    Public Function byteArrayCompare(ByRef array1 As Byte(), ByRef array2 As Byte(), _
                        ByVal comparelength As Integer) As Boolean
        Return byteArrayCompare(array1, 0, array2, 0, comparelength)
    End Function

    Public Function byteArrayCompare(ByRef array1 As Byte(), ByRef array2 As Byte(), _
                        ByVal offset As Integer, ByVal compareLength As Integer) As Boolean
        Return byteArrayCompare(array1, offset, array2, offset, compareLength)
    End Function

    Public Function byteArrayCompare(ByRef array1 As Byte(), ByVal array1offset As Integer, _
                        ByRef array2 As Byte(), ByVal array2offset As Integer, _
                        ByVal compareLength As Integer) As Boolean
        If ((array1.Length < array1offset + compareLength) Or _
                (array2.Length < array2offset + compareLength)) Then
            Return False
        End If

        Dim k = array2offset
        For j = array1offset To (array1offset + compareLength - 1)
            If (array1(j) <> array2(k)) Then
                Return False
            End If
            k += 1
        Next

        Return True
    End Function

    Public Function byteArray2String(ByRef array As Byte(), ByVal offset As Integer, ByVal length As Integer) As String
        Dim tempString = ""
        For j = offset To (offset + length - 1)
            tempString = tempString & ChrW(array(j))
        Next
        Return tempString
    End Function

    Shared Function HexByte2Char(ByVal Value As Byte) As String
        ' Return a byte value as a two-digit hex string. 
        HexByte2Char = IIf(Value < &H10, "0", "") & Hex$(Value)
    End Function

    Shared Function Byte2Char(ByVal value As Byte) As String
        Byte2Char = IIf(value < &H20, "(0x" & HexByte2Char(value) & ")", ChrW(value))
    End Function

    Public CMD_DATA_ACK() As Byte = New Byte() {&H6}

    Public CMD_DATA_SHOW_FORM_COMMAND() As Byte = New Byte() { _
                                          &H2, &H56, &H1C, &H46, &H4E, &H43, &H4F, &H4D _
                                        , &H4D, &H41, &H4E, &H44, &H3}

    Public CMD_DATA_OPEN_PASSTHROUGH() As Byte = New Byte() { _
                                          &H2, &H42, &H1C, &H50, &H4E, &H32, &H1C, &H41, &H43 _
                                        , &H4F, &H1C, &H42, &H52, &H35, &H1C, &H43, &H50, &H32 _
                                        , &H3}
    Public CMD_DATA_AMC_RESET() As Byte = New Byte() { _
                                          &H2, &H42, &H1C, &H50, &H4E, &H32, &H1C, &H41, &H43 _
                                        , &H57, &H1C, &H44, &H54, &H30, &H30, &H32, &H1C, &H7F _
                                        , &H0, &H3}

    Public CMD_DATA_AMC_ARM2READ() As Byte = New Byte() { _
                                      &H2, &H42, &H1C, &H50, &H4E, &H32, &H1C, &H41, &H43 _
                                    , &H57, &H1C, &H44, &H54, &H30, &H30, &H31, &H1C, &H50, &H3}

    Public CMD_DATA_SHOW_FORM_SWIPE() As Byte = New Byte() { _
                                    &H2, &H56, &H1C, &H46, &H4E, &H53, &H57, &H49, &H50 _
                                    , &H45, &H41, &H4D, &H43, &H3}

    Public CMD_DATA_AMC_READTK1() As Byte = New Byte() { _
                                      &H2, &H42, &H1C, &H50, &H4E, &H32, &H1C, &H41, &H43 _
                                    , &H57, &H1C, &H44, &H54, &H30, &H30, &H31, &H1C, &H51, &H3}

    Public CMD_DATA_AMC_READTK2() As Byte = New Byte() { _
                                      &H2, &H42, &H1C, &H50, &H4E, &H32, &H1C, &H41, &H43 _
                                    , &H57, &H1C, &H44, &H54, &H30, &H30, &H31, &H1C, &H52, &H3}

    Public CMD_DATA_AMC_READTK3() As Byte = New Byte() { _
                                    &H2, &H42, &H1C, &H50, &H4E, &H32, &H1C, &H41, &H43 _
                                    , &H57, &H1C, &H44, &H54, &H30, &H30, &H32, &H1C, &H73, &H31 _
                                    , &H3}

    Public CMD_DATA_SHOW_FORM_DATATK3() As Byte = New Byte() { _
                                    &H2, &H56, &H1C, &H46, &H4E, &H44, &H41, &H54, &H41 _
                                    , &H54, &H52, &H33, &H3}

    Public CMD_DATA_SHOW_FORM_PLACEFINGER() As Byte = New Byte() { _
                                    &H2, &H56, &H1C, &H46, &H4E, &H50, &H4C, &H41, &H43 _
                                    , &H45, &H46, &H3}

    Public CMD_DATA_SHOW_FORM_STATUS() As Byte = New Byte() { _
                                    &H2, &H56, &H1C, &H46, &H4E, &H53, &H54, &H41, &H54 _
                                    , &H55, &H53, &H3}

    Public CMD_DATA_SHOW_FORM_LIFTFINGER() As Byte = New Byte() { _
                                    &H2, &H56, &H1C, &H46, &H4E, &H4C, &H49, &H46, &H54 _
                                    , &H46, &H49, &H4E, &H47, &H45, &H52, &H3}

    Public CMD_DATA_SHOW_FORM_ENTERPIN() As Byte = New Byte() { _
                                    &H2, &H56, &H1C, &H46, &H4E, &H50, &H49, &H4E, &H46 _
                                    , &H52, &H4D, &H3}

    Public CMD_DATA_SHOW_FORM_WAIT() As Byte = New Byte() { _
                                    &H2, &H56, &H1C, &H46, &H4E, &H57, &H41, &H49 _
                                    , &H54, &H3}

    Public CMD_DATA_SETCOERCIVITY() As Byte = New Byte() { _
                                    &H2, &H42, &H1C, &H50, &H4E, &H32, &H1C, &H41 _
                                    , &H43, &H57, &H1C, &H44, &H54, &H30, &H30, &H34 _
                                    , &H1C, &H3C, &H32, &H35, &H35, &H3}

    Public CMD_DATA_HYPERCOMSWIPE() As Byte = New Byte() { _
                                        &H2, &H56, &H1C, &H46, &H4E, &H53, &H57, &H49, &H50 _
                                        , &H45, &H46, &H52, &H4D, &H1C, &H54, &H4B, &H34, &H1C _
                                        , &H48, &H50, &H1C, &H50, &H31, &H73, &H77, &H69, &H70 _
                                        , &H65, &H20, &H63, &H61, &H72, &H64, &H3}

    Public CMD_DATA_SHOW_FORM_ENROLLOK() As Byte = New Byte() { _
                                        &H2, &H56, &H1C, &H46, &H4E, &H45, &H4E, &H52, &H4F _
                                        , &H4C, &H4C, &H4F, &H4B, &H3}

    Public CMD_DATA_SHOW_FORM_ENROLLFAIL() As Byte = New Byte() { _
                                        &H2, &H56, &H1C, &H46, &H4E, &H4E, &H4F, &H45, &H4E _
                                        , &H52, &H4F, &H4C, &H4C, &H3}

    Public CMD_DATA_SHOW_FORM_VERIFYOK() As Byte = New Byte() { _
                                        &H2, &H56, &H1C, &H46, &H4E, &H56, &H45, &H52, &H49 _
                                        , &H46, &H59, &H4F, &H4B, &H3}


    Public CMD_DATA_SHOW_FORM_VERIFYFAIL() As Byte = New Byte() { _
                                        &H2, &H56, &H1C, &H46, &H4E, &H4E, &H4F, &H56, &H45 _
                                        , &H52, &H49, &H46, &H59, &H3}

    Public CMD_DATA_SHOW_FORM_SHOWTEXT() As Byte = New Byte() { _
                                        &H2, &H56, &H1C, &H46, &H4E, &H53, &H48, &H4F, &H57 _
                                        , &H54, &H45, &H58, &H54, &H3}

    Public CMD_DATA_AMC_UNKNOWN1() As Byte = New Byte() { _
                                      &H2, &H42, &H1C, &H50, &H4E, &H32, &H1C, &H41, &H43 _
                                    , &H57, &H1C, &H44, &H54, &H30, &H30, &H33, &H1C, &H3B _
                                    , &H33, &H48, &H3}

    Public CMD_DATA_AMC_UNKNOWN2() As Byte = New Byte() { _
                                  &H2, &H42, &H1C, &H50, &H4E, &H32, &H1C, &H41, &H43 _
                                , &H57, &H1C, &H44, &H54, &H30, &H30, &H31, &H1C, &H40 _
                                , &H3}


    Public CMD_PREFIX_SHOW_MESSAGE() As Byte = New Byte() { _
                                        &H2, &H56, &H1C, &H46, &H4E, &H53, &H54, &H41, &H54 _
                                        , &H55, &H53, &H1C, &H44, &H4C, &H30, &H32, &H30, &H35 _
                                        , &H44, &H30, &H30, &H30, &H30, &H30, &H46, &H46, &H32}

    Public CMD_PREFIX_AMC_WRITETK3() As Byte = New Byte() { _
                                        &H2, &H42, &H1C, &H50, &H4E, &H32, &H1C, &H41, &H43 _
                                        , &H57, &H1C, &H44, &H54}

    Public CMD_PREFIX_LOAD_TEXT() As Byte = New Byte() {&H2, &H4D, &H1C, &H43, &H31, &H4E}

    Public Shared SEND_DATA_ACK() As Byte = New Byte() {&H6}

    Public Shared SEND_DATA_ENROLL() As Byte = New Byte() {&H2, &H56, &H1C, &H42, &H46, &H42, &H3, &HC}
    Public Shared SEND_DATA_HARDTOENROLL() As Byte = New Byte() {&H2, &H56, &H1C, &H42, &H46, &H46, &H3, &HB}
    Public Shared SEND_DATA_VERIFY() As Byte = New Byte() {&H2, &H56, &H1C, &H42, &H46, &H43, &H3, &HC}

    Public Shared SEND_DATA_AMC_ACK() As Byte = New Byte() { _
                                          &H2, &H42, &H1C, &H50, &H4E, &H32, &H1C, &H44 _
                                        , &H54, &H30, &H30, &H31, &H5E, &H3, &H12}

    Public Shared SEND_DATA_AMC_RESET_ACK() As Byte = New Byte() { _
                                          &H2, &H42, &H1C, &H50, &H4E, &H32, &H1C, &H44, &H54 _
                                        , &H30, &H30, &H31, &H3A, &H3, &H76}

    Public Shared SEND_DATA_YES_CLICK() As Byte = New Byte() { _
                                        &H2, &H56, &H1C, &H42, &H46, &H4B, &H3, &HC}

    Public Shared SEND_DATA_PASS_RESP() As Byte = New Byte() {&H2, &H58, &H4E, &H3, &H15}

End Class
