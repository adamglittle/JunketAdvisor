Imports System.ServiceModel
Imports System.ServiceModel.Web
Imports System.ServiceModel.Description
Imports System.Text
Imports System.Threading
Imports System.Security.Principal
Imports System.Net
Imports System.ServiceModel.Security
Imports System.IdentityModel.Selectors
Imports System.Web.Script.Serialization
Imports System.Security



Public Class Form1
    Private fMembers As New Members

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try

            Dim host As WebServiceHost = New WebServiceHost(GetType(Service), New Uri("http://0.0.0.0/"))

            Dim binding As New WebHttpBinding()
            binding.Security.Mode = WebHttpSecurityMode.TransportCredentialOnly
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Basic
            binding.Security.Transport.Realm = "this realm"



            Dim ep As Description.ServiceEndpoint = host.AddServiceEndpoint(GetType(IService), binding, "http://localhost:8005/")

            host.Credentials.UserNameAuthentication.UserNamePasswordValidationMode = UserNamePasswordValidationMode.Custom
            host.Credentials.UserNameAuthentication.CustomUserNamePasswordValidator = New CustomUserNamePasswordValidator()

            host.Open()




        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Dim thisMember As Member = fMembers.GetMember("bob@email.com")
        thisMember.SetPassword("testpwd")

        thisMember = fMembers.GetMember("bob@email.com")

        If thisMember.CheckPassword("testpwd") Then
            ' MessageBox.Show("worked")
        End If
        If thisMember.CheckPassword("testpwd1") Then
            MessageBox.Show("worked")
        End If


        thisMember = New Member("adam1", "little", "altest@email.com", "")
        fMembers.AddMember(thisMember)


    End Sub

    Public Class CustomUserNamePasswordValidator
        Inherits UserNamePasswordValidator
        Private fMembers As New Members
        Public Overrides Sub Validate(userName As String, password As String)
            'Your logic to validate username/password
            Dim thisMember As Member = fMembers.GetMember(userName)
            If thisMember Is Nothing Then
                Throw New Authentication.AuthenticationException("access denied")

            End If
            If Not thisMember.CheckPassword(password) Then
                Throw New Authentication.AuthenticationException("access denied")
            End If
        End Sub
    End Class

End Class

<ServiceContract()> _
Public Interface IService
    '<OperationContract()> _
    '<WebGet()> _
    'Function EchoWithGet(ByVal s As String) As String

    <OperationContract()> _
     <WebGet(responseformat:=WebMessageFormat.Json)> _
    Function GetMember(ByVal s As String, ByVal callback As String) As IO.Stream

    <OperationContract()> _
    <WebInvoke()> _
    Function EchoWithPost(ByVal s As String) As String

    <OperationContract()> _
    <WebGet(responseformat:=WebMessageFormat.Json)> _
    Function GetCityImage(ByVal city As String, ByVal callback As String) As IO.Stream

End Interface

Public Class Service
    Implements IService
    'Public Function EchoWithGet(ByVal s As String) As String Implements IService.EchoWithGet
    '    Return "You said " + s
    'End Function

    Public Function GetMember(ByVal s As String, ByVal callback As String) As IO.Stream Implements IService.GetMember
        Dim strAuthHeader As String = WebOperationContext.Current.IncomingRequest.Headers.Get("Authorization")



        If strAuthHeader Is Nothing Then
            WebOperationContext.Current.OutgoingResponse.StatusCode = Net.HttpStatusCode.Unauthorized
            WebOperationContext.Current.OutgoingResponse.ContentType = "text"
            Return New IO.MemoryStream(Encoding.UTF8.GetBytes("Unauthorised"))
        End If

        Dim fMember As New Members
        Dim thisMember As Member = fMember.GetMember(s)

        Dim ser As New JavaScriptSerializer

        Dim dict = ser.Serialize(thisMember)


        WebOperationContext.Current.OutgoingResponse.ContentType = "application/javascript"
        Dim ms As New IO.MemoryStream(Encoding.UTF8.GetBytes(callback & "(" + dict.ToString + ")"))
        Return ms
    End Function

    Public Function EchoWithPost(ByVal s As String) As String Implements IService.EchoWithPost
        Return "You said " + s
    End Function

    Public Function GetCityImage(ByVal city As String, ByVal callback As String) As IO.Stream Implements IService.GetCityImage
        Dim fs As IO.FileStream = IO.File.OpenRead("..\..\images\" & city & ".jpg")
        Dim imageBytes(fs.Length - 1) As Byte
        fs.Read(imageBytes, 0, fs.Length - 1)
        Dim returnString As String = callback & "(""" & Convert.ToBase64String(imageBytes) & """)"
        WebOperationContext.Current.OutgoingResponse.ContentType = "application/javascript"
        Return New IO.MemoryStream(Encoding.UTF8.GetBytes(returnString))
    End Function

End Class