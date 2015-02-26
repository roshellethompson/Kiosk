Public Class clsListItem
    Inherits ListControl
    Dim iIndex As Integer = 0
    Dim strName As String
    Dim strValue As String
    Public Property Value As String
        Get
            Return strValue
        End Get
        Set(ByVal value As String)
            strValue = value
        End Set
    End Property
    Protected Overrides Sub RefreshItem(ByVal index As Integer)

    End Sub

    Public Overrides Property SelectedIndex As Integer
        Get
            Return iIndex
        End Get
        Set(ByVal value As Integer)
            iIndex = value
        End Set
    End Property

    Protected Overrides Sub SetItemsCore(ByVal items As System.Collections.IList)

    End Sub

    Public Sub New(ByVal strName As String, ByVal strValue As String)
        Text = strName
        Value = strValue
    End Sub
End Class
