<?php
	
	// make sure you have a folder called CSV in the same directory with this PHP file
	$nameOfFolder = "CSV";

	// reads all the CSVs in this directory
	readDirectory($nameOfFolder);

	function readDirectory ($nameOfFolder) {
		
		$result = array();

		// check if the folder is a directory
		if (is_dir($nameOfFolder)) {
			if ($dh = opendir($nameOfFolder)) {
				while (($filename = readdir($dh)) !== false) {
					
					// find the position of the . so that I can get the filename
					$finddot = strpos($filename, ".");;

					// the name of the file = $table name. I do a substring to get filename
					$tableName = substr($filename, 0, $finddot);

					// last 3 charcter is file extension
					$file_extension = substr($filename, -3);

					// check if the file is a csv before reading it
					if ($file_extension == "csv"):
						$result[$tableName] = readCSV ($nameOfFolder."/".$filename);
					endif;
				}
				closedir($dh);
			}
		}

		// show the JSON file
		$json = json_encode($result);		// encode the json file
		//$decode = json_decode ($json);	// sample code to decode the json file

		echo $json;
		file_put_contents("JSONData.json", $json);

		//debug ($result);
		//debug ($decode);
	}

	function readCSV ( $filename) {
		$count = 0;
		$rowCount = 0;

		$columnHeader = array();
		$data = array();
		$result = array();

		if (($handle = fopen($filename, "r")) !== FALSE) {
			while (($datarow = fgetcsv($handle, 1000, ",")) !== FALSE) {
				
				// assuming that the first row of the CSV is always the column names
				if ($count == 0):		
					$rowCount = sizeof($datarow);

					// remove wild characters in csv
					$datarow[0] = preg_replace('/\xEF\xBB\xBF/', '', $datarow[0]);
					$columnHeader = $datarow;

				else:

					// assign the value to the column name
					for ($i=0; $i<$rowCount; $i++):
						$data[$columnHeader[$i]] = $datarow[$i];
					endfor;

					//debug ($datarow);
					$result[] = $data;
					$data = array(); // reset data
				endif;

				$count ++;
			}
		
			fclose($handle);
		}

		return $result;
	}

	function debug ($array){
		echo "<pre>";
		print_r($array);
		echo "</pre>";
	}
?>