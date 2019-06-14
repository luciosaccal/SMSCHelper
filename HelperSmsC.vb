'SMSC Api

'@author Moises Saccal
'@link https://smsc.com.ar/ Smsc
'@link https://github.com/smsc/smsc-api-php Smsc on GitHub
'@version 1.0.1

Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Class HelperSmsC
    Private _key As String
    Private _alias As String

    'Constructor
    Sub New(xalias As String, xkey As String)
        _key = xkey
        _alias = xalias
    End Sub
    'Envia SMS
    Public Function EnviarSms(numero As Long, mensaje As String) As Sms_Enviado
        Try
            Dim res = Ejecutar(Of Sms_Enviado)("enviar", "num=" & numero & "&msj=" & mensaje)
            If res.code = 200 Then
                Return res
            Else
                Throw New ApplicationException("fallo envio mensaje " & res.code & " " & res.message)
            End If

        Catch ex As Exception
            Throw New ApplicationException("fallo envio mensaje " & ex.Message.ToString)
        End Try

    End Function
    'Evalua un numero
    Public Function EvalNum(num As String) As Sms_Eval
        Return Ejecutar(Of Sms_Eval)("evalnumero", "num=" & num)
    End Function
    'Devuelve el Saldo
    Public Function Saldo() As Sms_Saldo
        Return Ejecutar(Of Sms_Saldo)("saldo", "")
    End Function
    Public Function Encolados(prioridad As Integer) As Sms_Encolados

        Return Ejecutar(Of Sms_Encolados)("encolados", "prioridad=" & prioridad)

    End Function

    Private Function Ejecutar(Of T)(cmd As String, acc As String) As T
        Dim wc As New System.Net.WebClient()
        Dim _base As String = "https://www.smsc.com.ar/api/0.3/?alias=" & _alias & "&apikey=" & _key & "&cmd=" & cmd & "&" & acc
        Dim res = wc.DownloadString(_base)
        Return JsonConvert.DeserializeObject(Of T)(res)
    End Function

    Public Class Sms_Encolados
        Inherits Sms_Result_Basic
        Public Property data As New Sms_Encolados_Data
        Public Class Sms_Encolados_Data
            Public Property mensajes As Integer
        End Class
    End Class
    Public Class Sms_Eval
        Inherits Sms_Result_Basic
        Public Property data As New Sms_Eval_Data
        Public Class Sms_Eval_Data
            Public Property estado As Boolean
        End Class
    End Class
    Public Class Sms_Estado

        Inherits Sms_Result_Basic

        Public Property data As New Sms_Estado_Data
        Public Class Sms_Estado_Data

            Public Property linea_smsc As Boolean
            Public Property daemon As Boolean
            Public Property estado As Boolean
        End Class

    End Class
    Public Class Sms_Saldo

        Inherits Sms_Result_Basic
        Public Property data As New Sms_Saldo_Data
        Public Class Sms_Saldo_Data
            Public Property mensajes As Integer
        End Class
    End Class
    Public Class Sms_Enviado
        Inherits Sms_Result_Basic
        Public Property data As New Sms_Enviado_Data
        Public Class Sms_Enviado_Data
            Public Property id As Long
            Public Property sms As Integer
        End Class
    End Class
    Public Class Sms_Result_Basic
        Public Property code As Integer
        Public Property message As String

    End Class
End Class
