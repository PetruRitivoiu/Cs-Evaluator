#include<iostream>
#include<fstream>
#include<sstream>

using namespace std;


void main(int argc, char *argv[]) {

	const int BUFFER_SIZE = 255;


	if (argc == 1)
	{
		cout << "error";
	}
	else
	{
		int arrLength = 0;
		char arr1[BUFFER_SIZE];
		char arr2[BUFFER_SIZE];

		int arr_int1[100];
		int arr_int2[100];

		//read array length
		//ifstream file("C:/Users/thinkpad-e560/Documents/Visual Studio 2017/Projects/cs-evaluator/EvaluatorEngine/uploads/validation_files/1/initial.txt");
		ifstream file(argv[1]);


		if (file.is_open())
		{
			file >> arrLength;

			file.getline(arr1, 1);

			file.getline(arr1, BUFFER_SIZE);

			file.getline(arr2, BUFFER_SIZE);
		}


		//tokenizare
		char *token1 = std::strtok(arr1, ",");

		int i = 0;
		while (token1 != NULL) {
			arr_int1[i] = atoi(token1);
			i++;
			token1 = std::strtok(NULL, ",");
		}


		char *token2 = std::strtok(arr2, ",");

		int j = 0;
		while (token2 != NULL) {
			arr_int2[j] = atoi(token2);
			j++;
			token2 = std::strtok(NULL, ",");
		}


		int result = 0;
		for (int i = 0; i < arrLength; i++)
		{
			result += arr_int1[i] * arr_int2[i];
		}

		cout << result;
	}




}
