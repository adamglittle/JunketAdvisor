Public Class Members
    Public Sub AddMember(thisMember As Member)

    End Sub
    Public Function GetMember(id As Integer) As Member
        Dim thisMember As Member


        Return Nothing
    End Function
End Class
Public Class Member
    Private _id As Integer
    Private _firstName As String
    Private _lastName As String
    Private _emailAddress As String
    Private _role As String

    Public ReadOnly Property id As Integer
        Get
            Return _id
        End Get
    End Property
    Public Property firstName As String
        Get
            Return _firstName
        End Get
        Set(value As String)
            _firstName = value
        End Set
    End Property
    Public Property lastName As String
        Get
            Return _lastName
        End Get
        Set(value As String)
            _lastName = value
        End Set
    End Property
    Public Property emailAddress As String
        Get
            Return _emailAddress
        End Get
        Set(value As String)
            _emailAddress = value
        End Set
    End Property
    Public Property role As String
        Get
            Return _role
        End Get
        Set(value As String)
            _role = value
        End Set
    End Property

    Public Sub New()

    End Sub
    Public Sub New(firstName As String, lastName As String, emailAddress As String, role As String)
        _firstName = firstName
        _lastName = lastName
        _emailAddress = emailAddress
        _role = role
    End Sub
End Class