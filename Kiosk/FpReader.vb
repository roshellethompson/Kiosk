Imports System.IO.Ports
Imports System.Threading
Imports System

'Purpose: FPReader class is software interface to communicate with Validator Fingerprint Reader
'
'  Steps: 1) Establish instance of FPReader object
'         2) Call FPReader.setMode(ValidatorAPI.FPReader.enumRunMode.SIMULATION)
'         3) Call FPReader.initialize()
'         4) Confirm that initialize function returned zero (success)
Public Class FPReader

    Private runMode As Integer

    Shared _continue As Boolean
    Shared _serialPort As SerialPort
    Shared fpReaderMonitorThread As Thread
    Shared writingKeypadNumber As Boolean

    Shared readBuffer(1000) As Byte
    Shared writeBuffer(1000) As Byte
    Shared tempBuffer(1000) As Byte
    Shared readBufferOffset As Integer

    Shared copyStart As Integer
    Shared copyLength As Integer
    Shared junkBytes As Integer

    Shared DETAILED_LOGGING As Boolean

    Shared status = STATUS_IDLE

    Shared STATUS_CONNECTING = 1
    Shared STATUS_IDLE = 2
    Shared STATUS_ENROLL_RETRY = 3 'waiting for reset so we can start over
    Shared STATUS_ENROLL_HARDTOENROLL = 4 'waiting for reset so we can do hard2enroll
    Shared STATUS_ENROLL = 5
    Shared STATUS_ENROLL_COMPLETE = 6 'waiting for reset back to command form
    Shared STATUS_VERIFY = 7
    Shared STATUS_VERIFY_COMPLETE = 8 'waiting for reset back to command form

    Shared track1Data As String = ""
    Shared track2Data As String = ""
    Shared track3Data As String = ""

    Shared Event EnrollSuccess(ByVal Track3 As String)
    Shared Event EnrollFailed(ByVal message As String)
    Shared Event EnrollOverflowError()
    Shared Event EnrollError(ByVal message As String)
    Shared Event VerifySuccess(ByVal textLine1 As String, _
                                ByVal textLine2 As String, _
                                ByVal textLine3 As String, _
                                ByVal textLine4 As String)
    Shared Event VerifyFailed(ByVal message As String)
    Shared Event VerifyError(ByVal message As String)
    Shared Event UpdatePrompt(ByVal prompt As String)
    Shared Event PlaceFinger()
    Shared Event EnterPin()


    Public Enum enumRunMode
        SIMULATION = 0
        PRODUCTION = 1
    End Enum

    'Function: setMode()
    '  Inputs: None
    ' Returns: Nothing
    ' Purpose: Set the mode of the FPReader API
    '          This allows the API to be set up in production mode or simulation mode
    '          Production mode expects the hardware to be connected operational
    '          Simulation mode enables remote development without hardware
    '
    ' Example: setMode(FPReader.enumRunMode.SIMULATION)
    '
    Public Sub setMode(ByVal mode As enumRunMode)

        Select Case mode
            Case enumRunMode.PRODUCTION
                runMode = enumRunMode.PRODUCTION
            Case enumRunMode.SIMULATION
                runMode = enumRunMode.SIMULATION
            Case Else
                runMode = enumRunMode.SIMULATION
        End Select
    End Sub

    'Function: getVersion()
    '  Inputs: None
    ' Returns: String - Version Number
    '
    ' Purpose: To get the assembly version of the API.  This is useful when testing multiple versions.
    '
    Public Function getVersion() As String

        Return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()

    End Function

    'Function: initialize(commPort)
    '  Inputs: commPort - Examples: "COM1", "COM2", etc.
    ' Returns: Integer - 0=SUCCESS Else=Error Code
    '
    ' Purpose: To initialize the Fingerprint Reader device
    '
    Public Function initialize(ByVal commPort As String) As Integer

        log("Initializing fingerprint device...")
        runMode = enumRunMode.PRODUCTION
        Select Case runMode
            Case enumRunMode.SIMULATION
                'Dim simulatorDialog As New Dialog_InitializeSimulator
                'simulatorDialog.ShowDialog()
                'Return simulatorDialog.initializeResponse
            Case enumRunMode.PRODUCTION
                ' Instantiate thread that will monitor communication with the FingerPrint Reader device
                fpReaderMonitorThread = New Thread(AddressOf FpReaderMonitor)

                ' Create a new SerialPort object with default settings.
                _serialPort = New SerialPort()


                ' Allow the user to set the appropriate properties.
                _serialPort.PortName = commPort
                _serialPort.BaudRate = 19200
                _serialPort.Parity = Parity.None
                _serialPort.DataBits = 8
                _serialPort.StopBits = StopBits.One
                _serialPort.Handshake = Handshake.None

                ' Set the read/write timeouts
                _serialPort.ReadTimeout = 500
                _serialPort.WriteTimeout = 500

                Try
                    _serialPort.Open()
                Catch ex As System.IO.IOException
                    log("IOException Error opening " & commPort & " (port probably doesn't exist)")
                    Return FpReaderCommand.ERROR_INVALID_PORT
                Catch ex As System.UnauthorizedAccessException
                    log("Unauthorized Exception while opening " & commPort & " (port probably already open for another device)")
                    Return FpReaderCommand.ERROR_PORT_ALREADY_IN_USE
                End Try

                _continue = True

                readBufferOffset = 0
                status = STATUS_CONNECTING
                junkBytes = 0 'Keep track of junk bytes on startup

                log("Attempting to connect to validator on " & commPort)

                fpReaderMonitorThread.IsBackground = True
                fpReaderMonitorThread.Start()

                writingKeypadNumber = False

                writeKeypadNumber(1) 'This is just to write something so that we get an ACK back if validator is alive.

                For j = 1 To 5
                    Sleep(2000)
                    If (status = STATUS_IDLE) Then
                        If (junkBytes > 0) Then
                            log("Received response, but there have been " & junkBytes & " bytes of junk data sent")
                            log("Ignoring response and trying again")
                            status = STATUS_CONNECTING
                            junkBytes = 0 'reset junk byte count
                        Else
                            log("Found Validator on " & commPort)
                            Return 0
                        End If
                    End If

                    writeKeypadNumber(1) 'This is just to write something so that we get an ACK back if validator is alive.
                Next

                log("TIMEOUT - Could not find Validator on " & commPort)

                Close()

                Return FpReaderCommand.ERROR_FP_READER_NOT_FOUND

        End Select

        Return FpReaderCommand.ERROR_INIT_PROBLEM

    End Function

    'Function: initiateEnroll(track1, track2)
    '  Inputs: track1 - The text that exists on magnetic strip track 1 for the card
    '          track2 - The text that exists on magnetic strip track 2 for the card
    ' Returns: None
    '
    ' Purpose: To initiate the Enroll Sequence.  This function does not return any values
    '          because it's only purpose is to kick off the enroll sequence, which then 
    '          runs in a separate thread.  The calling application should expect a
    '          sequence of events, such as PlaceFinger, UpdatePrompt, EnrollSuccess, etc.
    '
    Public Sub initiateEnroll(ByVal track1 As String, ByVal track2 As String)

        Select Case runMode
            Case enumRunMode.SIMULATION
                'Dim simulatorDialog As New Dialog_EnrollSimulator
                'simulatorDialog.initFpReader(Me)
                'simulatorDialog.Show()
            Case enumRunMode.PRODUCTION

                If (status <> STATUS_IDLE) Then
                    For i = 0 To 40
                        RaiseEvent UpdatePrompt("Validator is busy, waiting...")
                        Sleep(500)
                        If (status = STATUS_IDLE) Then
                            Exit For
                        End If
                    Next
                End If
                If (status <> STATUS_IDLE) Then
                    RaiseEvent EnrollError("ERROR - Validator not responding")
                    Return
                End If
                log("beginning enroll sequence")

                track1Data = track1
                track2Data = track2

                writeResponse(FpReaderCommand.SEND_DATA_ENROLL)
                status = STATUS_ENROLL
                'RaiseEvent UpdatePrompt("Beginning Enrollment Sequence...")
        End Select

    End Sub

    'Function: retryEnroll()
    '  Inputs: None
    ' Returns: None
    '
    ' Purpose: To re-initiate the Enroll Sequence.
    '          This function is identical to initiateEnroll, except it does not require
    '          track1 or track2, and the enroll sequence utilizes the track1/track2 values
    '          of the last initiateEnroll() call that was performed.  The purpose of this
    '          function is to allow the calling function to re-initiate an Enroll after
    '          the previous attempt resulted an EnrollOverflow error.  The main difference
    '          with this retryEnroll() function is that it utilizes the PIN input from the
    '          previous enroll attempt.  By calling this function, you avoid asking the user
    '          to enter the same PIN a second time just because an EnrollOverflow event was
    '          raised.  This function does not return any values because it's only purpose 
    '          is to kick off the enroll sequence, which then runs in a separate thread.  
    '          The calling application should expect a sequence of events, such as 
    '          PlaceFinger, UpdatePrompt, EnrollSuccess, etc.
    '
    Public Sub retryEnroll()

        log("restarting enroll sequence")

        Select Case runMode
            Case enumRunMode.SIMULATION
                'Dim simulatorDialog As New Dialog_EnrollSimulator
                'simulatorDialog.initFpReader(Me)
                'simulatorDialog.Show()

                'RaiseEvent UpdatePrompt("Lift Finger")
            Case enumRunMode.PRODUCTION

                status = STATUS_ENROLL_RETRY
                RaiseEvent UpdatePrompt("Lift Finger")
        End Select

    End Sub

    'Function: initiateHardToEnroll()
    '  Inputs: track1 - The text that exists on magnetic strip track 1 for the card
    '          track2 - The text that exists on magnetic strip track 2 for the card
    ' Returns: None
    '
    ' Purpose: To initiate the Hard-to-Enroll Sequence.  This function is similar to the
    '          initiateEnroll function, but the threshholds for a successful enroll are
    '          relaxed to increase the likelihood of a successful enrollment.
    '          A regular enroll is more reliable if it can be obtained, and for most users
    '          a regular enroll will work OK.  However, if the enrollment fails (possibily 
    '          after multiple attempts depending on the implementation), then the calling
    '          program can call the Hard-to-Enroll to ensure that every user can be enrolled.
    '
    Public Sub initiateHardToEnroll(ByVal track1 As String, ByVal track2 As String)

        Select Case runMode
            Case enumRunMode.SIMULATION
                'Dim simulatorDialog As New Dialog_EnrollSimulator
                'simulatorDialog.initFpReader(Me)
                'simulatorDialog.Show()

                'RaiseEvent UpdatePrompt("Lift Finger")
            Case enumRunMode.PRODUCTION
                track1Data = track1
                track2Data = track2

                log("starting hard-to-enroll sequence")

                writeResponse(FpReaderCommand.SEND_DATA_HARDTOENROLL)
                status = STATUS_ENROLL_HARDTOENROLL
                RaiseEvent UpdatePrompt("Lift Finger")
        End Select
    End Sub

    'Function: initiateVerify(track1, track2, track3)
    '  Inputs: track1 - The text that exists on magnetic strip track 1 for the card
    '          track2 - The text that exists on magnetic strip track 2 for the card
    '          track3 - The text that exists on magnetic strip track 3 for the card
    ' Returns: None
    '
    ' Purpose: To initiate the Verify Sequence.  This function does not return any values
    '          because it's only purpose is to kick off the verify sequence, which then 
    '          runs in a separate thread.  The calling application should expect a
    '          sequence of events, such as PlaceFinger, UpdatePrompt, VerifySuccess, etc.
    '
    Public Sub initiateVerify(ByVal track1 As String, ByVal track2 As String, ByVal track3 As String)

        Select Case runMode
            Case enumRunMode.SIMULATION
                'Dim simulatorDialog As New Dialog_VerifySimulator
                'simulatorDialog.initFpReader(Me)
                'simulatorDialog.Show()
            Case enumRunMode.PRODUCTION
                If (status <> STATUS_IDLE) Then
                    For i = 0 To 10
                        RaiseEvent UpdatePrompt("Validator is busy, waiting...")
                        Sleep(500)
                        If (status = STATUS_IDLE) Then
                            Exit For
                        End If
                    Next
                End If
                If (status <> STATUS_IDLE) Then
                    RaiseEvent VerifyError("ERROR - Validator not responding")
                    Return
                End If
                log("beginning verify sequence")

                track1Data = track1
                track2Data = track2
                track3Data = track3

                writeResponse(FpReaderCommand.SEND_DATA_VERIFY)
                status = STATUS_VERIFY

                'RaiseEvent UpdatePrompt("Initiating Verification Sequence...")
        End Select

    End Sub

    'Function: writeKeypadNumber(number)
    '  Inputs: number - The digit to write back to the API (0,1,2,3,4,5,6,7,8, or 9)
    ' Returns: None
    '
    ' Purpose: To write a keypad number to the API when the "EnterPin" event has been raised.
    '          This function should only be called when the API is asking for a 3-digit PIN.
    '          This function should be called three times - once for each digit to write.
    '
    Public Sub writeKeypadNumber(ByVal number As Integer)
        Sleep(500)
        If writingKeypadNumber Then
            log("Pause because it's already writing.")
            Sleep(1000)
        End If

        writingKeypadNumber = True
        Dim prefix() As Byte = New Byte() {&H2, &H56, &H1C, &H42, &H46}
        Dim suffix() As Byte = New Byte() {&H3, &HC}

        Array.Copy(prefix, 0, writeBuffer, 0, prefix.Length)
        writeBuffer(prefix.Length) = &H40 + number
        If (number = 0) Then
            writeBuffer(prefix.Length) = &H4A
        End If
        Array.Copy(suffix, 0, writeBuffer, prefix.Length + 1, suffix.Length)
        If (DETAILED_LOGGING) Then
            log("writing pin number: " & number)
        End If

        writeData(writeBuffer, 0, prefix.Length + 1 + suffix.Length)
        writingKeypadNumber = False

    End Sub


    'Function: Close()
    '  Inputs: None
    ' Returns: None
    '
    ' Purpose: Close the serial port and stop the infinite thread.
    '          This function should only be called as a "cleanup" function if the calling
    '          program needs to stop the API FpReader monitoring activities and close
    '          the serial port.
    '
    Public Sub Close()
        _continue = False
        _serialPort.Close()
        fpReaderMonitorThread.Join()
    End Sub

    'Function: FpReaderMonitor()
    '  Inputs: None
    ' Returns: None
    '
    ' Purpose: This is the continuous loop that runs in a stand-alone thread for the API.
    '          This function monitors incoming and outgoing data on the serial port, 
    '          manages the read and write buffers, maintains the state of the device,
    '          and raises appropriate events as applicable.
    '
    '          Note - This function shall never be called by an application - it is only
    '                 called from within the API (initiated upon instantiation)
    '
    Private Sub FpReaderMonitor()

        Dim bytesRead As Integer

        While (_continue)
            Try
                If _serialPort.BytesToRead > 0 Then
                    'log("Bytes to Read: " & _serialPort.BytesToRead & "    - offset: " & readBufferOffset)
                    bytesRead = _serialPort.Read(readBuffer, readBufferOffset, _serialPort.BytesToRead)
                    If (bytesRead > 0) Then
                        'log("Bytes Read: " & bytesRead)
                        'log("readbufferoffset: " & readBufferOffset)
                        Dim k = readBufferOffset + bytesRead - 1
                        'log("k: " & k)
                        For j = readBufferOffset To k
                            If DETAILED_LOGGING Then
                                log("j: " & j & " = " & Byte2Char(readBuffer(j)) & " (0x" & HexByte2Char(readBuffer(j)) & ")")
                                'Console.Write(Byte2Char(readBuffer(j)))
                            End If
                        Next j
                        readBufferOffset += bytesRead
                        'log("new readbufferoffset: " & readBufferOffset)

                    Else
                        log("There were bytes to read, but no bytes were read????")
                    End If
                Else
                    If (DETAILED_LOGGING) Then
                        Console.Write(".")
                    End If
                End If

                Dim commandStart As Integer = -1
                Dim commandLength As Integer = -1

                If (readBufferOffset > 0) Then

                    For a = 0 To (readBufferOffset - 1)
                        If (readBuffer(a) = &H6) Then
                            commandStart = a
                            commandLength = 1
                            Exit For
                        ElseIf (readBuffer(a) = &H2) Then
                            commandStart = a
                        ElseIf (readBuffer(a) = &H3) And (commandStart >= 0) Then
                            commandLength = (a + 1 - commandStart)
                            Exit For
                        End If
                    Next

                    'log("Command Start: " & commandStart & " - commandLength: " & commandLength)
                End If

                If (commandStart > 0) Then
                    If (DETAILED_LOGGING) Then
                        log("BYTES BEING TRASHED")
                    End If
                    For b = 0 To commandStart - 1
                        If DETAILED_LOGGING Then
                            log("b: " & b & " = " & Byte2Char(readBuffer(b)) & " (0x" & HexByte2Char(readBuffer(b)) & ")")
                        End If
                        junkBytes += 1
                    Next
                End If

                If (commandStart >= 0 And commandLength > 0) Then
                    Dim fpReaderCommand As New FpReaderCommand(readBuffer, commandStart, commandLength)
                    'log("readBufferOffset: " & readBufferOffset)
                    copyStart = commandStart + commandLength
                    copyLength = readBufferOffset - commandLength - commandStart
                    'log("copystart: " & copyStart & " - copylength: " & copyLength)
                    If (copyLength > 0) Then
                        'log("copying arrays")
                        Array.Copy(readBuffer, copyStart, tempBuffer, 0, copyLength)
                        Array.Copy(tempBuffer, readBuffer, copyLength)
                    End If
                    'log("readBufferOffset: " & readBufferOffset & "  - now subtracting copyStart = " & copyStart)
                    readBufferOffset -= copyStart
                    'log("readBufferOffset: " & readBufferOffset)

                    'log("new readbufferoffset: " & readBufferOffset)

                    If (status = STATUS_CONNECTING) Then
                        If fpReaderCommand.command = fpReaderCommand.COMMAND_ACK Then
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            log("received ACK while connecting - FOUND THE VALIDATOR")
                            status = STATUS_IDLE
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_CMD Then
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            log("received show command form while connecting - FOUND THE VALIDATOR")
                            status = STATUS_IDLE
                        End If
                    ElseIf (status = STATUS_IDLE) Then
                        If fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_CMD Then
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            log("received show command form while idle - OK")
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_ACK Then
                            log("received ACK - OK")
                        Else
                            log("invalid command while in IDLE status: " & fpReaderCommand.command)
                        End If
                    ElseIf (status = STATUS_VERIFY_COMPLETE) Then
                        If fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_CMD Then
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            log("received show command form - Now back to IDLE")
                            status = STATUS_IDLE
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_TEXT Then
                            log("received show text from last verify - just ignoring")
                            'don't send ACK, or else validator will pause for 90 seconds
                        Else
                            log("invalid command while in VERIFY_COMPLETE status: " & fpReaderCommand.command)
                        End If
                    ElseIf (status = STATUS_ENROLL_COMPLETE) Then
                        If fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_CMD Then
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            log("received show command form - Now back to IDLE")
                            status = STATUS_IDLE
                        Else
                            log("invalid command while in ENROLL_COMPLETE status: " & fpReaderCommand.command)
                        End If

                    ElseIf (status = STATUS_VERIFY) Then
                        If fpReaderCommand.command = fpReaderCommand.COMMAND_ACK Then
                            log("received ACK - OK")
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_OPEN_PASSTHROUGH Then
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            log("received open passthrough - OK")
                            writeResponse(fpReaderCommand.SEND_DATA_PASS_RESP)
                            log("sent open passthrough response")
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_AMC_RESET Then
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            writeResponse(fpReaderCommand.SEND_DATA_AMC_RESET_ACK)
                            log("received AMC reset - OK")
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_AMC_ARM2READ Then
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            writeResponse(fpReaderCommand.SEND_DATA_AMC_ACK)
                            log("received AMC Arm to Read - OK")
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_AMC_SWIPE Then
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            writeResponse(fpReaderCommand.SEND_DATA_AMC_ACK)
                            log("received AMC Swipe - OK")
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_AMC_READTK1 Then
                            'writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            'writeResponse(fpReaderCommand.SEND_DATA_AMC_ACK)
                            log("received AMC Read Track 1 - OK")
                            writeTrackData(track1Data)
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_AMC_READTK2 Then
                            'writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            'writeResponse(fpReaderCommand.SEND_DATA_AMC_ACK)
                            log("received AMC Read Track 2 - OK")
                            writeTrackData(track2Data)
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_AMC_READTK3 Then
                            'writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            'writeResponse(fpReaderCommand.SEND_DATA_AMC_ACK)
                            log("received AMC Read Track 3 - OK")
                            writeTrackData(track3Data)
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_PLACEFINGER Then
                            RaiseEvent PlaceFinger()
                            log("received show place finger - OK")
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_LIFTFINGER Then
                            RaiseEvent UpdatePrompt("Lift Finger")
                            log("received show lift finger - OK")
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_STATUS Then
                            log("received show status form - OK")
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SHOW_STATUS_MESSAGE Then
                            log("received show status - OK")
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            RaiseEvent UpdatePrompt(fpReaderCommand.statusText)
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_ENTERPIN Then
                            log("received show enter pin - OK")
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            RaiseEvent UpdatePrompt("Enter 3-digit PIN")
                            RaiseEvent EnterPin()
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_VERIFYFAIL Then
                            log("verify failed")
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            RaiseEvent VerifyFailed("Verification Failed")
                            status = STATUS_VERIFY_COMPLETE
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_LOAD_TEXT Then
                            log("verify ok")
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            RaiseEvent UpdatePrompt("VERIFY SUCCESSFUL")
                            RaiseEvent VerifySuccess(fpReaderCommand.textLine1, _
                                                        fpReaderCommand.textLine2, _
                                                        fpReaderCommand.textLine3, _
                                                        fpReaderCommand.textLine4)
                            status = STATUS_VERIFY_COMPLETE
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_TEXT Then
                            log("received show text - OK")
                            'Don't send ACK so that it will time out - otherwise, it pauses for 90 seconds
                            'writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            status = STATUS_VERIFY_COMPLETE
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_VERIFYOK Then
                            log("verify ok")
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            status = STATUS_IDLE
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_CMD Then
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            log("received show command form while in verify - Aborted?")
                            RaiseEvent VerifyError("Aborted")
                            status = STATUS_IDLE
                        Else
                            log("invalid command while in VERIFY status: " & fpReaderCommand.command)
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            RaiseEvent VerifyError("unknown command")
                            status = STATUS_IDLE
                        End If
                    ElseIf (status = STATUS_ENROLL) _
                            Or (status = STATUS_ENROLL_RETRY) _
                            Or (status = STATUS_ENROLL_HARDTOENROLL) Then
                        If fpReaderCommand.command = fpReaderCommand.COMMAND_ACK Then
                            log("received ACK - OK")
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_OPEN_PASSTHROUGH Then
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            log("received open passthrough - OK")
                            writeResponse(fpReaderCommand.SEND_DATA_PASS_RESP)
                            log("sent open passthrough response")
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_AMC_RESET Then
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            writeResponse(fpReaderCommand.SEND_DATA_AMC_RESET_ACK)
                            log("received AMC reset - OK")
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_AMC_ARM2READ Then
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            writeResponse(fpReaderCommand.SEND_DATA_AMC_ACK)
                            log("received AMC Arm to Read - OK")
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_AMC_SWIPE Then
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            writeResponse(fpReaderCommand.SEND_DATA_AMC_ACK)
                            log("received AMC Swipe - OK")
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_AMC_READTK1 Then
                            'writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            'writeResponse(fpReaderCommand.SEND_DATA_AMC_ACK)
                            log("received AMC Read Track 1 - OK")
                            writeTrackData(track1Data)
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_AMC_READTK2 Then
                            'writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            'writeResponse(fpReaderCommand.SEND_DATA_AMC_ACK)
                            log("received AMC Read Track 2 - OK")
                            writeTrackData(track2Data)
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_AMC_READTK3 Then
                            'writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            'writeResponse(fpReaderCommand.SEND_DATA_AMC_ACK)
                            log("received AMC Read Track 3 - OK")
                            writeTrackData("SAMPLE TRACK 3 DATA")
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_DATATK3 Then
                            log("received show data track 3 - OK")
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            writeResponse(fpReaderCommand.SEND_DATA_YES_CLICK)
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_PLACEFINGER Then
                            RaiseEvent PlaceFinger()
                            log("received show place finger - OK")
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            If status = STATUS_ENROLL_RETRY Or status = STATUS_ENROLL_HARDTOENROLL Then
                                'if we're in retry or hard2enroll mode, go ahead and switch back to normal enroll mode upon place finger
                                status = STATUS_ENROLL
                            End If
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_STATUS Then
                            log("received show status form - OK")
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SHOW_STATUS_MESSAGE Then
                            If (fpReaderCommand.statusText.StartsWith("Overflow")) Then
                                log("received OVERFLOW error")
                                writeResponse(fpReaderCommand.SEND_DATA_ACK)
                                log(" ---------------------------- OVERFLOW! -------------------------")
                                RaiseEvent EnrollOverflowError()
                            Else
                                log("received show status - OK")
                                writeResponse(fpReaderCommand.SEND_DATA_ACK)
                                RaiseEvent UpdatePrompt(fpReaderCommand.statusText)
                            End If
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_LIFTFINGER Then
                            RaiseEvent UpdatePrompt("Lift Finger")
                            log("received show lift finger - OK")
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_ENTERPIN Then
                            log("received show enter pin - OK")
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            RaiseEvent UpdatePrompt("Processing...(pin)")
                            RaiseEvent EnterPin()
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_WAIT Then
                            log("received show wait form - OK")
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            RaiseEvent UpdatePrompt("Processing...")
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SETCOERCIVITY Then
                            log("received set coercivity - OK")
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            writeResponse(fpReaderCommand.SEND_DATA_AMC_ACK)
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_HYPERCOMSWIPE Then
                            log("received hypercom card swipe - OK")
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            'writeHypercomTrackData(track1Data, track2Data)
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_AMC_UNKNOWN1 Then
                            log("received unknown AMC command1")
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            writeResponse(fpReaderCommand.SEND_DATA_AMC_ACK)
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_AMC_UNKNOWN2 Then
                            log("received unknown AMC command2")
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            writeResponse(fpReaderCommand.SEND_DATA_AMC_ACK)
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_AMC_WRITETK3 Then
                            track3Data = fpReaderCommand.track3
                            log("received AMC write tk3 - OK")
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            writeResponse(fpReaderCommand.SEND_DATA_AMC_ACK)
                            writeResponse(fpReaderCommand.SEND_DATA_AMC_ACK)
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_ENROLLOK Then
                            log("ENROLL SUCCESSFUL!!")
                            RaiseEvent UpdatePrompt("ENROLL SUCCESSFUL")
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            log("raising event: " & fpReaderCommand.track3)
                            RaiseEvent EnrollSuccess(track3Data)
                            status = STATUS_ENROLL_COMPLETE
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_ENROLLFAIL Then
                            If status = STATUS_ENROLL_RETRY Or status = STATUS_ENROLL_HARDTOENROLL Then
                                log("got the enrollfail message during ENROLL_RETRY or ENROLL_HARD2ENROLL - ignoring")
                                writeResponse(fpReaderCommand.SEND_DATA_ACK)
                            Else
                                log("ENROLL FAILED!!")
                                'RaiseEvent UpdatePrompt("ENROLL FAILED")
                                writeResponse(fpReaderCommand.SEND_DATA_ACK)
                                status = STATUS_ENROLL_COMPLETE
                                RaiseEvent EnrollFailed("Enroll Failed")
                            End If
                        ElseIf fpReaderCommand.command = fpReaderCommand.COMMAND_SHOWFORM_CMD Then
                            If status = STATUS_ENROLL_RETRY Then
                                writeResponse(fpReaderCommand.SEND_DATA_ACK)
                                log("got CMD form during ENROLL_RETRY, sending enroll command to start over")
                                writeResponse(fpReaderCommand.SEND_DATA_ENROLL)
                            ElseIf status = STATUS_ENROLL_HARDTOENROLL Then
                                writeResponse(fpReaderCommand.SEND_DATA_ACK)
                                log("got CMD form during ENROLL_HARDTOENROLL, sending hard-to-enroll command")
                                writeResponse(fpReaderCommand.SEND_DATA_HARDTOENROLL)
                            Else
                                writeResponse(fpReaderCommand.SEND_DATA_ACK)
                                log("received show command form while in ENROLL - Aborted?")
                                RaiseEvent EnrollError("Aborted")
                                status = STATUS_IDLE
                            End If
                        Else
                            log("invalid command while in ENROLL_INITIATED status: " & fpReaderCommand.command)
                            writeResponse(fpReaderCommand.SEND_DATA_ACK)
                        End If

                    End If



                End If

            Catch ex As TimeoutException
                ' Do nothing
            End Try

            Sleep(100)

        End While
    End Sub

    ' internal function just for writing data
    Shared Sub writeResponse(ByRef responseData As Byte())
        writeData(responseData, 0, responseData.Length)
    End Sub

    ' internal function for writing mag-strip track information
    Shared Sub writeTrackData(ByVal dataString As String)

        ''log("WRITING TRACK DATA <<< " & dataString & " >>>")
        Dim prefix() As Byte = New Byte() { _
                                &H6, &H2, &H42, &H1C, &H50, &H4E, &H32, &H1C, &H44, &H54}
        Dim suffix() As Byte = New Byte() {&H3, &H0}

        Array.Copy(prefix, 0, writeBuffer, 0, prefix.Length)
        Array.Copy(System.Text.Encoding.UTF8.GetBytes(number2string(dataString.Length)), 0, _
                    writeBuffer, prefix.Length, 3)
        Array.Copy(System.Text.Encoding.UTF8.GetBytes(dataString), 0, _
                    writeBuffer, prefix.Length + 3, dataString.Length)
        Array.Copy(suffix, 0, writeBuffer, prefix.Length + 3 + dataString.Length, suffix.Length)

        'For i = 0 To (prefix.Length + 3 + dataString.Length + suffix.Length - 1)
        '   log("i: " & i & " = " & Byte2Char(writeBuffer(i)) & " (0x" & HexByte2Char(writeBuffer(i)) & ")")
        'Next
        writeData(writeBuffer, 0, prefix.Length + 3 + dataString.Length + suffix.Length)
    End Sub

    ' Utiliy function for writing data to the serial port
    Shared Sub writeData(ByRef array1 As Byte(), ByVal offset As Integer, ByVal length As Integer)
        If DETAILED_LOGGING Then
            For i = 0 To (length - 1)
                log("              i: " & i & " = " & Byte2Char(array1(i)) & " (0x" & HexByte2Char(array1(i)) & ")")
            Next
        End If

        _serialPort.Write(array1, offset, length)
    End Sub

    ' Utility function for converting a number to a string value
    Shared Function number2string(ByVal number As Integer) As String
        Dim tempString As String = ""
        If (number < 10) Then
            tempString = "00"
        ElseIf number < 100 Then
            tempString = "0"
        End If
        tempString = tempString & CStr(number)

        Return tempString
    End Function

    ' Utility function for converting a Hexidecimal number to an ASCII character value
    Shared Function HexByte2Char(ByVal Value As Byte) As String
        ' Return a byte value as a two-digit hex string. 
        HexByte2Char = IIf(Value < &H10, "0", "") & Hex$(Value)
    End Function

    ' Utility function for converting a Byte to an ASCII character value
    Shared Function Byte2Char(ByVal value As Byte) As String
        Byte2Char = IIf(value < &H20, "(0x" & HexByte2Char(value) & ")", ChrW(value))
    End Function

    '     Sub: log(message)
    '  Inputs: Message as String
    ' Returns: None
    '
    ' Purpose: To provide ability to log operational messages to console or to log file
    '
    Public Shared Sub log(ByVal message As String)
        Console.WriteLine(message)
    End Sub

    ' The following functions support simulation mode operations

    Public Sub simulatePlaceFinger()
        RaiseEvent PlaceFinger()
    End Sub

    Public Sub simulateUpdatePrompt(ByVal message As String)
        RaiseEvent UpdatePrompt(message)
    End Sub

    Public Sub simulateEnrollOverflowError()
        RaiseEvent EnrollOverflowError()
    End Sub

    Public Sub simulateEnrollError()
        RaiseEvent EnrollError("Enroll process encountered an error")
    End Sub

    Public Sub simulateEnrollFailed()
        RaiseEvent EnrollFailed("Enroll Failed")
    End Sub

    Public Sub simulateEnrollSuccess(ByVal template As String)
        RaiseEvent EnrollSuccess(template)
    End Sub

    Public Sub simulateVerifyFailed()
        RaiseEvent VerifyFailed("Verify Failed")
    End Sub

    Public Sub simulateVerifyError()
        RaiseEvent VerifyError("Verify process encountered an error")
    End Sub

    Public Sub simulateVerifySuccess(ByVal line1 As String, ByVal line2 As String, ByVal line3 As String, ByVal line4 As String)
        RaiseEvent VerifySuccess(line1, line2, line3, line4)
    End Sub

    ' utility function to allow the program to go to sleep for a certain number of milliseconds
    Private Declare Sub Sleep Lib "kernel32" Alias "Sleep" (ByVal dwMilliseconds As Long)

End Class
