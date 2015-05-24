Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Text

Friend Class Members
    Public Sub AddMember(thisMember As Member)
        PersistMember(thisMember)
    End Sub
    Public Function GetMember(id As Integer) As Member
        Dim thisMember As Member


        Return Nothing
    End Function
    Public Function GetMember(email As String) As Member
        Dim thisMember As New Member

        Dim conn As New SqlConnection(MyConfig.GetConnectionStringByName("JunketAdvisorService.My.MySettings.JaDbConnectionString"))

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
            If Not IsDBNull(dr("pwd")) Then
                thisMember.pwd = dr("pwd")
            End If
            Return thisMember
        Next


        Return Nothing
    End Function

    Private Sub PersistMember(thisMember As Member)
        Dim conn As New SqlConnection(MyConfig.GetConnectionStringByName("JunketAdvisorService.My.MySettings.JaDbConnectionString"))
        Dim da As New SqlDataAdapter("select * from members", conn)

        ' if this member is not in the db then add it
        Dim cmd As New SqlCommand("select id from members where emailAddress = @email", conn)
        cmd.Parameters.Add("@email", SqlDbType.VarChar, 50).Value = thisMember.emailAddress

        Dim dr As SqlDataReader

        Try
            conn.Open()
            dr = cmd.ExecuteReader
            If dr.HasRows Then
                dr.Read()
                thisMember.id = dr.GetInt32(0)
                conn.Close()

                Dim query As String = String.Empty
                query &= "Update members set emailAddress = @emailAddress, firstName = @firstName, lastName = @lastName, role = @role "
                query &= "where id = @id"

                Using comm As New SqlCommand()
                    With comm
                        .Connection = conn
                        .CommandType = CommandType.Text
                        .CommandText = query
                        .Parameters.AddWithValue("@emailAddress", thisMember.emailAddress)
                        .Parameters.AddWithValue("@firstName", thisMember.firstName)
                        .Parameters.AddWithValue("@lastName", thisMember.lastName)
                        .Parameters.AddWithValue("@role", thisMember.role)
                        .Parameters.AddWithValue("@id", thisMember.id)
                    End With
                    Try
                        If Not conn.State = ConnectionState.Open Then conn.Open()
                        comm.ExecuteNonQuery()
                        conn.Close()
                    Catch ex As SqlException
                        MessageBox.Show(ex.Message.ToString(), "Error Message")
                    End Try
                End Using
            Else
                conn.Close()

                Dim query As String = String.Empty
                query &= "INSERT INTO members (emailAddress, firstName, lastName, "
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
                        If Not conn.State = ConnectionState.Open Then conn.Open()
                        comm.ExecuteNonQuery()
                        conn.Close()
                    Catch ex As SqlException
                        MessageBox.Show(ex.Message.ToString(), "Error Message")
                    End Try
                End Using

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
  
End Class
<Serializable> _
Friend Class Member
    Private _id As Integer
    Private _firstName As String
    Private _lastName As String
    Private _emailAddress As String
    Private _role As String
    Private _pwd As Byte()

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
    Public Property pwd As Byte()
        Get
            Return _pwd
        End Get
        Set(value As Byte())
            _pwd = value
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

    Public Function CheckPassword(plainPwd As String) As Boolean
        Dim hsh As New MD5CryptoServiceProvider
        If Me._pwd.SequenceEqual(CreateHash(plainPwd)) Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub SetPassword(plainPwd As String)
        Dim conn As New SqlConnection(MyConfig.GetConnectionStringByName("JunketAdvisorService.My.MySettings.JaDbConnectionString"))

        Dim query As String = String.Empty
        query &= "Update members set pwd = @pwd where id = @id"

        Using comm As New SqlCommand()
            With comm
                .Connection = conn
                .CommandType = CommandType.Text
                .CommandText = query
                .Parameters.AddWithValue("@pwd", Me.CreateHash(plainPwd))
                .Parameters.AddWithValue("@id", Me._id)
            End With
            Try
                conn.Open()
                comm.ExecuteNonQuery()
                conn.Close()
            Catch ex As SqlException
                MessageBox.Show(ex.Message.ToString(), "Error Message")
            End Try
        End Using

    End Sub
    Public Function CreateHash(plainPwd As String) As Byte()
        Dim hsh As New MD5CryptoServiceProvider
        Return hsh.ComputeHash(Encoding.UTF8.GetBytes(plainPwd & Me._id.ToString))
    End Function
End Class