VERSION 5.00
Begin VB.Form Form1 
   Caption         =   "Form1"
   ClientHeight    =   3135
   ClientLeft      =   60
   ClientTop       =   405
   ClientWidth     =   5415
   BeginProperty Font 
      Name            =   "Tahoma"
      Size            =   8.25
      Charset         =   222
      Weight          =   400
      Underline       =   0   'False
      Italic          =   0   'False
      Strikethrough   =   0   'False
   EndProperty
   LinkTopic       =   "Form1"
   ScaleHeight     =   3135
   ScaleWidth      =   5415
   ShowInTaskbar   =   0   'False
   StartUpPosition =   3  'Windows Default
   Begin VB.CommandButton cmdPipeSend 
      Caption         =   "SEND MESSAGE TO PIPE SERVER"
      Height          =   360
      Left            =   1440
      TabIndex        =   2
      Top             =   1200
      Width           =   3615
   End
   Begin VB.TextBox txtResult 
      Height          =   375
      Left            =   600
      TabIndex        =   1
      Text            =   "Result"
      Top             =   1920
      Width           =   4455
   End
   Begin VB.TextBox txtInput 
      Height          =   375
      Left            =   600
      TabIndex        =   0
      Text            =   "50"
      Top             =   720
      Width           =   4455
   End
   Begin VB.Label lblResultFrom 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Result From Pipe Server"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   222
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   195
      Left            =   120
      TabIndex        =   4
      Top             =   1560
      Width           =   2040
   End
   Begin VB.Label lblInputMessage 
      AutoSize        =   -1  'True
      BackStyle       =   0  'Transparent
      Caption         =   "Input Message"
      BeginProperty Font 
         Name            =   "Tahoma"
         Size            =   8.25
         Charset         =   222
         Weight          =   700
         Underline       =   0   'False
         Italic          =   0   'False
         Strikethrough   =   0   'False
      EndProperty
      Height          =   195
      Left            =   120
      TabIndex        =   3
      Top             =   360
      Width           =   1260
   End
End
Attribute VB_Name = "Form1"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit
   Private Const szPipeName = "\\.\\pipe\\invest-pipe", BUFFSIZE = 20000
   Private Declare Function CallNamedPipe Lib "kernel32" Alias _
      "CallNamedPipeA" ( _
      ByVal lpNamedPipeName As String, _
      lpInBuffer As Any, _
      ByVal nInBufferSize As Long, _
      lpOutBuffer As Any, _
      ByVal nOutBufferSize As Long, _
      lpBytesRead As Long, _
      ByVal nTimeOut As Long) As Long

Private Sub cmdPipeSend_Click()
    Dim message             As String
    Dim messageInByte()     As Byte
    Dim messageOutByte()    As Byte
    Dim cbRead              As Long
    Dim res                 As Long
    Dim temp                As String
    Dim i                   As Long
    Dim lmessageByteCount    As Long

    message = txtInput.Text
    
    'messageByte = StrConv(message, vbFromUnicode)          '<--- THIS CODE NOT WORKING (NOT COMPACT WITH .NET UNICODE
    
    'BEGIN: THIS CODE CONVERT EACH CHARACTOR TO ASCII FROM SUPPORT ENGLISH + THAI
    ReDim messageInByte(Len(message))
    lmessageByteCount = 1
    For lmessageByteCount = 1 To Len(message)
        messageInByte(lmessageByteCount) = Asc(Mid$(message, lmessageByteCount, 1))
    Next
    'END: THIS CODE CONVERT EACH CHARACTOR TO ASCII FROM SUPPORT ENGLISH + THAI
    
    ReDim messageOutByte(BUFFSIZE)  'Build the return buffer
     
    res = CallNamedPipe(szPipeName _
                      , messageInByte(0) _
                      , UBound(messageInByte) + 1 _
                      , messageOutByte(0) _
                      , UBound(messageOutByte) + 1 _
                      , cbRead _
                      , 30000)  'Wait up to 30 seconds for a response

    If res > 0 Then
        Dim c As Variant
        For Each c In messageOutByte
            temp = temp & Chr(c)
        Next c
        temp = Left$(temp, cbRead)
        txtResult.Text = temp
    Else
        MsgBox "Error number " & Err.LastDllError & " attempting to call CallNamedPipe.", vbOKOnly
    End If
End Sub

'OLD CODE
'Private Sub cmdSEND_Click()
'Dim res As Long, myStr As String, i As Long, cbRead As Long
'      Dim numBytes As Long, bArray() As Byte, temp As String
'
'      numBytes = txtInput.Text
'      If txtInput.Text < 0 Then
'         MsgBox "Value must be at least 0.", vbOKOnly
'         Exit Sub
'      End If
''
'      Dim message As String
'      Dim messageByte() As Byte
'      'message = txtInput.Text
'      'Convert string to bytes
'      messageByte = StrConv(message, vbFromUnicode)
'      If numBytes > BUFFSIZE Then
'         numBytes = BUFFSIZE
'      End If
'      ReDim bArray(numBytes)  'Build the return buffer
'
'      'Call CallNamedPipe to do the transaction all at once
''      res = CallNamedPipe(szPipeName, message, UBound(messageByte), _
''         bArray(0), numBytes, _
''         cbRead, 30000) 'Wait up to 30 seconds for a response
'
'      res = CallNamedPipe(szPipeName _
'                        , numBytes _
'                        , LenB(numBytes) _
'                        , bArray(0) _
'                        , numBytes _
'                        , cbRead _
'                        , 30000) 'Wait up to 30 seconds for a response
'
'      If res > 0 Then
'         temp = Format(bArray(0), " 000")
'         For i = 1 To cbRead - 1
'            If (i Mod 16) = 0 Then temp = temp & vbCrLf
'            temp = temp & " " & Format(bArray(i), "000")
'         Next i
'         txtResult.Text = temp
'      Else
'         MsgBox "Error number " & Err.LastDllError & _
'                " attempting to call CallNamedPipe.", vbOKOnly
'      End If
'End Sub
'OLD CODE
Private Sub txtInput_Change()

End Sub
