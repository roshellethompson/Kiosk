Imports AxIS380OCX
Imports System.Configuration
Imports System

Public Class clsIS380
    Inherits AxIS380OCX.AxIS380
    Private pMCPBus As New IS380OCX.MCPBusType


    Public Sub OpenPort(ByVal DevName As String)
        Try
            DeviceName = DevName
            InitMcpBus(pMCPBus)
            AutoEjectEnable = False
            AutoConsume = True
            CardDataChangedEventEnabled = True
            MSRReadDirManual = IS380OCX.ReadDirection.READ_BOTH

            If PortOpen = True Then
                PortOpen = False
            Else
                PortOpen = True
            End If
        Catch ex As Exception
            MessageBox.Show(LastErrorNum + " : " + LastErrorString)
        End Try
    End Sub
    Public Function GetDeviceName() As String
        CreateControl()
        Dim objDevices As Object = System.Reflection.Missing.Value
        objDevices = EnumerateMCPDevices()
        Dim nDev As Integer = objDevices.GetUpperBound(0)
        Dim enDevList As IEnumerable = CType(objDevices, IEnumerable)
        Dim o As Object
        Dim strIS380 As String = ""
        For Each o In enDevList
            strIS380 = CType(o, String)
        Next
        Return strIS380
    End Function


    Public Sub WriteToCard(ByVal strTrack1 As String, ByVal strTrack2 As String, ByVal strTrack3 As String)
        Dim strTrk1 As String = strTrack1
        Dim strTrk2 As String = strTrack2
        Dim strTrk3 As String = strTrack3
        Dim strTrk3Hex As String = strTrk3
        If strTrk1.Length > 77 Then
            strTrk1 = strTrk1.Substring(0, 77)
        Else
            strTrk1 = strTrk1.PadRight(77, " ")
        End If
        If strTrk2.Length > 37 Then
            strTrk2 = strTrk2.Substring(0, 37)
        Else
            strTrk2 = strTrk2.PadRight(37, "0")
        End If
        If strTrk3Hex.Length > 78 Then
            strTrk3Hex = strTrk3Hex.Substring(0, 78)
        End If
        'strTrk3Hex = "+" + strTrk3Hex
        'strTrk2 = ";" + strTrk2 + "?"
        'strTrk1 = "%" + strTrk1 + "?"
        Dim Track1 As String
        Dim Track2 As String
        Dim Track3 As String
        Dim bTrack1 As Boolean
        Dim bTrack2 As Boolean
        Dim bTrack3 As Boolean
        Call InitMcpBus(pMCPBus)
        pMCPBus.OutBuffer = Space(4096)
        pMCPBus.OutBufferSize = 4096


        Dim Rtrn As Integer = 0
        Track1 = strTrk1
        Track2 = strTrk2
        Track3 = strTrk3Hex

        If Track1 = "" Then bTrack1 = False Else bTrack1 = True
        If Track2 = "" Then bTrack2 = False Else bTrack2 = True
        If Track3 = "" Then bTrack3 = False Else bTrack3 = True
        pMCPBus.PropertyName = vbNullString
        pMCPBus.PropertyType = 1
        pMCPBus.ResponseLen = 0
        pMCPBus.ResultCode = 0


        'Set Tracks to 210 BPI
        If bTrack1 = True Then
            pMCPBus.ApplicationID = &H1
            pMCPBus.CommandID = &H1
            pMCPBus.PropertyID = &H12
            pMCPBus.InBuffer = Chr(5)
            pMCPBus.InBufferLen = 4
            Rtrn = IS380_MCPSet(pMCPBus)
            If Rtrn <> 0 Then MsgBox(Rtrn)


        End If

        If bTrack2 = True Then
            pMCPBus.ApplicationID = &H1
            pMCPBus.CommandID = &H1
            pMCPBus.PropertyID = &H13
            pMCPBus.InBuffer = Chr(2)
            pMCPBus.InBufferLen = 4
            Rtrn = IS380_MCPSet(pMCPBus)
            If Rtrn <> 0 Then MsgBox(Rtrn)

        End If

        If bTrack3 = True Then
            pMCPBus.ApplicationID = &H1
            pMCPBus.CommandID = &H1
            pMCPBus.PropertyID = &H14
            pMCPBus.InBuffer = Chr(5)
            pMCPBus.InBufferLen = 4
            Rtrn = IS380_MCPSet(pMCPBus)
            If Rtrn <> 0 Then MsgBox(Rtrn)

        End If

        Dim iRet As Integer = 0
        iRet = WriteMagStripe123(Track1, Track2, Track3)
        If iRet <> 0 Then
            MsgBox(LastErrorString)
        End If
    End Sub

    Public Sub New()

    End Sub
End Class
