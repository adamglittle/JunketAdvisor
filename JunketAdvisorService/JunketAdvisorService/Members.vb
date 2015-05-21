Imports System.Configuration
Imports System.Data.SqlClient
Public Class Members
    Public Sub AddMember(thisMember As Member)

    End Sub
    Public Function GetMember(id As Integer) As Member
        Dim thisMember As Member


        Return Nothing
    End Function
    Public Function GetMember(email As String) As Member
        Dim thisMember As New Member

        Dim conn As New SqlConnection(GetConnectionStringByName("JunketAdvisorService.My.MySettings.JaDbConnectionString"))

        ' if this member is not in the db then add it
        Dim cmd As New SqlCommand("select * from members where emailAddress = @email", conn)
        cmd.Parameters.Add("@email", SqlDbType.VarChar, 50).Value = email

        Dim da As New SqlDataAdapter(cmd)
        Dim dt As New DataTable
        Try
            da.Fill(dt)
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        For Each dr As DataRow In dt.Rows
            thisMember.id = dr("id")
            thisMember.emailAddress = CType(dr("emailAddress"), String).Trim
            thisMember.firstName = CType(dr("firstName"), String).Trim
            thisMember.lastName = CType(dr("lastName"), String).Trim
            Return thisMember
        Next


        Return Nothing
    End Function

    Private Sub PersistMember(thisMember As Member)
        Dim conn As New SqlConnection(GetConnectionStringByName("JunketAdvisorService.My.MySettings.JaDbConnectionString"))
        Dim da As New SqlDataAdapter("select * from members", conn)

        ' if this member is not in the db then add it
        Dim cmd As New SqlCommand("select * from members where emailAddress = @email", conn)
        cmd.Parameters.Add("@email", SqlDbType.VarChar, 50).Value = thisMember.emailAddress

        If cmd.ExecuteReader.HasRows Then

        Else
            Dim query As String = String.Empty
            query &= "INSERT INTO member (emailAddress, firstName, lastName, "
            query &= "                     role)  "
            query &= "VALUES (@emailAddress,@firstName, @lastName, @role)"

            Using comm As New SqlCommand()
                With comm
                    .Connection = conn
                    .CommandType = CommandType.Text
                    .CommandText = query
                    .Parameters.AddWithValue("@emailAddress", thisMember.emailAddress)
                    .Parameters.AddWithValue("@firstName", thisMember.firstName)
                    .Parameters.AddWithValue("@lastName", thisMember.lastName)
                    .Parameters.AddWithValue("@role", thisMember.role)
                End With
                Try
                    conn.Open()
                    comm.ExecuteNonQuery()
                Catch ex As SqlException
                    MessageBox.Show(ex.Message.ToString(), "Error Message")
                End Try
            End Using

        End If
    End Sub
    Private Shared Function GetConnectionStringByName( _
    ByVal name As String) As String

        ' Assume failure 
        Dim returnValue As String = Nothing

        ' Look for the name in the connectionStrings section. 
        Dim settings As ConnectionStringSettings = _
           ConfigurationManager.ConnectionStrings(name)

        ' If found, return the connection string. 
        If Not settings Is Nothing Then
            returnValue = settings.ConnectionString
        End If

        Return returnValue
    End Function
End Class
<Serializable> _
Public Class Member
    Private _id As Integer
    Private _firstName As String
    Private _lastName As String
    Private _emailAddress As String
    Private _role As String

    Public Property id As Integer
        Get
            Return _id
        End Get
        Set(value As Integer)
            _id = value
        End Set
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