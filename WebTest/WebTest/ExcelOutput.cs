using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ExcelMng;

namespace WindowsFormsApplication1
{
    /* 環境依存のため使用しない
	class ExcelOutput
	{
		ExcelController excelController;

		string stCurrentDir;

		public ExcelOutput(ExcelController excelController)
		{
			this.excelController = excelController;

			// カレントディレクトリを取得する
			stCurrentDir = System.IO.Directory.GetCurrentDirectory() + "\\" + "Ebidence" + "\\";
		}

		/// <summary>
		/// Excel出力
		/// </summary>
		public void doOutput(WebTestDto webTestDto)
		{

			//シートが存在する場合は削除
			excelController.deleteSheet(webTestDto.testNo.ToString());

			//シートを追加
			excelController.addSheet(webTestDto.testNo.ToString());

			//テストNoを設定
			excelController.setValue(null
				, excelController.setX
				, excelController.setY
				, webTestDto.testNo.ToString());

			//テスト名称を設定
			excelController.setValue(null
				, excelController.setX + 1
				, excelController.setY
				, webTestDto.testName);

			//処理前　エビデンス
			webTestDto.ebidenceSetMode = 0;
			setEvidence(webTestDto);

			//処理後　エビデンス
			webTestDto.ebidenceSetMode = 1;
			setEvidence(webTestDto);

		}

		/// <summary>
		/// エビデンス　設定
		/// </summary>
		/// <param name="webTestDto"></param>
		/// <param name="stCurrentDir"></param>
		private void setEvidence(WebTestDto webTestDto)
		{
			////キャプチャファイル名を生成
			string testFileName = stCurrentDir + webTestDto.testNo + "_" + webTestDto.ebidenceSetMode + ".bmp";

			//画面エビデンスを貼り付け(コメント行分、１行下に貼り付ける)
			excelController.setImage(null
				, testFileName
				, excelController.setX
				, excelController.setY + 1 + excelController.setAddY * webTestDto.ebidenceSetMode
				, excelController.setW
				, excelController.setH);

			//変更後の場合、変更前の情報を取得
			string lineStr;
			string csvFileName;
			List<List<string>> fromCrmList = new List<List<string>>();
			if (webTestDto.ebidenceSetMode.Equals(1))
			{
				csvFileName = webTestDto.testNo + "_" + "0" + "Db.csv";
				using (StreamReader sr = new StreamReader(stCurrentDir + csvFileName,
					Encoding.GetEncoding("Shift_JIS")))
				{
					while ((lineStr = sr.ReadLine()) != null)
					{
						//カンマで分離
						fromCrmList.Add(lineStr.Split(',').ToList<string>());
					}
				}
			}

			// CSV形式のDBエビデンス情報をはりつけ
			int dbAddX = 1;
			int dbAddY = 1;
			csvFileName = webTestDto.testNo + "_" + webTestDto.ebidenceSetMode + "Db.csv";
			using (StreamReader sr = new StreamReader(stCurrentDir + csvFileName,
				Encoding.GetEncoding("Shift_JIS")))
			{
				while ((lineStr = sr.ReadLine()) != null)
				{
					dbAddX = 1;

					//カンマで分離
					string[] splitStr = lineStr.Split(',');
					foreach (var itemStr in splitStr)
					{
						//色設定(先頭行のみ)
						if (dbAddY.Equals(1))
						{
							excelController.setBackColor(null
								, excelController.setX + dbAddX
								, excelController.setY
								  + excelController.setAddY * webTestDto.ebidenceSetMode
								  + excelController.setDbAddY + dbAddY + 1
								, 125, 255, 255);
						}

						//値設定
						excelController.setValue(null
							, excelController.setX + dbAddX
							, excelController.setY
							  + excelController.setAddY * webTestDto.ebidenceSetMode
							  + excelController.setDbAddY + dbAddY + 1
							, itemStr);

						//値に変化がある場合色つけ
						if (fromCrmList.Count < dbAddY)
						{
							if (fromCrmList[dbAddY - 1][dbAddX - 1].Equals(itemStr))
							{
								excelController.setBackColor(null
									, excelController.setX + dbAddX
									, excelController.setY
									  + excelController.setAddY * webTestDto.ebidenceSetMode
									  + excelController.setDbAddY + dbAddY + 1
									, 125, 125, 255);
							}
						}

						dbAddX++;
					}

					dbAddY++;
				}
			}
		}
	}

*/
}
