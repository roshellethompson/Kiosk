Imports System.Xml
Imports System.Configuration

Public Class clsError
    Public iErrCode As Integer = 0
    Public strErrText As String = ""
    Public strComponent As String = ""
    Public strAction As String = ""
    Dim oCheckCashing As New clsDataModule.clsInterface
    Dim oCustomer As clsCustomer
    Public Property ErrDetail As String = ""
    Public Property ErrCode As Integer
        Get
            Return iErrCode
        End Get
        Set(ByVal value As Integer)
            iErrCode = value
        End Set
    End Property
    Public Property ErrText As String
        Get
            Return strErrText
        End Get
        Set(ByVal value As String)
            strErrText = value
        End Set
    End Property
    Public Property Component As String
        Get
            Return strComponent
        End Get
        Set(ByVal value As String)
            strComponent = value
        End Set
    End Property
    Public Property Action As String
        Get
            Return strAction
        End Get
        Set(ByVal value As String)
            strAction = value
        End Set
    End Property
    Public Sub GetErrorData()
        Dim oXML As XmlElement = oCheckCashing.GetErrorData(ErrCode)
        For Each xmlNod In oXML
            If xmlNod.Name = "ErrCode" Then
                ErrCode = Integer.Parse(xmlNod.InnerText)
            ElseIf xmlNod.Name = "ErrText" Then
                ErrText = oCustomer.oLang.GetScreenLabel(ErrCode)
            ElseIf xmlNod.Name = "Component" Then
                Component = xmlNod.InnerText
            ElseIf xmlNod.name = "Action" Then
                Action = xmlNod.InnerText
            End If
        Next
        ErrText = oCustomer.oLang.GetScreenLabel(ErrCode)

    End Sub
    Public Sub New()

        ErrCode = 0
        Action = ""
        ErrText = ""
        Component = ""
        ErrDetail = ""
    End Sub
    Public Sub New(ByRef pCustomer As clsCustomer)

        oCustomer = pCustomer
        ErrCode = 0
        Action = ""
        ErrText = ""
        Component = ""
        ErrDetail = ""
    End Sub
    Public Function Clone() As clsError
        Return Me
    End Function
End Class
