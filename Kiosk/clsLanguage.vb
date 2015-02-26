Imports Microsoft.VisualBasic
Imports System.Xml
Imports System.Configuration

''' <summary>
''' This is the library for the language editor
''' </summary>
''' <remarks></remarks>
Public Class clsLanguage
    Dim arrCaptions As New ArrayList
    Dim iLangID As Integer = 0
    Dim strLang As String = ""
    Dim oCheckCashing As New clsDataModule.clsInterface
    Dim arrLanguages As New ArrayList
    Dim oChkService As New CheckService.ICheckServiceservice

    Public Property Languages As ArrayList
        Get
            Return arrLanguages
        End Get
        Set(ByVal value As ArrayList)
            arrLanguages.Clear()
            Dim lItem As clsDataModule.clsListItem
            Dim xmlEle As XmlNode
            Dim i As Integer = 0
            While i < value.Count
                lItem = value(i)
                arrLanguages.Add(lItem)
                lItem = New clsDataModule.clsListItem
                i += 1
            End While
        End Set
    End Property
    Public Property LangID As Integer
        Get
            Return iLangID
        End Get
        Set(value As Integer)
            iLangID = value
        End Set
    End Property
    Public Property Captions As ArrayList
        Get
            Return arrCaptions
        End Get
        Set(ByVal value As ArrayList)
            Dim lItem As clsDataModule.clsListItem
            Dim i As Integer = 0
            While i < value.Count
                lItem = value(i)
                arrCaptions.Add(lItem)
                lItem = New clsDataModule.clsListItem
                i += 1
            End While
        End Set
    End Property
    Public Property LanguageName As String
        Get
            Return strLang
        End Get
        Set(value As String)
            strLang = value
        End Set
    End Property

    Public Function FillLanguages() As ArrayList
        Languages = New ArrayList(oCheckCashing.FillLanguages())

    End Function
    Public Function GetScreenLabel(ByVal strScreenLabel As String) As String

        Dim lItem As clsDataModule.clsListItem
        For Each lItem In Captions
            If lItem.Text = strScreenLabel Then
                '    Dim c As Char = vbCrLf
                '    Dim i As Integer = 0
                '    Dim k As Integer = 0
                '    While i < Len(lItem.Value)
                '        If i <> 0 And (i Mod 20 = 0) Then
                '            If k = 0 Then
                '                lItem.Value = lItem.Value.Substring(k, i) + c + lItem.Value.Substring(i, Len(lItem.Value) - i)
                '            Else
                '                lItem.Value += lItem.Value.Substring(k, i) + c + lItem.Value.Substring(k + i, Len(lItem.Value) - (k + i))
                '            End If
                '            k += i + Len(c)
                '        End If
                '            i += 1
                '    End While
                Return lItem.Value
            End If
        Next
        Return strScreenLabel
    End Function

    Public Sub Refresh()

        Dim strLang() = oChkService.GetLanguageArrayStr(oCheckCashing.GetLangID(LanguageName))

        Dim objLang As New Object
        Dim arrCaptions As New ArrayList
        Captions.Clear()
        For Each objLang In strLang
            Dim FirstName As String = objLang.ToString.Substring(0, objLang.ToString.IndexOf(","))
            Dim strSecond As String = objLang.ToString.Substring(objLang.ToString.IndexOf(","), _
                                                                 objLang.ToString.Length - objLang.ToString.IndexOf(",")) _
                                                             .Replace(",", "")
            Dim myItem As New clsDataModule.clsListItem
            myItem.Text = FirstName
            myItem.Value = strSecond
            Captions.Add(myItem)
        Next

    End Sub

    Public Sub New()
        oChkService.Url = ConfigurationManager.AppSettings("CheckServiceURL")
    End Sub
End Class
