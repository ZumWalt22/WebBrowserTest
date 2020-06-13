Attribute VB_Name = "Module3"


Option Explicit

'フォルダ内のファイルからエビデンスを作成する
Function SetAllEbience(testName As String)

    Dim basePass As String
    Dim fileName As String

    'テストエビデンスファイルのDirを指定する
    basePass = "C:\Net\WebTest\WindowsFormsApplication1\bin\Release\Ebidence"
    
    'テスト名のシートを作成
    
    'シートがすでに存在する場合は削除
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
        Application.DisplayAlerts = False '削除確認をOFFにする
        ActiveSheet.DELETE
        Sheets(testName).DELETE
        Application.DisplayAlerts = True
    End If
    
    Worksheets.Add
    ActiveSheet.Name = testName
    
    fileName = basePass & "\" & testName
    
    '画面エビデンスを貼り付け（実行前）
    PastePicture CStr(fileName & "_0.bmp"), 2

    'DBエビデンスを貼り付け（実行前）
    CsvTableSet (GetFileStrList(fileName & "_0Db.csv"))
    
    
    '画面エビデンスを貼り付け（実行後）
    PastePicture CStr(fileName & "_1.bmp"), 2
    
    
    'DBエビデンスを貼り付け（実行後）
    CsvTableSet (GetFileStrList(fileName & "_1Db.csv"))


    '画像間を2行空ける
    'PastePicture CStr(fileName), 2
  
End Function

'画像を貼り付ける
Sub PastePicture(fileName As String, offset As Integer)
    Dim picture As Shape
    
    Set picture = ActiveSheet.Shapes.AddPicture( _
        fileName:=fileName, _
        LinkToFile:=False, SaveWithDocument:=True, _
        Left:=Selection.Left, Top:=Selection.Top, _
        Width:=0, Height:=0)

    picture.ScaleHeight 1!, msoTrue
    picture.ScaleWidth 1!, msoTrue
    'picture.heightはポイント単位
    '(ピクセル単位に変換するには96/72を掛ける)
    MoveDown picture.Height, offset
End Sub

'画像の範囲分Cellを下に移動
Sub MoveDown(pt As Double, offset As Integer)
    Dim moved As Double
    
    moved = 0
    Do While moved <= pt
        'ActiveCell.heightはポイント単位
        moved = moved + ActiveCell.Height
        ActiveCell.offset(1, 0).Activate
    Loop
    ActiveCell.offset(offset, 0).Activate
End Sub

'ファイルを読み込み
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


'CSVテーブルエビデンス情報を設定
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
   
   'モード　0:取得テーブル情報 1:テーブル項目名称列　2:テーブル情報列
   Dim setMode As Integer
   
   Dim crmCnt As Integer
   
   Dim setSpitStr As Variant
   
   setMode = 0

   'クリップボードの文字列リストを設定
   'setStrList = getClipStrList()
   setStrList = Split(csvStr, vbCrLf)

   '罫線　開始列を設定
   frLeft = ActiveCell.Column
   

   For Each setStr In setStrList

    If setMode = 0 Then

        '取得SQLを設定
        Selection.Value = setStr
        
        If setStr <> "" Then
            ActiveCell.offset(1, 0).Select     '次行にすすめる
            
            '罫線　開始行を設定
            frRow = ActiveCell.Row
                    
            setMode = 1
        End If
        
     ElseIf setMode = 1 Then

        'カラム名称を設定（色付け）
        setSpitStrList = Split(setStr, ",")
        crmCnt = 0
        For Each setSpitStr In setSpitStrList
            Selection.Interior.ColorIndex = 44
            ActiveCell.Value = setSpitStr
            ActiveCell.offset(0, 1).Select
            crmCnt = crmCnt + 1
        Next
        toLeft = ActiveCell.Column - 1

        ActiveCell.offset(1, -crmCnt).Select     '次行にすすめる 列位置をリセット
        
        setMode = 2
     ElseIf setMode = 2 Then
     
        If setStr = Null Or setStr = "" Then
           'テーブル情報の終わり
        
           '罫線　終了行を設定
           toRow = ActiveCell.Row - 1

           '範囲を選択
           Range(Cells(frRow, frLeft), Cells(toRow, toLeft)).Select
             
           '選択範囲にラインを引く
           With Selection.Borders
                .LineStyle = xlContinuous
                .Weight = xlThin
                .ColorIndex = xlAutomatic
           End With
        
            '再び末尾に移動
            Cells(toRow + 1, frLeft).Activate
        
           setMode = 0         'モードをリセット
        End If
        
        'テーブル情報を設定（空行があるまで）
        setSpitStrList = Split(setStr, ",")
        crmCnt = 0
        For Each setSpitStr In setSpitStrList
            ActiveCell.Value = setSpitStr
            ActiveCell.offset(0, 1).Select
            crmCnt = crmCnt + 1
        Next
        ActiveCell.offset(1, -crmCnt).Select     '次行にすすめる 列位置をリセット

      End If
   Next
End Function
