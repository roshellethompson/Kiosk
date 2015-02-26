Imports System

Public Class clsLog
    Public fLog As System.IO.FileStream
    Public Property FileName As String = "kioskTest.txt"
    Dim iSeq As Integer = 0

    Public Property Seq As Integer
        Get
            Return iSeq
        End Get
        Set(ByVal value As Integer)
            iSeq = value
        End Set
    End Property
    Public Function Clone() As clsLog
        Return Me
    End Function
    ''' <summary>
    ''' Log activity to a text file
    ''' </summary>
    ''' <param name="strMsg"></param>
    ''' <remarks></remarks>
    Public Sub LogMsg(ByVal strMsg As String)
        Try
            Close()
            fLog = New System.IO.FileStream(FileName, System.IO.FileMode.Append)

            strMsg = Date.Now.ToString + ": " + strMsg + vbCrLf
            Dim arrMsg(strMsg.Length) As Byte
            Dim i As Integer = 0
            For Each c In strMsg
                arrMsg(i) = AscW(c)
                i += 1
            Next
            fLog.Write(arrMsg, 0, arrMsg.Length)
            Close()
        Catch ex As Exception
            Close()
            LogMsg(strMsg)
        End Try

    End Sub

    Public Sub New(ByVal strFileName As String)


        FileName = strFileName.Substring(0, strFileName.IndexOf("."))
        FileName = strFileName
        Try
            Close()
            fLog = New System.IO.FileStream(FileName, System.IO.FileMode.Append)
        Catch ex As Exception
            Close()
        End Try


    End Sub
    Public Sub New()

    End Sub
    Public Sub Close()
        If Not fLog Is Nothing Then
            fLog.Close()
        End If
    End Sub

End Class
