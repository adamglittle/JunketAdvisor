Public Class Conferences
    Public Sub AddConference(thisConf As Conference)

    End Sub
    Public Function GetConference(id As Integer) As Conference
        Return Nothing
    End Function
End Class
Public Class Conference
    Private _id As Integer
    Private _name As String
    Private _year As String
    Private _location As String
    Private _date As String

    Public ReadOnly Property id As String
        Get
            Return _id
        End Get
    End Property
    Public Property name As String
        Get
            Return _name
        End Get
        Set(value As String)
            _name = value
        End Set
    End Property
    Public Property year As String
        Get
            Return _year
        End Get
        Set(value As String)
            _year = value
        End Set
    End Property
    Public Property location As String
        Get
            Return _location
        End Get
        Set(value As String)
            _location = value
        End Set
    End Property
    Public Property confDate As String
        Get
            Return _date
        End Get
        Set(value As String)
            _date = value
        End Set
    End Property

    Public Sub New()

    End Sub
    Public Sub New(name As String, year As String, location As String, confDate As String)
        _name = name
        _year = year
        _location = location
        _date = confDate
    End Sub
End Class