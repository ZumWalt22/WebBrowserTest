Attribute VB_Name = "Module3"


Option Explicit

'�t�H���_���̃t�@�C������G�r�f���X���쐬����
Function SetAllEbience(testName As String)

    Dim basePass As String
    Dim fileName As String

    '�e�X�g�G�r�f���X�t�@�C����Dir���w�肷��
    basePass = "C:\Net\WebTest\WindowsFormsApplication1\bin\Release\Ebidence"
    
    '�e�X�g���̃V�[�g���쐬
    
    '�V�[�g�����łɑ��݂���ꍇ�͍폜
    Dim ws As Worksheet
    Dim flag As Boolean
    flag = False
    For Each ws In Worksheets
        If ws.Name = testName Then
            flag = True
            Exit For
        End If
    Next ws
    
    If flag = True Then
        Application.DisplayAlerts = False '�폜�m�F��OFF�ɂ���
        ActiveSheet.DELETE
        Sheets(testName).DELETE
        Application.DisplayAlerts = True
    End If
    
    Worksheets.Add
    ActiveSheet.Name = testName
    
    fileName = basePass & "\" & testName
    
    '��ʃG�r�f���X��\��t���i���s�O�j
    PastePicture CStr(fileName & "_0.bmp"), 2

    'DB�G�r�f���X��\��t���i���s�O�j
    CsvTableSet (GetFileStrList(fileName & "_0Db.csv"))
    
    
    '��ʃG�r�f���X��\��t���i���s��j
    PastePicture CStr(fileName & "_1.bmp"), 2
    
    
    'DB�G�r�f���X��\��t���i���s��j
    CsvTableSet (GetFileStrList(fileName & "_1Db.csv"))


    '�摜�Ԃ�2�s�󂯂�
    'PastePicture CStr(fileName), 2
  
End Function

'�摜��\��t����
Sub PastePicture(fileName As String, offset As Integer)
    Dim picture As Shape
    
    Set picture = ActiveSheet.Shapes.AddPicture( _
        fileName:=fileName, _
        LinkToFile:=False, SaveWithDocument:=True, _
        Left:=Selection.Left, Top:=Selection.Top, _
        Width:=0, Height:=0)

    picture.ScaleHeight 1!, msoTrue
    picture.ScaleWidth 1!, msoTrue
    'picture.height�̓|�C���g�P��
    '(�s�N�Z���P�ʂɕϊ�����ɂ�96/72���|����)
    MoveDown picture.Height, offset
End Sub

'�摜�͈͕̔�Cell�����Ɉړ�
Sub MoveDown(pt As Double, offset As Integer)
    Dim moved As Double
    
    moved = 0
    Do While moved <= pt
        'ActiveCell.height�̓|�C���g�P��
        moved = moved + ActiveCell.Height
        ActiveCell.offset(1, 0).Activate
    Loop
    ActiveCell.offset(offset, 0).Activate
End Sub

'�t�@�C����ǂݍ���
Function GetFileStrList(fileName As String)
    Dim buf() As Byte
    Dim tmp As Variant
    Dim i As Long
    Dim strWk As String


    Open fileName For Binary As #1
    ReDim buf(1 To LOF(1))  '---(1)
    Get #1, , buf           '---(2)
    Close #1
    
    'tmp = Split(StrConv(buf, vbUnicode), vbLf) '---(3)
    'GetFileStrList = tmp
    
    strWk = StrConv(buf, vbUnicode)
    
    GetFileStrList = strWk

End Function


'CSV�e�[�u���G�r�f���X����ݒ�
Function CsvTableSet(csvStr As String)

   Dim setStr As Variant
   Dim setStrList As Variant
   Dim setSpitStrList As Variant
   
   Dim frTop As Integer
   Dim frLeft As Integer
   
   Dim toTop As Integer
   Dim toLeft As Integer
   
   Dim frRow As Integer
   Dim toRow As Integer
   
   '���[�h�@0:�擾�e�[�u����� 1:�e�[�u�����ږ��̗�@2:�e�[�u������
   Dim setMode As Integer
   
   Dim crmCnt As Integer
   
   Dim setSpitStr As Variant
   
   setMode = 0

   '�N���b�v�{�[�h�̕����񃊃X�g��ݒ�
   'setStrList = getClipStrList()
   setStrList = Split(csvStr, vbCrLf)

   '�r���@�J�n���ݒ�
   frLeft = ActiveCell.Column
   

   For Each setStr In setStrList

    If setMode = 0 Then

        '�擾SQL��ݒ�
        Selection.Value = setStr
        
        If setStr <> "" Then
            ActiveCell.offset(1, 0).Select     '���s�ɂ����߂�
            
            '�r���@�J�n�s��ݒ�
            frRow = ActiveCell.Row
                    
            setMode = 1
        End If
        
     ElseIf setMode = 1 Then

        '�J�������̂�ݒ�i�F�t���j
        setSpitStrList = Split(setStr, ",")
        crmCnt = 0
        For Each setSpitStr In setSpitStrList
            Selection.Interior.ColorIndex = 44
            ActiveCell.Value = setSpitStr
            ActiveCell.offset(0, 1).Select
            crmCnt = crmCnt + 1
        Next
        toLeft = ActiveCell.Column - 1

        ActiveCell.offset(1, -crmCnt).Select     '���s�ɂ����߂� ��ʒu�����Z�b�g
        
        setMode = 2
     ElseIf setMode = 2 Then
     
        If setStr = Null Or setStr = "" Then
           '�e�[�u�����̏I���
        
           '�r���@�I���s��ݒ�
           toRow = ActiveCell.Row - 1

           '�͈͂�I��
           Range(Cells(frRow, frLeft), Cells(toRow, toLeft)).Select
             
           '�I��͈͂Ƀ��C��������
           With Selection.Borders
                .LineStyle = xlContinuous
                .Weight = xlThin
                .ColorIndex = xlAutomatic
           End With
        
            '�Ăі����Ɉړ�
            Cells(toRow + 1, frLeft).Activate
        
           setMode = 0         '���[�h�����Z�b�g
        End If
        
        '�e�[�u������ݒ�i��s������܂Łj
        setSpitStrList = Split(setStr, ",")
        crmCnt = 0
        For Each setSpitStr In setSpitStrList
            ActiveCell.Value = setSpitStr
            ActiveCell.offset(0, 1).Select
            crmCnt = crmCnt + 1
        Next
        ActiveCell.offset(1, -crmCnt).Select     '���s�ɂ����߂� ��ʒu�����Z�b�g

      End If
   Next
End Function
