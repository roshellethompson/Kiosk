Imports System.Xml
Imports System
Imports System.Configuration

Public Class clsAffiliate

    Public Property AffiliateID As Integer = 0
    Public Property EntityID As Integer = 0
    Public Property Tier As Integer = 0
    Public Property EntitiesBK As Boolean = False
    Public Property EntitiesUS As Boolean = False
    Public Property EntitiesFN As Boolean = False
    Public Property EntitiesAT As Boolean = False
    Public Property oCheckCash As New clsDataModule.clsInterface
    Public Sub GetCompanyData()
        Dim xmlEle As XmlElement = oCheckCash.GetCompanyData(AffiliateID)
        For Each x As XmlNode In xmlEle.ChildNodes
            If x.Name = "EntityID" Then
                EntityID = Int32.Parse(x.InnerText)
            ElseIf x.Name = "Tier" Then
                If x.InnerText = "" Then
                    x.InnerText = "0"
                End If
                Tier = x.InnerText
            ElseIf x.Name = "BackgroundCheck" Then
                If x.InnerText = "0" Then
                    EntitiesBK = False
                Else
                    EntitiesBK = True
                End If
            ElseIf x.Name = "USPhotoID" Then
                If x.InnerText = "0" Then
                    EntitiesUS = False
                Else
                    EntitiesUS = True
                End If
            ElseIf x.Name = "USorForeignPhotoID" Then
                If x.InnerText = "0" Then
                    EntitiesFN = False
                Else
                    EntitiesFN = True
                End If
            ElseIf x.Name = "CustomerActivation" Then
                If x.InnerText = "0" Then
                    EntitiesAT = False
                Else
                    EntitiesAT = True
                End If
            End If
        Next
    End Sub
    Public Function SelectCard(ByVal oCust As clsCustomer) As String
        Dim iCardType = 3  'starts as reloadable
        If (oCust.IDType = "NX" Or oCust.IDType = "OX" Or
            oCust.IDType = "FX" Or oCust.IDType = "UX") Then
            iCardType = 1
        End If
        If (EntitiesBK And oCust.BackGroundCheckCode = 2) Then
            iCardType = 2
        End If
        If (EntitiesUS And oCust.IDType = "US") Then
        ElseIf (oCust.IDType <> "US") Then
            iCardType = 2
        End If
        If (EntitiesFN And (oCust.IDType = "FN" Or oCust.IDType = "US")) Then
            If (EntitiesAT And oCust.oFlags.CustACK = False) Then
                iCardType = 2
            End If
        ElseIf (oCust.IDType <> "FN" And oCust.IDType <> "US") Then
            iCardType = 2
        End If
        'Updated by Roshelle 11/27 - hardcoded CDType
        If (iCardType = 3) Then
            Return "IND"
        ElseIf (iCardType = 2) Then
            Return "IDO"
        ElseIf (iCardType = 1) Then
            Return "IDX"
        End If
    End Function
    Public Sub New()


    End Sub
End Class
