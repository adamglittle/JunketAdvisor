Public Class ConferenceReview
    Private _id As Integer
    Private _conferenceId As Integer
    Private _memberId As Integer
    Private _confRating As Integer
    Private _comments As String

    Public Property confRating As Integer
        Get
            Return _confRating
        End Get
        Set(value As Integer)
            _confRating = value
        End Set
    End Property
    Public Property comments As String
        Get
            Return _comments
        End Get
        Set(value As String)
            _comments = value
        End Set
    End Property
End Class
