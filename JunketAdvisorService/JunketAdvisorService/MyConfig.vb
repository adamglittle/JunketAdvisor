Imports System.Configuration

Public Class MyConfig
    Public Shared Function GetConnectionStringByName( _
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
